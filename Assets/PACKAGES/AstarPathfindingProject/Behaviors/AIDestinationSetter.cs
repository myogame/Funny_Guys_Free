using UnityEngine;
using System.Collections;

namespace Pathfinding {
	/// <summary>
	/// Sets the destination of an AI to the position of a specified object.
	/// This component should be attached to a GameObject together with a movement script such as AIPath, RichAI or AILerp.
	/// This component will then make the AI move towards the <see cref="target"/> set on this component.
	///
	/// See: <see cref="Pathfinding.IAstarAI.destination"/>
	///
	/// [Open online documentation to see images]
	/// </summary>
	[UniqueComponent(tag = "ai.destination")]
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_a_i_destination_setter.php")]
	public class AIDestinationSetter : VersionedMonoBehaviour {
		/// <summary>The object that the AI should move to</summary>
		public Transform target;
		IAstarAI ai;
		float time;


		void OnEnable () {
			ai = GetComponent<IAstarAI>();

			int rd = Random.Range(1, 10);
			Target(rd);
			
			// Update the destination right before searching for a path as well.
			// This is enough in theory, but this script will also update the destination every
			// frame as the destination is used for debugging and may be used for other things by other
			// scripts as well. So it makes sense that it is up to date every frame.
			if (ai != null) ai.onSearchPath += Update;
		}

		public void Target(int rd)
        {
			target = GameObject.Find("Target/Target_" + rd.ToString()).transform;
		}

        private void OnCollisionStay(Collision collision)
        {
			if (collision.gameObject.tag == "Target")
			{
				int rd = Random.Range(1, 10);
				Target(rd);
			}
		}

        

		void OnDisable () {
			if (ai != null) ai.onSearchPath -= Update;
		}

		/// <summary>Updates the AI's destination every frame</summary>
		void Update () {
			if (target != null && ai != null) ai.destination = target.position;

			if(time> 0)
            {
				time -= Time.deltaTime;
            }
            else
            {
				time = Random.Range(5,10);
				int rd = Random.Range(1, 10);
				Target(rd);
			}
		}
	}
}
