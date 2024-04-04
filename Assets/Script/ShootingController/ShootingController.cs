using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour
{
    InputManager inputManager;
    PlayerMovement playerMovement;
  

    Animator animator;

    [Header("Shooting Value")]
    public Transform firePoint;

    public float fireRate = 0f, fireRange = 100f, fireDamage = 15 ;
    private float nextFireTime = 0f;

    [Header("Shooting Flags")]
    public bool isShooting, isWalking, isShootingInput;
    public bool isReloading = false;

    [Header("Reloading")]
    public int maxAmmo = 30;
    public int currntAmmo;
    public float reloadTime;

    [Header("Sound Effect")]
    public AudioSource soundAudioSource;
    public AudioClip shootingsoundClip;
    public AudioClip reloadingSoundClip;

    [Header("Sound Effect")]
    public ParticleSystem bloodEffect;
    public ParticleSystem muzzleFlash;

    PhotonView View;
    public int playerTeam;

    void Start()
    {
        View = GetComponent<PhotonView>();

        animator = GetComponent<Animator>();
        inputManager = GetComponent<InputManager>();
        playerMovement = GetComponent<PlayerMovement>();

        if (View.Owner.CustomProperties.ContainsKey("Team"))
        {

            int Team = (int)View.Owner.CustomProperties["Team"];
            playerTeam = Team;
          
        }

    }

    private void Update()
    {
        if (!View.IsMine) return;

        if(isReloading || playerMovement.isSprinting) 
        {
            animator.SetBool("Shoot", false);
            animator.SetBool("ShootingMovement", false);
            animator.SetBool("ShootWalk", false);
            return;
        }

        isWalking = playerMovement.isMoving;
        isShootingInput = inputManager.fireInput;
        if(isShootingInput && isWalking) 
        {
        
            if(Time.time > nextFireTime) 
            {
                nextFireTime = Time.time + 1f / fireRate;
                Shoot();
                animator.SetBool("ShootWalk", true);
            }

            animator.SetBool("Shoot", false);
            animator.SetBool("ShootingMovement", true);
            isShooting= true;

        }else if (isShootingInput) 
        {

            if (Time.time > nextFireTime)
            {
                nextFireTime = Time.time + 1f / fireRate;
                Shoot();
           
            }

            animator.SetBool("Shoot", true);
            animator.SetBool("ShootingMovement", false);
            animator.SetBool("ShootWalk", false);
            isShooting = true;
        }
        else
        {
            animator.SetBool("Shoot", false);
            animator.SetBool("ShootingMovement", false);
            animator.SetBool("ShootWalk", false);
            isShooting = false;
        }
        if(inputManager.reloadInput && currntAmmo < maxAmmo) 
        {
            Reload();
        }
    }

    void Shoot() 
    {
        if (currntAmmo > 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, fireRange))
            {
               // Debug.Log(hit.transform.name);

                //Extract hit information
                Vector3 hitpoint = hit.point;
                Vector3 hitnormal = hit.normal;
                //Apply damage
                PlayerMovement playerMovementDaamge = hit.collider.GetComponent<PlayerMovement>();
                if(playerMovementDaamge != null && playerMovementDaamge.playerTeam != this.playerTeam  ) 
                {
                    playerMovementDaamge.ApplyDamage(fireDamage);
                 // ViewplayerMovementDaamgeRPC("PRC_ShootEffect", RpcTarget.All ,hitpoint,hitnormal);
                }
            }
            //play muzzleflash
          //  muzzleFlash.Play();
            //play sound
            //soundAudioSource.PlayOneShot(shootingsoundClip);
            currntAmmo--;
        }
        else 
        {
            Reload();
        }
    }

    [PunRPC]
    void PRC_ShootEffect(Vector3 hitpoint , Vector3 hitnorml) 
    {
        ParticleSystem blood = Instantiate(bloodEffect, hitpoint, Quaternion.LookRotation(hitnorml));
        Destroy(blood.gameObject, blood.main.duration);
    }
    private void Reload() 
    {
        if(!isReloading && currntAmmo < maxAmmo) 
        {
            if(isShootingInput&& isWalking)
            {
                animator.SetTrigger("ShootReload");
            }
            else 
            {
                animator.SetTrigger("Reload");
            }
          //  soundAudioSource.PlayOneShot(reloadingSoundClip);
            isReloading = true;
            Invoke(nameof(FininishReload), reloadTime);
            //play reload Sound
        }
    }

    private void FininishReload() 
    {
        currntAmmo = maxAmmo;
        isReloading= false;
        if (isShootingInput && isWalking)
        {
            animator.ResetTrigger ("ShootReload");
        }
        else
        {
            animator.ResetTrigger("Reload");
        }
    }

}
