using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Realtime;
using UnityEngine.UI;


public class ConnectToServer : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInput;
    public TMP_InputField joinInput;
    public Button createBtn;
    public Button joinBtn;
    public Button randomJoinBtn;
    int sellectMapMultiplay;

    public GameObject searchPanel;
    public Button stopSearch;

    public GameObject characterPanel;
    public GameObject button;

    public Button startGame_multiBtn;

    public int playerCount;


   

    private void Awake()
    {
       
        createBtn.onClick.AddListener(() => CreatRoom());
        joinBtn.onClick.AddListener(() => JoinRoom());
        randomJoinBtn.onClick.AddListener(() => JoinRandom());
        stopSearch.onClick.AddListener(() => StopSearch());
        startGame_multiBtn.onClick.AddListener(() => Start_Game_Call_All());
    }
    // Start is called before the first frame update
    void Start()
    {
        startGame_multiBtn.interactable = false;
        createBtn.interactable = true;
        joinBtn.interactable = true;
        randomJoinBtn.interactable = true;
        searchPanel.SetActive(false);
        PhotonNetwork.ConnectUsingSettings();
        
    }

   public override void OnConnectedToMaster()
   {
        PhotonNetwork.JoinLobby();

    }

    public void CreatRoom()
    {
        
        PhotonNetwork.CreateRoom(createInput.text);
        searchPanel.SetActive(true);
    }

    public void JoinRoom()
    {
       
        PhotonNetwork.JoinRoom(joinInput.text);
        searchPanel.SetActive(true);
        //PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void JoinRandom()
    {
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("Join Random...");
        searchPanel.SetActive(true);
       
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //base.OnJoinRandomFailed(returnCode, message);
        MakeRoom();
        Debug.Log("Could Find Room");
    }

    public void MakeRoom()
    {
        int randomRoomName = Random.Range(0, 200);
        RoomOptions roomOptions =
            new RoomOptions()
            {
                IsVisible = true,
                IsOpen = true,
                MaxPlayers = 2
            };
        PhotonNetwork.CreateRoom("RoomName_" + randomRoomName, roomOptions);
        Debug.Log("Room Creating! Waiting...");
    }

    public override void OnJoinedRoom()
    {
        GameObject newButton = Instantiate(button);
        RectTransform rectTransform = newButton.GetComponent<RectTransform>();
        rectTransform.SetParent(characterPanel.transform, false);
     
        playerCount++;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        GameObject newButton = Instantiate(button);
        RectTransform rectTransform = newButton.GetComponent<RectTransform>();
        rectTransform.SetParent(characterPanel.transform, false);
      
        playerCount +=2;



        if (playerCount > 1)
        {
            startGame_multiBtn.interactable = true;
        }
    }

    void Start_Game_Call_All()
    {
        
        photonView.RPC("HandleIput_Call_All", RpcTarget.All, sellectMapMultiplay);
        photonView.RPC("StartGame_Multi", RpcTarget.All);
    }
   

    [PunRPC]
    public void StartGame_Multi()
    {
        PhotonNetwork.LoadLevel("GamePlay_" + (sellectMapMultiplay + 1).ToString() + "_Multiplay");
    }

    
    public void HandleInputData(int val)
    {
        sellectMapMultiplay = val;
       
    }


    [PunRPC]
    public void HandleIput_Call_All(int val)
    {
        sellectMapMultiplay = val;
       
    }


    public void StopSearch()
    {
        searchPanel.SetActive(false);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(0);

    }
}
