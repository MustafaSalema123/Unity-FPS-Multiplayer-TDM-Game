using Photon.Pun;
using Photon.Realtime;

using UnityEngine;
using UnityEngine.UI;


public class PlayerListItem : MonoBehaviourPunCallbacks
{
    [SerializeField] Text playerUsername;
    [SerializeField] Text teamtext;
    Player player;
    int team;
    public void SetUp(Player _Player ,int _team)
    {
        player = _Player;
        team= _team ;
        playerUsername.text = _Player.NickName;
        teamtext.text = "Team " + _team;


        ExitGames.Client.Photon.Hashtable customProps = new ExitGames.Client.Photon.Hashtable();
        customProps["Team"] = _team;
        _Player.SetCustomProperties(customProps);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (player == otherPlayer)
        {
            Destroy(gameObject);
        }
    }

    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }

}
