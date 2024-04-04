//SlapChickenGames
//2021
//Manager for weapon inventory and switching

using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;


    public class GunManager : MonoBehaviourPunCallbacks
    {
        public GameObject[] weapons;
        public Animator anim;
        public OffsetRotation oRot;
        public float swapTime;
        int index = 0;

        PhotonView View;
        InputManager inputManager;

  //  [HideInInspector] public int damage;


    public int playerTeam;

    public ParticleSystem bloodEffect;

    private void Awake()
        {
            inputManager= GetComponent<InputManager>();
            View = GetComponent<PhotonView>();
        }
        void Start()
        {


            if (!View.IsMine) return;
      
            //Initialize each weapon and set state to swapping automatically so gun controller knows to setup weapon positions
            foreach (GameObject weapon in weapons)
            {
                weapon.GetComponent<GunController>().swapping = true;
            }
        if (View.Owner.CustomProperties.ContainsKey("Team"))
        {

            int Team = (int)View.Owner.CustomProperties["Team"];
            playerTeam = Team;

        }

        /*
        The invoke timing here is based off the time it takes for the swap animation to complete + transition time,
        this is so that the weapon aiming position is based off where its first position is out of the swap anim
        */
        Invoke("setSwappedWeaponPositions", .567f + .25f);
        }

        void Update()
        {
            if (!View.IsMine) return;


        if (GameManager.instance.isPlayGame == false) return;
        //To add more weapons, just copy one of these blocks of code, add an else if, and change the keybind to the next one up ex., 
        //Aplha4, then set index to the corresponding key value such as 4
        if (Input.GetKeyDown(KeyCode.Alpha1) && index != 0)
            {
                if (!weapons[index].GetComponent<GunController>().firing && !weapons[index].GetComponent<GunController>().swapping
                    && !weapons[index].GetComponent<GunController>().aiming && weapons[index].GetComponent<GunController>().aimFinished
                    && !weapons[index].GetComponent<GunController>().reloading && !weapons[index].GetComponent<GunController>().cycling)
                {
                    index = 0;
                    Invoke("swapWeapons", swapTime);
                    foreach (GameObject weapon in weapons)
                    {
                        weapon.GetComponent<GunController>().swapping = true;
                    }
                    anim.SetBool("putaway", true);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) && index != 1 && weapons.Length > 1)
            {
                if (!weapons[index].GetComponent<GunController>().firing && !weapons[index].GetComponent<GunController>().swapping
                    && !weapons[index].GetComponent<GunController>().aiming && weapons[index].GetComponent<GunController>().aimFinished
                    && !weapons[index].GetComponent<GunController>().reloading && !weapons[index].GetComponent<GunController>().cycling)
                {
                    {
                        index = 1;
                        Invoke("swapWeapons", swapTime);
                        foreach (GameObject weapon in weapons)
                        {
                            weapon.GetComponent<GunController>().swapping = true;
                        }
                        anim.SetBool("putaway", true);
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) && index != 2 && weapons.Length > 2)
            {
                if (!weapons[index].GetComponent<GunController>().firing && !weapons[index].GetComponent<GunController>().swapping
                    && !weapons[index].GetComponent<GunController>().aiming && weapons[index].GetComponent<GunController>().aimFinished
                    && !weapons[index].GetComponent<GunController>().reloading && !weapons[index].GetComponent<GunController>().cycling)
                {
                    {
                        index = 2;
                        Invoke("swapWeapons", swapTime);
                        foreach (GameObject weapon in weapons)
                        {
                            weapon.GetComponent<GunController>().swapping = true;
                        }
                        anim.SetBool("putaway", true);
                    }
                }
            }

       // ShootDamage();
          weapons[index].GetComponent<GunController>().ShootGunUpdate(inputManager.fireInput , inputManager.reloadInput , playerTeam);
        }
        [PunRPC]
        void PRC_ShootEffect(Vector3 hitpoint, Vector3 hitnorml)
        {
            ParticleSystem blood = Instantiate(bloodEffect, hitpoint, Quaternion.LookRotation(hitnorml));
            Destroy(blood.gameObject, blood.main.duration);
        }

    void ShootDamage() 
    {
        if (inputManager.fireInput)
        {

            RaycastHit hit;

            if (Physics.Raycast(weapons[index].GetComponent<GunController>().shootPoint.transform.position
                , weapons[index].GetComponent<GunController>().shootPoint.transform.forward, out hit, 300f))
            {


                //Extract hit information
                Vector3 hitpoint = hit.point;
                Vector3 hitnormal = hit.normal;

                HealthController playerMovementDaamge = hit.collider.GetComponent<HealthController>();
                
                if (playerMovementDaamge != null && playerMovementDaamge.playerTeam != this.playerTeam)
                {
                    //damage = weapons[index].GetComponent<GunController>().Damage;

                    //Damage Apply 
                    playerMovementDaamge.ApplyDamage(weapons[index].GetComponent<GunController>().Damage);
                    View.RPC("PRC_ShootEffect", RpcTarget.All, hitpoint, hitnormal);
                }
            }
        }
    }
    private void LateUpdate()
        {
            
            weapons[index].GetComponent<GunController>().ScopeWeaponLateUpdate(inputManager.scopeInput);
        }
        void swapWeapons()
        {
            //Set every other weapon except the one we want to swap to at index to false
            for (int i = 0; i < weapons.Length; i++)
            {
                if (i != index)
                {
                    weapons[i].SetActive(false);
                }
            }

            //Set desired weapon to active
            weapons[index].SetActive(true);
            Invoke("setSwappedWeaponPositions", .567f + .25f);

            //Initliaze the correct spine rotation on the spine bone's orientation script
            if (weapons[index].GetComponent<GunController>().Weapon == GunController.WeaponTypes.Rifle)
            {
                oRot.rifle = true;
                oRot.pistol = false;
            }
            else if (weapons[index].GetComponent<GunController>().Weapon == GunController.WeaponTypes.Pistol)
            {
                oRot.rifle = false;
                oRot.pistol = true;
            }
            anim.SetBool("putaway", false);

        if (View.IsMine)
        {
            Hashtable hashWeapon = new Hashtable();
            hashWeapon.Add("ItemIndex", index);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hashWeapon);
        }
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if(!View.IsMine && targetPlayer == View.Owner) 
        {
          //  weapons[(int)changedProps["ItemIndex"]].SetActive(true);
        }
    }

    void setSwappedWeaponPositions()
        {
            //Initialize the correct original aim position if it is the first time swapping
            if (!weapons[index].GetComponent<GunController>().aimPosSet)
            {
                weapons[index].GetComponent<GunController>().initiliazeOrigPositions();
                weapons[index].GetComponent<GunController>().aimPosSet = true;
            }

            foreach (GameObject weapon in weapons)
            {
                weapon.GetComponent<GunController>().swapping = false;
            }
        }
    }

