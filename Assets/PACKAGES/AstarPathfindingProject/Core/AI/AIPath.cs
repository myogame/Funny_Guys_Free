using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pathfinding {
	using Pathfinding.RVO;
	using Pathfinding.Util;

	[AddComponentMenu("Pathfinding/AI/AIPath (2D,3D)")]
	public partial class AIPath : AIBase, IAstarAI {

		
		public float maxAcceleration = -2.5f;
		[UnityEngine.Serialization.FormerlySerializedAs("turningSpeed")]
		public float rotationSpeed = 360;
		public float slowdownDistance = 0.6F;
		public float pickNextWaypointDist = 2;
		public float endReachedDistance = 0.2F;
		public bool alwaysDrawGizmos;
		public bool slowWhenNotFacingTarget = true;
		public CloseToDestinationMode whenCloseToDestination = CloseToDestinationMode.Stop;
		public bool constrainInsideGraph = false;
		protected Path path;
		protected PathInterpolator interpolator = new PathInterpolator();

		#region IAstarAI implementation
		public override void Teleport (Vector3 newPosition, bool clearPath = true) {
			reachedEndOfPath = false;
			base.Teleport(newPosition, clearPath);
		}
		public float remainingDistance {
			get {
				return interpolator.valid ? interpolator.remainingDistance + movementPlane.ToPlane(interpolator.position - position).magnitude : float.PositiveInfinity;
			}
		}

		

		

		public bool reachedDestination {
			get {
				if (!reachedEndOfPath) return false;
				if (remainingDistance + movementPlane.ToPlane(destination - interpolator.endPoint).magnitude > endReachedDistance) return false;

				// Don't do height checks in 2D mode
				if (orientation != OrientationMode.YAxisForward) {
					// Check if the destination is above the head of the character or far below the feet of it
					float yDifference;
					movementPlane.ToPlane(destination - position, out yDifference);
					var h = tr.localScale.y * height;
					if (yDifference > h || yDifference < -h*0.5) return false;
				}

				return true;
			}
		}
		public bool reachedEndOfPath { get; protected set; }
		public bool hasPath {
			get {
				return interpolator.valid;
			}
		}
		public bool pathPending {
			get {
				return waitingForPathCalculation;
			}
		}
		public Vector3 steeringTarget {
			get {
				return interpolator.valid ? interpolator.position : position;
			}
		}
		float IAstarAI.radius { get { return radius; } set { radius = value; } }
		float IAstarAI.height { get { return height; } set { height = value; } }
		float IAstarAI.maxSpeed { get { return maxSpeed; } set { maxSpeed = value; } }
		bool IAstarAI.canSearch { get { return canSearch; } set { canSearch = value; } }
		bool IAstarAI.canMove { get { return canMove; } set { canMove = value; } }

		#endregion

		protected override void OnDisable () {
			base.OnDisable();

			// Release current path so that it can be pooled
			if (path != null) path.Release(this);
			path = null;
			interpolator.SetPath(null);
		}
		public virtual void OnTargetReached () {
		}
		protected override void OnPathComplete (Path newPath) {
			ABPath p = newPath as ABPath;

			if (p == null) throw new System.Exception("This function only handles ABPaths, do not use special path types");

			waitingForPathCalculation = false;
			p.Claim(this);
			if (p.error) {
				p.Release(this);
				return;
			}
			if (path != null) path.Release(this);
			path = p;

			if (path.vectorPath.Count == 1) path.vectorPath.Add(path.vectorPath[0]);
			interpolator.SetPath(path.vectorPath);

			var graph = path.path.Count > 0 ? AstarData.GetGraph(path.path[0]) as ITransformedGraph : null;
			movementPlane = graph != null ? graph.transform : (orientation == OrientationMode.YAxisForward ? new GraphTransform(Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(-90, 270, 90), Vector3.one)) : GraphTransform.identityTransform);
			reachedEndOfPath = false;
			interpolator.MoveToLocallyClosestPoint((GetFeetPosition() + p.originalStartPoint) * 0.5f);
			interpolator.MoveToLocallyClosestPoint(GetFeetPosition());
			interpolator.MoveToCircleIntersection2D(position, pickNextWaypointDist, movementPlane);

			var distanceToEnd = remainingDistance;
			if (distanceToEnd <= endReachedDistance) {
				reachedEndOfPath = true;
				OnTargetReached();
			}
		}

		protected override void ClearPath () {
			CancelCurrentPathRequest();
			interpolator.SetPath(null);
			reachedEndOfPath = false;
		}

		/// <summary>Called during either Update or FixedUpdate depending on if rigidbodies are used for movement or not</summary>
		protected override void MovementUpdateInternal (float deltaTime, out Vector3 nextPosition, out Quaternion nextRotation) {
			float currentAcceleration = maxAcceleration;

			// If negative, calculate the acceleration from the max speed
			if (currentAcceleration < 0) currentAcceleration *= -maxSpeed;

			if (updatePosition) {
				// Get our current position. We read from transform.position as few times as possible as it is relatively slow
				// (at least compared to a local variable)
				simulatedPosition = tr.position;
			}
			if (updateRotation) simulatedRotation = tr.rotation;

			var currentPosition = simulatedPosition;

			// Update which point we are moving towards
			interpolator.MoveToCircleIntersection2D(currentPosition, pickNextWaypointDist, movementPlane);
			var dir = movementPlane.ToPlane(steeringTarget - currentPosition);

			// Calculate the distance to the end of the path
			float distanceToEnd = dir.magnitude + Mathf.Max(0, interpolator.remainingDistance);

			// Check if we have reached the target
			var prevTargetReached = reachedEndOfPath;
			reachedEndOfPath = distanceToEnd <= endReachedDistance && interpolator.valid;
			if (!prevTargetReached && reachedEndOfPath) OnTargetReached();
			float slowdown;

			// Normalized direction of where the agent is looking
			var forwards = movementPlane.ToPlane(simulatedRotation * (orientation == OrientationMode.YAxisForward ? Vector3.up : Vector3.forward));

			// Check if we have a valid path to follow and some other script has not stopped the character
			if (interpolator.valid && !isStopped) {
				// How fast to move depending on the distance to the destination.
				// Move slower as the character gets closer to the destination.
				// This is always a value between 0 and 1.
				slowdown = distanceToEnd < slowdownDistance ? Mathf.Sqrt(distanceToEnd / slowdownDistance) : 1;

				if (reachedEndOfPath && whenCloseToDestination == CloseToDestinationMode.Stop) {
					// Slow down as quickly as possible
					velocity2D -= Vector2.ClampMagnitude(velocity2D, currentAcceleration * deltaTime);
				} else {
					velocity2D += MovementUtilities.CalculateAccelerationToReachPoint(dir, dir.normalized*maxSpeed, velocity2D, currentAcceleration, rotationSpeed, maxSpeed, forwards) * deltaTime;
				}
			} else {
				slowdown = 1;
				// Slow down as quickly as possible
				velocity2D -= Vector2.ClampMagnitude(velocity2D, currentAcceleration * deltaTime);
			}

			velocity2D = MovementUtilities.ClampVelocity(velocity2D, maxSpeed, slowdown, slowWhenNotFacingTarget && enableRotation, forwards);

			ApplyGravity(deltaTime);

			if (rvoController != null && rvoController.enabled) {
				
				var rvoTarget = currentPosition + movementPlane.ToWorld(Vector2.ClampMagnitude(velocity2D, distanceToEnd), 0f);
				rvoController.SetTarget(rvoTarget, velocity2D.magnitude, maxSpeed);
			}

			// Set how much the agent wants to move during this frame
			var delta2D = lastDeltaPosition = CalculateDeltaToMoveThisFrame(movementPlane.ToPlane(currentPosition), distanceToEnd, deltaTime);
			nextPosition = currentPosition + movementPlane.ToWorld(delta2D, verticalVelocity * lastDeltaTime);
			CalculateNextRotation(slowdown, out nextRotation);
		}

		protected virtual void CalculateNextRotation (float slowdown, out Quaternion nextRotation) {
			if (lastDeltaTime > 0.00001f && enableRotation) {
				Vector2 desiredRotationDirection;
				if (rvoController != null && rvoController.enabled) {
					
					var actualVelocity = lastDeltaPosition/lastDeltaTime;
					desiredRotationDirection = Vector2.Lerp(velocity2D, actualVelocity, 4 * actualVelocity.magnitude / (maxSpeed + 0.0001f));
				} else {
					desiredRotationDirection = velocity2D;
				}

				// Rotate towards the direction we are moving in.
				// Don't rotate when we are very close to the target.
				var currentRotationSpeed = rotationSpeed * Mathf.Max(0, (slowdown - 0.3f) / 0.7f);
				nextRotation = SimulateRotationTowards(desiredRotationDirection, currentRotationSpeed * lastDeltaTime);
			} else {
				// TODO: simulatedRotation
				nextRotation = rotation;
			}
		}

		static NNConstraint cachedNNConstraint = NNConstraint.Default;
		protected override Vector3 ClampToNavmesh (Vector3 position, out bool positionChanged) {
			if (constrainInsideGraph) {
				cachedNNConstraint.tags = seeker.traversableTags;
				cachedNNConstraint.graphMask = seeker.graphMask;
				cachedNNConstraint.distanceXZ = true;
				var clampedPosition = AstarPath.active.GetNearest(position, cachedNNConstraint).position;

				// We cannot simply check for equality because some precision may be lost
				// if any coordinate transformations are used.
				var difference = movementPlane.ToPlane(clampedPosition - position);
				float sqrDifference = difference.sqrMagnitude;
				if (sqrDifference > 0.001f*0.001f) {
					// The agent was outside the navmesh. Remove that component of the velocity
					// so that the velocity only goes along the direction of the wall, not into it
					velocity2D -= difference * Vector2.Dot(difference, velocity2D) / sqrDifference;

					// Make sure the RVO system knows that there was a collision here
					// Otherwise other agents may think this agent continued
					// to move forwards and avoidance quality may suffer
					if (rvoController != null && rvoController.enabled) {
						rvoController.SetCollisionNormal(difference);
					}
					positionChanged = true;
					// Return the new position, but ignore any changes in the y coordinate from the ClampToNavmesh method as the y coordinates in the navmesh are rarely very accurate
					return position + movementPlane.ToWorld(difference);
				}
			}

			positionChanged = false;
			return position;
		}

	#if UNITY_EDITOR
		[System.NonSerialized]
		int gizmoHash = 0;

		[System.NonSerialized]
		float lastChangedTime = float.NegativeInfinity;

		protected static readonly Color GizmoColor = new Color(46.0f/255, 104.0f/255, 201.0f/255);

		protected override void OnDrawGizmos () {
			base.OnDrawGizmos();
			if (alwaysDrawGizmos) OnDrawGizmosInternal();
		}

		protected override void OnDrawGizmosSelected () {
			base.OnDrawGizmosSelected();
			if (!alwaysDrawGizmos) OnDrawGizmosInternal();
		}

		void OnDrawGizmosInternal () {
			var newGizmoHash = pickNextWaypointDist.GetHashCode() ^ slowdownDistance.GetHashCode() ^ endReachedDistance.GetHashCode();

			if (newGizmoHash != gizmoHash && gizmoHash != 0) lastChangedTime = Time.realtimeSinceStartup;
			gizmoHash = newGizmoHash;
			float alpha = alwaysDrawGizmos ? 1 : Mathf.SmoothStep(1, 0, (Time.realtimeSinceStartup - lastChangedTime - 5f)/0.5f) * (UnityEditor.Selection.gameObjects.Length == 1 ? 1 : 0);

			if (alpha > 0) {
				// Make sure the scene view is repainted while the gizmos are visible
				if (!alwaysDrawGizmos) UnityEditor.SceneView.RepaintAll();
				Draw.Gizmos.Line(position, steeringTarget, GizmoColor * new Color(1, 1, 1, alpha));
				Gizmos.matrix = Matrix4x4.TRS(position, transform.rotation * (orientation == OrientationMode.YAxisForward ? Quaternion.Euler(-90, 0, 0) : Quaternion.identity), Vector3.one);
				Draw.Gizmos.CircleXZ(Vector3.zero, pickNextWaypointDist, GizmoColor * new Color(1, 1, 1, alpha));
				Draw.Gizmos.CircleXZ(Vector3.zero, slowdownDistance, Color.Lerp(GizmoColor, Color.red, 0.5f) * new Color(1, 1, 1, alpha));
				Draw.Gizmos.CircleXZ(Vector3.zero, endReachedDistance, Color.Lerp(GizmoColor, Color.red, 0.8f) * new Color(1, 1, 1, alpha));
			}
		}
	#endif

		protected override int OnUpgradeSerializedData (int version, bool unityThread) {
			base.OnUpgradeSerializedData(version, unityThread);
			// Approximately convert from a damping value to a degrees per second value.
			if (version < 1) rotationSpeed *= 90;
			return 2;
		}
	}
}
