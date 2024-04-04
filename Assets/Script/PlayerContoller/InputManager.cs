using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class InputManager : MonoBehaviour
{

    PlayerController playerController;
    //AnimatorManager animatorManager;
    //PlayerMovement playerMovement;

    public Vector2 movementInput;
    public Vector2 cameraMovemntInput;

    public float verticleInput , horizontalInput;
    public float cameraInputX, cameraInputY;
    public float movementAmount;

    [Header("Input Button Flags")]
    public bool bInput;
    public bool jumpInput;
    public bool fireInput;

    public bool reloadInput;

    public bool scopeInput;
    public bool crouchInput;
    public bool proneInput;
    private void Awake()
    {
      //  animatorManager = GetComponent<AnimatorManager>();
       // playerMovement = GetComponent<PlayerMovement>();
    }
    private void OnEnable()
    {
        if(playerController == null) 
        {
        playerController = new PlayerController();
            playerController.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerController.PlayerMovement.CamearaMovement.performed += i => cameraMovemntInput = i.ReadValue<Vector2>();

            playerController.PlayerAction.B.performed += i => bInput = true;
            playerController.PlayerAction.B.canceled += i => bInput = false;
            playerController.PlayerAction.Jump.performed += i => jumpInput = true;

            playerController.PlayerAction.Fire.performed += i => fireInput = true;
            playerController.PlayerAction.Fire.canceled  += i => fireInput = false ;

            playerController.PlayerAction.Reload.performed += i => reloadInput = true;
            playerController.PlayerAction.Reload.canceled += i => reloadInput = false;

            playerController.PlayerAction.Scope.performed += i => scopeInput = true;
            playerController.PlayerAction.Scope.canceled += i => scopeInput = false;

            playerController.PlayerAction.Crouch.performed += i => crouchInput = true;
            playerController.PlayerAction.Crouch.canceled += i => crouchInput = false;


           // playerController.PlayerAction.Prone.performed += i => proneInput = true;
           // playerController.PlayerAction.Prone.canceled += i => proneInput = false;
}

        playerController.Enable();
    }
    private void OnDisable()
    {
        playerController.Disable();
    }

    public void handleAllInput() 
    {
       // HandleMovementInput();
      //  HandleSprintInput();
        //HandleJumpInput();
    }


    //private void HandleMovementInput() 
    //{
    //    verticleInput = movementInput.y;
    //    horizontalInput = movementInput.x;

    //    cameraInputX = cameraMovemntInput.x;
    //    cameraInputY = cameraMovemntInput.y;

    //    movementAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticleInput));
    //    animatorManager.ChangeAnimationValues(0,movementAmount,playerMovement.isSprinting);
    //}

    //private void HandleSprintInput()
    //{
    //    if (bInput && movementAmount > 0.5f) 
    //    {
    //        playerMovement.isSprinting = true;
    //    }
    //    else 
    //    {
    //        playerMovement.isSprinting = false;
    //    }
    //}
    //private void HandleJumpInput()
    //{
    //    if (jumpInput) 
    //    {
    //    jumpInput= false;
    //        playerMovement.HandleJump();
    //    }
    //}
}
