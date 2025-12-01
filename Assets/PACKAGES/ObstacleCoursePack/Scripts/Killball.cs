using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Killball : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        
        if (col.gameObject.tag == "Ball")
        {
            Destroy(col.gameObject);
        }
    }
}
