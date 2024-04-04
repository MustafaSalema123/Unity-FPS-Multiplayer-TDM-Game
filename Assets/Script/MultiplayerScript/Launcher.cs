using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using System.Linq;
using Unity.VisualScripting;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher instance;

    [SerializeField] InputField roomNameInputField;
    [SerializeField] Text roomNameText;
    [SerializeField] Text errorText;

    [SerializeField] Transform roomListCount;
    [SerializeField] Transform roomListItemPrefab;

    [SerializeField] Transform playerListCount;
    [SerializeField] Transform playerListItemPrefab;


    public GameObject startButton;

    private int nextTeamMember = 1;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {


    
        //OnJoinedLobby();
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
      //  MenuManager.instance.OpenMenu("TitleMenu");
        MenuManager.instance.OpenMenu("UserNameMenu");
        //Debug.Log("Joined lobby");
        //PhotonNetwork.NickName = "Player " + Random.Range(0,10000).ToString("0000");
    }
    public void CreateRoom() 
    {
        if (string.IsNullOrEmpty(roomNameInputField.text)) 
        {
            return;
        }
        PhotonNetwork.CreateRoom(roomNameInputField.text);
        MenuManager.instance.OpenMenu("LoadMenu");
    }
    public override void OnJoinedRoom()
    {
        MenuManager.instance.OpenMenu("RoomMenu");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        foreach (Transform trans in playerListCount)
        {
            Destroy(trans.gameObject);
        }
        //Joind room item 
        Player[] players = PhotonNetwork.PlayerList;
        for(int i = 0; i < players.Count(); i++)
        {
            int teamMember = GetNextTeamNumber();
            Instantiate(playerListItemPrefab, playerListCount).GetComponent<PlayerListItem>().SetUp(players[i], teamMember);
        }

        startButton.SetActive(PhotonNetwork.IsMasterClient);

    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startButton.SetActive(PhotonNetwork.IsMasterClient);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {

        errorText.text = "Room Generation UnSuccessFul " + message;
        MenuManager.instance.OpenMenu("ErrorMenu");
    }
    public void JoinRoom(RoomInfo room) 
    {
        PhotonNetwork.JoinRoom(room.Name);
        MenuManager.instance.OpenMenu("LoadMenu");
    }
    public void LeaveRoom() 
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.instance.OpenMenu("LoadMenu");
    }

    public override void OnLeftRoom()
    {
        MenuManager.instance.OpenMenu("TitleMenu");
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        //room list update
        foreach (Transform trans in roomListCount)
        {
            Destroy(trans.gameObject);
        }

        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
                continue;
         
            Instantiate(roomListItemPrefab, roomListCount).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //Player enter in room Data
        int teamMember = GetNextTeamNumber();

        GameObject playerItem = Instantiate(playerListItemPrefab, playerListCount).gameObject;
        playerItem.GetComponent<PlayerListItem>().SetUp(newPlayer , teamMember);
    }


    private int GetNextTeamNumber() 
    {
        int teamMember = nextTeamMember;
        nextTeamMember = 3 - nextTeamMember;
        return teamMember;
    }

    public void StartGame() 
    {
    PhotonNetwork.LoadLevel(1);
    }
}
