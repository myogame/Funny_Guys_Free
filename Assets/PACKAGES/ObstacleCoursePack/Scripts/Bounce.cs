using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
	public float force = 10f;
	public float stunTime = 0.5f;
	private Vector3 hitDir;
	public int bouceValue;
	
    void OnCollisionEnter(Collision collision)
	{
		foreach (ContactPoint contact in collision.contacts)
		{
			Debug.DrawRay(contact.point, contact.normal, Color.white);
			if (collision.gameObject.tag == "Player")
			{
				
				hitDir = contact.normal;
				collision.gameObject.GetComponent<CharacterControls>().HitPlayer(-hitDir * force, stunTime, bouceValue);
				collision.gameObject.GetComponent<SoundManager>().AudioFX(0);
				return;
			}
			if (collision.gameObject.tag == "Bot")
			{
				
				hitDir = contact.normal;
				collision.gameObject.GetComponent<BOTControls>().HitPlayer(-hitDir * force, stunTime, bouceValue);
				collision.gameObject.GetComponent<SoundManager>().AudioFX(0);
				return;
			}

			if (collision.gameObject.tag == "MultiPlayer")
			{

				hitDir = contact.normal;
				collision.gameObject.GetComponent<CharacterCtr_Multi>().HitPlayer(-hitDir * force, stunTime, bouceValue);
				collision.gameObject.GetComponent<SoundManager>().AudioFX(0);
				return;
			}
		}
		
	}
}
