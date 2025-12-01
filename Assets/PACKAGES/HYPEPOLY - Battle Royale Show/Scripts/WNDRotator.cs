using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WNDRotator : MonoBehaviour
{
    public Vector3 rotationSpeed;
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
        if (startGame)
        transform.Rotate(rotationSpeed);
    }
}
