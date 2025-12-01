using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnItem : MonoBehaviour
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




        for (int i = 1; i < teleport.Length; i++)
        {
            int randomItem = Random.Range(0, itemPrefeb.Length);
            Instantiate(itemPrefeb[randomItem], teleport[i].position, itemPrefeb[randomItem].GetComponent<Transform>().rotation);  //teleport[i].rotation

        }



      
    }

    private void Update()
    {
        

    }
}
