using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class Slot_MutilPlayer : MonoBehaviour
{
    public TextMeshProUGUI nameSlot;
    PhotonView photonView;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        nameSlot = GetComponent<TextMeshProUGUI>();

        PhotonNetwork.NickName = PlayerPrefs.GetString("PlayerName");
      



    }

    
}
