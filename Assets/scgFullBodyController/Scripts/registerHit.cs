
using Photon.Pun;
using UnityEngine;


    public class registerHit : MonoBehaviour
    {
  
        //IMPORTANT, this script must be on root of bullet object

        public GameObject impactParticle;
        public GameObject impactBloodParticle;
        public float impactDespawnTime;
        [HideInInspector] public int damage;
    [HideInInspector] public int playerTeam;
    public ParticleSystem bloodEffect;

    //add a vie after workd
    void OnCollisionEnter(Collision col)
        {
            //If we (the bullet) hit the col object check for Player tag
            if (col.transform.tag == "Player")
            {

            HealthController otherhealthController = col.transform.root.gameObject.GetComponent<HealthController>();
         
                //If the root object we hit has a healthcontroller then apply damage
                if (otherhealthController ) 
                {
                 
                if (otherhealthController.playerTeam != playerTeam)
                {
               
                    otherhealthController.ApplyDamage(damage);
                    //Spawn blood on player
                    GameObject tempImpact;
                    tempImpact = Instantiate(impactBloodParticle, this.transform.position, this.transform.rotation);
                    tempImpact.transform.Rotate(Vector3.left * 90);
                    Destroy(tempImpact, impactDespawnTime);
                }
                }

        
            }
            else
            {

                GameObject tempImpact;
                tempImpact = Instantiate(impactParticle, this.transform.position, this.transform.rotation) ;
                tempImpact.transform.Rotate(Vector3.left * 90);
                Destroy(tempImpact, impactDespawnTime);
            }

            //Finally, destroy us (the bullet)
            Destroy(gameObject);
        }
    [PunRPC]
    void PRC_ShootEffect(Vector3 hitpoint, Vector3 hitnorml)
    {
        ParticleSystem blood = Instantiate(bloodEffect, hitpoint, Quaternion.LookRotation(hitnorml));
        Destroy(blood.gameObject, blood.main.duration);
    }

}



    

