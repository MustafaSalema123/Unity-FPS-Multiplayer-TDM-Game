using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

using System.IO;

using Photon.Realtime;
using System.Linq;
using Hastable = ExitGames.Client.Photon.Hashtable;
public class PlayerControlManager : MonoBehaviourPunCallbacks
{
    public PhotonView view;

    GameObject controller;

    public int playerTeam;

    private Dictionary<int ,int> playerTeams = new Dictionary<int ,int>();

    int Kills;
    int Deaths;
    private void Awake()
    {
        view = GetComponent<PhotonView>();
    }
    private void Start()
    {
        if(view.IsMine) 
        {
            CreateController();
        }
    }

    void CreateController() 
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Team"))
        {

            playerTeam = (int)PhotonNetwork.LocalPlayer.CustomProperties["Team"];
           // Debug.Log("player teams " + playerTeam);
        }

        AssignPLayerToSpawnArea(playerTeam);
    }
    void AssignPLayerToSpawnArea(int team) 
    {
        //get the photon view in view Id // yes we are respawing a somw player

        GameObject spawnArea1 = GameObject.Find("SpawnArea1");
        GameObject spawnArea2 = GameObject.Find("SpawnArea2");
        if(spawnArea1 == null ||spawnArea2 == null) 
        {
            Debug.Log("Spawn Area is not found");
            return;
        }

        Transform spawnPoint = null;
        if(team == 1) 
        {
            spawnPoint = spawnArea1.transform.GetChild(Random.Range(0 , spawnArea1.transform.childCount));
        }
        if (team == 2)
        {
            spawnPoint = spawnArea2.transform.GetChild(Random.Range(0 , spawnArea2.transform.childCount));

        }
        if(spawnPoint != null) 
        {

        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "FullBodyControllerOnline"), spawnPoint.position, spawnPoint.rotation, 0, new object[] { view.ViewID });
           //controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "FullBodyControllerOnline Player"), spawnPoint.position, spawnPoint.rotation, 0, new object[] { view.ViewID });
           //controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "FullBodyControllerOnline"), Vector3.zero, transform.rotation, 0, new object[] { view.ViewID });
        }
    }
    void AssignTeamsToPlayer() 
    {
        foreach(Player player in PhotonNetwork.PlayerList) 
        {

            if (player.CustomProperties.ContainsKey("Team"))
            {
                int team = (int)player.CustomProperties["Team"];
                playerTeams[player.ActorNumber] = team;
                //Debug.Log(player.NickName + " s team: " + team);

                AssignPLayerToSpawnArea(team);
            }
        }

    }



  public  void Die() 
    {

      
    PhotonNetwork.Destroy(controller);
        CreateController();

        Deaths++;
        Hastable hash = new Hastable();
        hash.Add("Death", Deaths);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AssignTeamsToPlayer();
    }

    public void GetKill() 
    {
        view.RPC(nameof(All_GetKill) , view.Owner);
    }

    [PunRPC]
    public void All_GetKill()
    {

        Kills++;

        Hastable hash  = new Hastable();
        hash.Add("Kills", Kills);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }
    public static PlayerControlManager Find(Player player) 
    {
    return FindObjectsOfType<PlayerControlManager>().SingleOrDefault(x => x.photonView.Owner == player);
    }
}
