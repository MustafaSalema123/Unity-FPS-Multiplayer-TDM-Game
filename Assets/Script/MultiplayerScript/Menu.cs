using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{

    public string menuName;
    public bool isOpen;

    private void Start()
    {
      //  MenuManager.instance.OpenMenu("TitleMenu");
    }
    public void Open() 
    {
    gameObject.SetActive(true);
        isOpen= true;
    }
    public void Closed()
    {
        gameObject.SetActive(false);
        isOpen= false;

    }

    
    
}
