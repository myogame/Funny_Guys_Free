using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WNDRotatorGP2 : MonoBehaviour
{
    public Vector3 rotationSpeed;
    public Vector3 giatoc;
    bool startGame;

    private void Start()
    {
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        startGame = false;
        yield return new WaitForSeconds(4.0f);
        startGame = true;
    }

    void FixedUpdate()
    {
        rotationSpeed += giatoc; 

        if (startGame)
        transform.Rotate(rotationSpeed);
    }
}
