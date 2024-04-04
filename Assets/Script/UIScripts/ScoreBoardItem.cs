
using ExitGames.Client.Photon;

using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
public class ScoreBoardItem : MonoBehaviourPunCallbacks
{
    public Text usernameText;
    public Text killsText;
    public Text deathText;

    Player player;
    public void Initilaze(Player _player) 
    {
        player = _player;
        usernameText.text = _player.NickName;

        UpdateState();
    }

    void UpdateState() 
    {
        if(player.CustomProperties.TryGetValue("Kills", out object Kills)) 
        {
            killsText.text = Kills.ToString();
        }
        if (player.CustomProperties.TryGetValue("Death", out object deaths))
        {
            deathText.text = deaths.ToString();
        }

    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if(targetPlayer == player  ) 
        {

            if (changedProps.ContainsKey("Kills") || changedProps.ContainsKey("Death")) 
            {
            UpdateState();
            }
        }
    }
}
