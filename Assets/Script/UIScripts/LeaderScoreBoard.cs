using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderScoreBoard : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform container1;
    [SerializeField] Transform container2;
    [SerializeField] GameObject scoreBoardItemteam1Prefab;
    [SerializeField] GameObject scoreBoardItemteam2Prefab;

    Dictionary<Player , ScoreBoardItem> scoreBoardItems= new Dictionary<Player , ScoreBoardItem>();


    public int playerTeam;

    PlayerController manager;
    private void Start()
    {

        //if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Team"))
        //{

        //    playerTeam = (int)PhotonNetwork.LocalPlayer.CustomProperties["Team"];
        //     Debug.Log("player for leader team Vice " + PhotonNetwork.LocalPlayer.NickName + " pt " + playerTeam);
        //}


        foreach (Player player in PhotonNetwork.PlayerList) 
        {

            if (player.CustomProperties.ContainsKey("Team"))
            {
                int team = (int)player.CustomProperties["Team"];
          
                AddScoreBoardItem(player , team);
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (newPlayer.CustomProperties.ContainsKey("Team"))
        {
            int team = (int)newPlayer.CustomProperties["Team"];

                 AddScoreBoardItem(newPlayer, team);
           }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RemoveScoreBoardItem(otherPlayer);
    }

    void AddScoreBoardItem(Player player , int team) 
    {
        if (team == 1) 
        {
            ScoreBoardItem item1 = Instantiate(scoreBoardItemteam1Prefab, container1).GetComponent<ScoreBoardItem>();
            item1.Initilaze(player);
            scoreBoardItems[player] = item1;
        }
        if(team == 2) 
        {
            ScoreBoardItem item2 = Instantiate(scoreBoardItemteam2Prefab, container2).GetComponent<ScoreBoardItem>();
            item2.Initilaze(player);
            scoreBoardItems[player] = item2;
        }
     
    }

    void RemoveScoreBoardItem(Player player)
    {
        Destroy(scoreBoardItems[player].gameObject);
        scoreBoardItems.Remove(player);
    }
}
