using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class SpawnPlayer_Multi : MonoBehaviour
{
    public GameObject[] SpawnPoint;
  

    void Start()
    {
        if (PhotonNetwork.IsConnected )
        {
            SpawnPlayer();
        }
    }

    void SpawnPlayer()
    {
        Debug.Log("Spawn Player Success!");
        int player = Random.Range(0,10);
        GameObject Player = PhotonNetwork.Instantiate("Player_" + PlayerPrefs.GetInt("Select").ToString() + "_Multiplay", SpawnPoint[player].transform.position, Quaternion.identity);
        

    }
}
