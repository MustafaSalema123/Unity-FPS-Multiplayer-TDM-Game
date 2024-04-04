//SlapChickenGames
//2021
//Sniper scope controller

using UnityEngine;
using UnityEngine.Rendering;



    public class sniperScopeController : MonoBehaviour
    {
        public CameraController camControl;

        public float sniperAimSensitivty;
        float originalCamSensitivity;
     
        public Animator blackLensAnim;
        // Start is called before the first frame update
        void Start()
        {
            originalCamSensitivity = camControl.Sensitivity;

        }

        // Update is called once per frame
        void Update()
        {
            if (gameObject.GetComponent<GunController>().aiming)
            {
                camControl.Sensitivity = sniperAimSensitivty;
               // dofComponent.active = true;
                blackLensAnim.SetBool("aiming", true);
            }
            else
            {
                camControl.Sensitivity = originalCamSensitivity;
              //  dofComponent.active = false;
                blackLensAnim.SetBool("aiming", false);
            }
        }
    }

