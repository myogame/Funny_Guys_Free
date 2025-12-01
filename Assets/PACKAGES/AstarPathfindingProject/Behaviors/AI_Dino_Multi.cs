using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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
	public class AI_Dino_Multi : VersionedMonoBehaviour {
		/// <summary>The object that the AI should move to</summary>
		public GameObject target;
		IAstarAI ai;
		public AIPath aIPath;

        private void Start()
        {
			
			target = GameObject.Find("Player_MultiPlay");
		}

        void OnEnable () {
			
			ai = GetComponent<IAstarAI>();
			StartCoroutine(Countdown());
			if (ai != null) ai.onSearchPath += Update;
		}

        private void OnCollisionEnter(Collision collision)
        {
			target = GameObject.Find("Player_MultiPlay");
			StartCoroutine(Countdown());

		}

		IEnumerator Countdown()
		{
			aIPath.canMove = false;
			yield return new WaitForSeconds(3.0f);
			aIPath.canMove = true;
		}

		void OnDisable () {
			if (ai != null) ai.onSearchPath -= Update;
		}

		/// <summary>Updates the AI's destination every frame</summary>
		void Update () {
			if (target == null) target = GameObject.Find("Player_MultiPlay");

			if (target != null && ai != null) ai.destination = target.gameObject.GetComponent<Transform>().position;

		}
	}
}
