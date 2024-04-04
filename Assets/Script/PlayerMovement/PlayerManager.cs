using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
   InputManager inputManager;
    PlayerMovement playerMovement;
    public CameraManager cameraManager;

    Animator animator;
    public bool isInteracting;

    PhotonView View;
    void Awake()
    {
        View= GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        inputManager= GetComponent<InputManager>();
        playerMovement = GetComponent<PlayerMovement>();
        cameraManager= FindObjectOfType<CameraManager>();

    }
    private void Start()
    {
        if (!View.IsMine)
        {
            Destroy(GetComponentInChildren<CameraManager>().gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!View.IsMine) return;
        inputManager.handleAllInput();

    }

    private void FixedUpdate()
    {
        if (!View.IsMine) return;

        playerMovement.HandleAllInput();
    }

    private void LateUpdate()
    {
        if (!View.IsMine) return;
        cameraManager.HandleAllCamraMovemnt();

        isInteracting = animator.GetBool("isInteracting");
        playerMovement.isJumping = animator.GetBool("isJumping");
        animator.SetBool("isGrounded", playerMovement.isGrounded);
    }
}
