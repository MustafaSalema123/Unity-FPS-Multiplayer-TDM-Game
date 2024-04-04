using Photon.Pun;

using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{

    [Header("player health")]
    const float maxHealth = 150f;
    public float currentHealth;
    public Slider healthSlider;
    public GameObject playerUi;


    [Header("Ref and Physics")]
    InputManager inputManager;
    PlayerManager playerManager;
    AnimatorManager animatorManager;
    PlayerControlManager playerControlManager;
    Vector3 moveDirection;

    Transform camearGameObject;
    Rigidbody playerRigidbody;

    [Header("Falling and Landing")]
    public float isAirTimer;
    public float leapingVelocity;
    public float fallingVelocity;
    public float rayCastHeightffset = 0.5f;
    public LayerMask groundLayer;



    [Header("Movement Flag")]
    public bool isMoving ;
    public bool isSprinting;
    public bool isGrounded;
    public bool isJumping;

    [Header("Movement Value")]
    public float Movementspeed = 2f;
    public float rotationSpeed = 1f;
    public float sprintingSpeed = 1f;

    [Header("Jump Value")]
    public float jumpHeight = 4f;
    public float gravityintensity = -15f;

    PhotonView View;
    public int playerTeam;
    private void Awake()
    {
        currentHealth = maxHealth;
        inputManager = GetComponent<InputManager>();
        playerManager = GetComponent<PlayerManager>();
        playerRigidbody = GetComponent<Rigidbody>();
        animatorManager = GetComponent<AnimatorManager>();

        camearGameObject = Camera.main.transform;
        View= GetComponent<PhotonView>();

        playerControlManager  = PhotonView.Find((int)View.InstantiationData[0]).GetComponent<PlayerControlManager>();

        healthSlider.minValue= 0;
        healthSlider.maxValue= maxHealth;
        healthSlider.value= currentHealth;
    }
    private void Start()
    {
        if (!View.IsMine) 
        {
            Destroy(playerUi);
            Destroy(playerRigidbody);
        }

        if (View.Owner.CustomProperties.ContainsKey("Team"))
        {

            int Team = (int)View.Owner.CustomProperties["Team"];
            playerTeam = Team;

        }
    }

    public void HandleAllInput() 
    {
      

        HandleFallingandLanding();
        if (playerManager.isInteracting)
            return;

        HandleMovement();
        HandleRotation();
    }
     void HandleMovement() {

        if (isJumping)
            return;

        //moveDirection = camearGameObject.forward * inputManager.verticleInput;
        moveDirection = new Vector3(camearGameObject.forward.x , 0 , camearGameObject.forward.z) * inputManager.verticleInput;

        moveDirection = moveDirection + camearGameObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();

        moveDirection.y = 0;

        if (isSprinting) 
        {
            moveDirection = moveDirection * sprintingSpeed;
        }
        else 
        {
            if(inputManager.movementAmount >= 0.5f) 
            {
            
            moveDirection = moveDirection* Movementspeed;
                    isMoving= true;
            }
            if (inputManager.movementAmount <= 0f)
            {

                //moveDirection = moveDirection * Movementspeed;
                isMoving = false;
            }
        }

        Vector3 movementVelocity = moveDirection;
        playerRigidbody.velocity = movementVelocity;    

    }

     void HandleRotation()
    {
        if (isJumping)
            return;



        Vector3 targetDirection; //= Vector3.zero;

        targetDirection = camearGameObject.forward * inputManager.verticleInput;
        targetDirection = targetDirection + camearGameObject.right * inputManager.horizontalInput;

        targetDirection.Normalize();
        targetDirection.y = 0;

        if(targetDirection == Vector3.zero) 
        {
        targetDirection = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerSmoothRotation = Quaternion.Slerp(transform.rotation , targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation= playerSmoothRotation;

    }

    void HandleFallingandLanding() 
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        Vector3 targetPosition;
        rayCastOrigin.y = rayCastOrigin.y + rayCastHeightffset;
        targetPosition = transform.position;

        if (!isGrounded && !isJumping) 
        {
            if (!playerManager.isInteracting) {
                animatorManager.PlayTargetAnim("Falling", true);
            }

            isAirTimer = isAirTimer + Time.deltaTime;
            playerRigidbody.AddForce(transform.forward * leapingVelocity);
            playerRigidbody.AddForce(-Vector3.up * fallingVelocity * isAirTimer);
            }

       if( Physics.SphereCast(rayCastOrigin , 0.2f , -Vector3.up , out hit , groundLayer)) 
        {
        if(!isGrounded && !playerManager.isInteracting) 
            {
                animatorManager.PlayTargetAnim("Landing", true);
            }

            Vector3 raycastHitPoint = hit.point;
            targetPosition.y = raycastHitPoint.y;

            isAirTimer = 0;
            isGrounded= true;
        }
        else 
        {
        isGrounded= false;
        }
       if(isGrounded && !isJumping)
        {
            if (playerManager.isInteracting || inputManager.movementAmount > 0)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / 0.1f);
            }
            else 
            {
            transform.position = targetPosition;
            }

        }
    }

   public  void HandleJump() 
    {
        if (isGrounded) 
        {
            animatorManager.animator.SetBool("isJumping", true);

            animatorManager.PlayTargetAnim("Jump", false);

            float jumpVelocity = Mathf.Sqrt(-2 * gravityintensity * jumpHeight);
            Vector3 playerVelocity = moveDirection;
            playerVelocity.y = jumpVelocity;
            playerRigidbody.velocity= playerVelocity;

            isJumping = false;
        }
    }

    public void SetIsJumping(bool isJump) 
    {

        this.isJumping= isJump;
    }
    public void ApplyDamage(float damagevalue) 
    {
        //, RpcTarget.All all player
        View.RPC("RPC_takeDamae", RpcTarget.All, damagevalue);


    }
    [PunRPC]
    void RPC_takeDamae(float Damage) {

    if (!View.IsMine)
        return;

        currentHealth -= Damage;
        healthSlider.value = currentHealth;
        if (currentHealth <= 0) 
        {
            
            Die();
        }

    }
    void Die() 
    {
      
    playerControlManager.Die();
        //Incread Score

        ScoreBoard.Instance.PlayerDiedTeamVise(playerTeam);
    }

 }
