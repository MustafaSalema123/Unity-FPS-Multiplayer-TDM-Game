﻿//SlapChickenGames
//2021
//ragdoll Cam Controller

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class ragdollCamera : MonoBehaviour
    {
        public Transform headBone;
        
        void Start()
        {
           
                GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
                //GameObject hud = GameObject.FindGameObjectWithTag("hud");
                //hud.SetActive(false);
                cam.transform.parent = headBone;
                cam.transform.localPosition = new Vector3(0, 0, .3f);
            
        }

    }

