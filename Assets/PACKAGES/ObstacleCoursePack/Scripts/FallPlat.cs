using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallPlat : MonoBehaviour
{
	public float fallTime = 0.5f;
	public float rebuild = 3f;

    private void Start()
    {
		int rd = Random.Range(0, 10);
		if (rd == 1) fallTime = 999;
		else fallTime = 0.5f;
	}

    void OnCollisionEnter(Collision collision)
	{
		foreach (ContactPoint contact in collision.contacts)
		{
			//Debug.DrawRay(contact.point, contact.normal, Color.white);
			if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Bot" || collision.gameObject.tag == "MultiPlayer")
			{
				
				StartCoroutine(Fall(fallTime));


			}
		}
	}

	IEnumerator Fall(float time)
	{
		yield return new WaitForSeconds(time);
		gameObject.GetComponent<MeshRenderer>().enabled = false;
		gameObject.GetComponent<BoxCollider>().enabled = false;
		yield return new WaitForSeconds(rebuild);
		gameObject.GetComponent<MeshRenderer>().enabled = true;
		gameObject.GetComponent<BoxCollider>().enabled = true;
		int rd = Random.Range(0, 5);
		if (rd == 1) fallTime = 999;
		else fallTime = 0.5f;
		yield return null;
	}
}
