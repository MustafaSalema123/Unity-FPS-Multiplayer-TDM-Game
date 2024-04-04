//SlapChickenGames
//2021
//Health controller

using Photon.Pun;
using System.Collections;


using UnityEngine;
using UnityEngine.UI;


    public class HealthController : MonoBehaviour
    {
        //IMPORTANT, this script needs to be on the root transform

        [Header("Basics")]
        public float health;
        float maxHealth;
        public Text healthText;

        public GameObject ragdoll;
        public bool dontSpawnRagdoll;
        public float deadTime;
        GameObject tempdoll;
       
        public bool isAiOrDummy;

        [Header("Sound")]
        public bool playNoiseOnHurt;
        public float percentageToPlay;
        public AudioClip hurtNoise;

        [Header("Regen")]
       public bool regen;
        public float timeBeforeRegen;
        float origTimeBeforeRegen;
        public float regenSpeed;
        bool alreadyRegenning;
        public AudioClip regenNoise;

    
    PhotonView View;
        public int playerTeam;



    [Header("Who Kills")]
    [SerializeField] Transform container;
    [SerializeField] GameObject whoKillTextDamagePrefab;
    //Respwan
    PlayerControlManager playerControlManager;
        private void Awake()
        {
            View = GetComponent<PhotonView>();

            playerControlManager = PhotonView.Find((int)View.InstantiationData[0]).GetComponent<PlayerControlManager>();
        }
        void Start()
        {
            //Get a reference to the original reset time
            origTimeBeforeRegen = timeBeforeRegen;


        container = GameObject.Find("WhoDamage Panel").transform;
            //Set maxHealth to what our max is at start of the scene
            maxHealth = health;

            if (View.Owner.CustomProperties.ContainsKey("Team"))
            {

                int Team = (int)View.Owner.CustomProperties["Team"];
                playerTeam = Team;

            }
        }

        void Update()
        {
          
 

            if (!View.IsMine) return;

            
           
        //if(health < maxHealth && !alreadyRegenning) 
        //{
        //    if (regen)
        //    {
          

        //        timeBeforeRegen = origTimeBeforeRegen;
        //        StopCoroutine("regenHealth");
        //        CancelInvoke();
        //        if (timeBeforeRegen == origTimeBeforeRegen)
        //        {
          
        //            alreadyRegenning = true;
        //            Invoke(nameof(regenEnumeratorStart), timeBeforeRegen);
                
        //        }
        //    }
        //}
        //else
        if (health == maxHealth && regen && alreadyRegenning)
        {
            alreadyRegenning = false;
            StopCoroutine("regenHealth");
        }



    }

    public void ApplyDamage(float damage )
        {
            //If we are a player, take damage, otherwise (AI), apply the hit animation and attack the player

            View.RPC(nameof(RPC_takeDamae), View.Owner, damage);


        //This for debug
        //if (regen)
        //{

        //    //Call in over player
        //    timeBeforeRegen = origTimeBeforeRegen;
        //    StopCoroutine("regenHealth");
        //    CancelInvoke();
        //    if (timeBeforeRegen == origTimeBeforeRegen)
        //    {
        //    Debug.Log("Regen is working ");
        //        alreadyRegenning = true;
        //        Invoke(nameof(regenEnumeratorStart), timeBeforeRegen);

        //    }
        //}


        if (regen)
        {
            // Reset regen time
         //   Debug.Log("hEATH IS increaing ");
            timeBeforeRegen = origTimeBeforeRegen;
            StopCoroutine("RegenHealth");
            if (!alreadyRegenning)
            {
                alreadyRegenning = true;
                Invoke(nameof(regenEnumeratorStart), timeBeforeRegen);
            }
        }


    }
    [PunRPC]
        void RPC_takeDamae(float Damage , PhotonMessageInfo info)
        {

            if (!View.IsMine)
                return;

            health -= Damage;
           
            healthText.text = health.ToString();
            if (playNoiseOnHurt)
            {
                //if (Random.value < percentageToPlay)
                //{
                    this.GetComponent<AudioSource>().PlayOneShot(hurtNoise);
                //}
            }
            // healthSlider.value = currentHealth;
            if (health <= 0)
            {

            PlayerControlManager.Find(info.Sender).GetKill();



            Text t = Instantiate(whoKillTextDamagePrefab, container).GetComponent<Text>();
            t.text = info.Sender.NickName + " Kill " + info.photonView.Owner.NickName;


            ScoreBoard.Instance.PlayerDiedTeamVise(playerTeam);
             
                playerControlManager.Die();
              // StartCoroutine(RespanwAfterSameTime());
               // tempdoll = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "RagDollOn"), this.transform.position, this.transform.rotation , 0, new object[] { View.ViewID }) ;
          //  View.RPC(nameof(WhoKillUi), RpcTarget.All, info);
                
                // Destroy(gameObject);
                //playerControlManager.Die();

            } 

    }


    [PunRPC]
    public void WhoKillUi(PhotonMessageInfo info) 
    {
         
        Text t = Instantiate(whoKillTextDamagePrefab, container).GetComponent<Text>();
        t.text = info.Sender.NickName + " Kill " + info.photonView.Owner.NickName;
    }



        void regenEnumeratorStart()
        {
        this.GetComponent<AudioSource>().PlayOneShot(regenNoise);
        StartCoroutine("regenHealth");
        }

        IEnumerator regenHealth()
        {

        //Only regen while under max health and gain 1 health every regenSpeed seconds
        while (health < maxHealth)
            {
            health++;
            healthText.text = health.ToString();
            yield return new WaitForSeconds(regenSpeed);
            }
        }

        void Die()
        {
            //Only spawn ragdoll if option is selected
            if (!dontSpawnRagdoll)
            {
                //Spawn ragdoll and destroy us
                tempdoll = Instantiate(ragdoll, this.transform.position, this.transform.rotation) as GameObject;

                //Tell the ragdoll if we are a player or not so it knows to move our camera or not to the ragdoll
             //   tempdoll.GetComponent<ragdollCamera>().isAi = isAiOrDummy;
                Destroy(gameObject);

                //Destroy ragdoll if we are an AI after deadTime seconds
                if (isAiOrDummy)
                    Destroy(tempdoll, deadTime);
            }
         
        }


        IEnumerator RespanwAfterSameTime() 
        {

            yield return new WaitForSeconds(3f);

            playerControlManager.Die();
            //Incread Score

          
        }
        public void DamageByKick(Vector3 pos, float kickForce, int kickDamage)
        {
            //Subtract the damage from values passed in by kickSensing
            health -= kickDamage;

            //If kicked enough, then die
            if (health <= 0)
            {
                
                tempdoll = Instantiate(ragdoll, this.transform.position, this.transform.rotation) as GameObject;
            
                Destroy(gameObject);

                foreach (Rigidbody rb in tempdoll.GetComponentsInChildren<Rigidbody>())
                {
                    rb.AddForce(pos * kickForce);
                }
            }
            else
            {
                //Dont die just play hit anim
                gameObject.GetComponent<Animator>().SetTrigger("hit");
            }
        }
    }
