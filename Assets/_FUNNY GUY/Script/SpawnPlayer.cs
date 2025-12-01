using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    
    public GameObject[] itemPrefeb;

    int value = 0;
    public Transform[] teleport;

    private void Start()
    {
        teleport = gameObject.GetComponentsInChildren<Transform>();

        foreach (Transform trans in teleport)
        {
            value++;
        }


        int rd = Random.Range(1, teleport.Length - 1);
        Instantiate(itemPrefeb[PlayerPrefs.GetInt("Select")], teleport[rd].position, itemPrefeb[PlayerPrefs.GetInt("Select")].GetComponent<Transform>().rotation);
      
    }

    private void Update()
    {
        

    }
}
