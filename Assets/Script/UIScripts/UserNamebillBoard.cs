using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserNamebillBoard : MonoBehaviour
{
    Camera mainCam;

    // Update is called once per frame
    void Update()
    {
        if(mainCam == null) { mainCam = FindObjectOfType<Camera>(); }
        if (mainCam == null)
            return;

        transform.LookAt(mainCam.transform);
        transform.Rotate(Vector3.up * 100);
        
    }
}
