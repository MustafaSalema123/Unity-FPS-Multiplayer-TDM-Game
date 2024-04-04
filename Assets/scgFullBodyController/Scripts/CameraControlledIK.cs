//SlapChickenGames
//2021
//Camera spine controller


using UnityEngine;



    public class CameraControlledIK : MonoBehaviour
    {
        public Transform spineToOrientate;

        // Update is called once per frame
        void LateUpdate()
        {

       
           // spineToOrientate.rotation = transform.rotation;
        }
    public void SplineRotation() 
    {
        spineToOrientate.rotation = transform.rotation;
    }
}
