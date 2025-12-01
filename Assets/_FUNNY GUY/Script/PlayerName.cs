using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerName : MonoBehaviour
{
    public Transform cameraName;
    // Start is called before the first frame update
    void Start()
    {
        cameraName = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + cameraName.rotation * Vector3.forward, cameraName.rotation * Vector3.up);
    }
}
