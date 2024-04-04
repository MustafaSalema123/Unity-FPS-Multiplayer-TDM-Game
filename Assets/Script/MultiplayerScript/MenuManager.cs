using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
 
    public static MenuManager instance;
    public Menu[] menus;

    private void Awake()
    {
        instance = this;
    }
    public void OpenMenu(string menuName)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].menuName == menuName) 
            {
                //OpenMenu(menus[i]);
                menus[i].Open();

            }else if (menus[i].isOpen) 
            {
                ClosedMenu(menus[i]);
            }
        }

    }
    public void OpenMenu(Menu menu) 
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].isOpen)
            {
                ClosedMenu(menus[i]);
            }
        }
        menu.Open();
    }
    public void ClosedMenu(Menu menu) 
    {
        menu.Closed();
    }
}
