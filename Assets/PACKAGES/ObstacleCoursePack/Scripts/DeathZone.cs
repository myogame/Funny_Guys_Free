using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
			col.gameObject.GetComponent<CharacterControls>().LoadCheckPoint();
        }
        if (col.gameObject.tag == "Bot")
        {
            col.gameObject.GetComponent<BOTControls>().LoadCheckPoint();
        }
        if (col.gameObject.tag == "Ball")
        {
            Destroy(col.gameObject);
        }
    }
}
