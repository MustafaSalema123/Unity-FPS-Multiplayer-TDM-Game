using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class PlayerUserNameManager : MonoBehaviour
{
    [SerializeField] InputField userNameInput;
    [SerializeField] Text errorMessageText;


    private void Start()
    {
        if (PlayerPrefs.HasKey("username")) 
        {
        userNameInput.text = PlayerPrefs.GetString("username"); 
            PhotonNetwork.NickName= PlayerPrefs.GetString("username");
        }
    }
    public void PlayerUsernameInputValueChanged() 
    {
    
        string userName = userNameInput.text;
      if(!string.IsNullOrEmpty(userName) && userName.Length <= 15) 
        {
        PhotonNetwork.NickName= userName;
            PlayerPrefs.SetString("username", userName);
            errorMessageText.text = "";
            MenuManager.instance.OpenMenu("TitleMenu");

        }
        else {
            errorMessageText.text = "User must not be empty and shoud be 15 character or less";
        }

    }
}
