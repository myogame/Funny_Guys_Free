using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding.RVO;

namespace Pathfinding.Legacy {
	[RequireComponent(typeof(Seeker))]
	[AddComponentMenu("Pathfinding/Legacy/AI/Legacy RichAI (3D, for navmesh)")]
	/// <summary>
	/// Advanced AI for navmesh based graphs.
	///
	/// Deprecated: Use the RichAI class instead. This class only exists for compatibility reasons.
	/// </summary>
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_legacy_1_1_legacy_rich_a_i.php")]
	public class LegacyRichAI : RichAI {
		/// <summary>
		/// Use a 3rd degree equation for calculating slowdown acceleration instead of a 2nd degree.
		/// A 3rd degree equation can also make sure that the velocity when reaching the target is roughly zero and therefore
		/// it will have a more direct stop. In contrast solving a 2nd degree equation which will just make sure the target is reached but
		/// will usually have a larger velocity when reaching the target and therefore look more "bouncy".
		/// </summary>
		public bool preciseSlowdown = true;

		public bool raycastingForGroundPlacement = false;

		/// <summary>
		/// Current velocity of the agent.
		/// Includes eventual velocity due to gravity
		/// </summary>
		new Vector3 velocity;

		Vector3 lastTargetPoint;
		Vector3 currentTargetDirection;

		protected override void Awake () {
			base.Awake();
			if (rvoController != null) {
				if (rvoController is LegacyRVOController) (rvoController as LegacyRVOController).enableRotation = false;
				else Debug.LogError("The LegacyRichAI component only works with the legacy RVOController, not the latest one. Please upgrade this component", this);
			}
		}

		/// <summary>Smooth delta time to avoid getting overly affected by e.g GC</summary>
		static float deltaTime;

		/// <summary>Update is called once per frame</summary>
		

		new Vector3 RaycastPosition (Vector3 position, float lasty) {
			if (raycastingForGroundPlacement) {
				RaycastHit hit;
				float up = Mathf.Max(height*0.5f, lasty-position.y+height*0.5f);

				if (Physics.Raycast(position+Vector3.up*up, Vector3.down, out hit, up, groundMask)) {
					if (hit.distance < up) {
						//grounded
						position = hit.point;//.up * -(hit.distance-centerOffset);
						velocity.y = 0;
					}
				}
			}
			return position;
		}

		/// <summary>Rotates along the Y-axis the transform towards trotdir</summary>
		bool RotateTowards (Vector3 trotdir) {
			trotdir.y = 0;
			if (trotdir != Vector3.zero) {
				Quaternion rot = tr.rotation;

				Vector3 trot = Quaternion.LookRotation(trotdir).eulerAngles;
				Vector3 eul = rot.eulerAngles;
				eul.y = Mathf.MoveTowardsAngle(eul.y, trot.y, rotationSpeed*deltaTime);
				tr.rotation = Quaternion.Euler(eul);
				//Magic number, should expose as variable
				return Mathf.Abs(eul.y-trot.y) < 5f;
			}
			return false;
		}
	}
}
