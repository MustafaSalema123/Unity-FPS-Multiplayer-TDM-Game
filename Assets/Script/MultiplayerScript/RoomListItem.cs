using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] Text roomNametext;
    public RoomInfo info;
    
    public void SetUp(RoomInfo _info) 
    {
    info= _info;
        roomNametext.text = _info.Name;
    }
    public void Click() 
    {
    Launcher.instance.JoinRoom(info);
    }
}
