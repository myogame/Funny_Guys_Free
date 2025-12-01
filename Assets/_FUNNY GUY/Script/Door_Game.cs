using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Game : MonoBehaviour
{
    public GameObject[] Door;
    public GameObject[] Part;

    // Start is called before the first frame update
    void Start()
    {
        int rd = Random.Range(0, 5);
        int rd2 = Random.Range(0, 5);
        int rd3 = Random.Range(0, 5);
        RandomGate(rd);
        RandomGate(rd2);
        RandomGate(rd3);


    }

    void RandomGate(int index)
    {
        switch (index)
        {
            case 0:
                Part[0].GetComponent<Rigidbody>().isKinematic = true;
                Part[1].GetComponent<Rigidbody>().isKinematic = true;
                Door[0].GetComponent<BoxCollider>().enabled = true;


                break;
            case 1:
                Part[2].GetComponent<Rigidbody>().isKinematic = true;
                Part[3].GetComponent<Rigidbody>().isKinematic = true;
                Door[1].GetComponent<BoxCollider>().enabled = true;
                break;
            case 2:
                Part[4].GetComponent<Rigidbody>().isKinematic = true;
                Part[5].GetComponent<Rigidbody>().isKinematic = true;
                Door[2].GetComponent<BoxCollider>().enabled = true;
                break;
            case 3:
                Part[6].GetComponent<Rigidbody>().isKinematic = true;
                Part[7].GetComponent<Rigidbody>().isKinematic = true;
                Door[3].GetComponent<BoxCollider>().enabled = true;
                break;
            case 4:
                Part[8].GetComponent<Rigidbody>().isKinematic = true;
                Part[9].GetComponent<Rigidbody>().isKinematic = true;
                Door[4].GetComponent<BoxCollider>().enabled = true;

                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
