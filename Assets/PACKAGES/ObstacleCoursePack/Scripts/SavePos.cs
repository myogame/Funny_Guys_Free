using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SavePos : MonoBehaviour
{
	public Transform checkPoint;

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Player")
			col.gameObject.GetComponent<CharacterControls>().checkPoint = checkPoint.position;
		
		if (col.gameObject.tag == "Bot")
			col.gameObject.GetComponent<BOTControls>().checkPoint = checkPoint.position;
			
		
		if (col.gameObject.tag == "MultiPlayer")
			col.gameObject.GetComponent<CharacterCtr_Multi>().checkPoint = checkPoint.position;
		
	}
}
