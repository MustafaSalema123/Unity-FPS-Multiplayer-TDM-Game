using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class UserNameDisplay : MonoBehaviour
{
   // PhotonView playerPv;

    public Text userNametext;
    public Text teamtext;
    public PhotonView view;
    public Image imageColor;
    private void Start()
    {
        if (view.IsMine) 
        {
            //dont want to display us User name
        gameObject.SetActive(false);
        
        }

        userNametext.text = view.Owner.NickName;


        //Show team member
        if (view.Owner.CustomProperties.ContainsKey("Team")) 
        {
            int team = (int)view.Owner.CustomProperties["Team"];

            if(team == 1) 
            {
                imageColor.color = Color.red;
            }else
            {
                imageColor.color = Color.cyan;
            
            }

            teamtext.text = "Team: " + team;
        }
    }
}
