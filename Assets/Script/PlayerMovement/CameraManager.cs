using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraManager : MonoBehaviour
{
    InputManager inputManager;

    public Transform playerTransform;

    public Transform cameraTransform;
    [Header("camera Movement")]
    public Transform cameraPivot;
    private Vector3 cameraFollowVelocity = Vector3.zero;
    public float camearaFollowSpeed = 0.3f; 
    public float camLookSpeed = 2f , camPivotSpeed = 2;
    public float lookAngle, pivotAngle;

    [Header("Collision Movement")]
    public LayerMask collisionLayer;
    private float defaultPosition;

    public float cameraCollisionOffset = 0.2f , minCollisionOffset = 0.2f , camearaCollisionRadius = 0.2f ;
    private Vector3 cameraVectorPosition;


    private PlayerMovement playermovemnt;
    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
        playermovemnt = FindObjectOfType<PlayerMovement>();

        playerTransform= FindObjectOfType<PlayerManager>().transform;
        cameraTransform = Camera.main.transform;
        defaultPosition = cameraTransform.localPosition.z;
    }

    public void HandleAllCamraMovemnt() 
    {
        followTarget();

        RotationCamera();
        CameraCollision();
    }

       void followTarget() 
    {
    Vector3 targetPosition = Vector3.SmoothDamp(transform.position , playerTransform.position , ref cameraFollowVelocity, camearaFollowSpeed);

        transform.position = targetPosition;
    }

     void RotationCamera()
    {
        Vector3 rotation;

        lookAngle  = lookAngle + (inputManager.cameraInputX* camLookSpeed);
        pivotAngle = pivotAngle + (inputManager.cameraInputY * camPivotSpeed);
        pivotAngle = Mathf.Clamp(pivotAngle, -30, 30);

        rotation = Vector3.zero;
        rotation.y = lookAngle;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = -pivotAngle;
        targetRotation = Quaternion. Euler(rotation);
        cameraPivot.localRotation =targetRotation;

        if(playermovemnt.isMoving==false && playermovemnt.isSprinting == false) 
        {
        playerTransform.rotation = Quaternion.Euler(0,lookAngle,0);
        }

    }

    void CameraCollision()
    {
        float targetPosition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivot.position;
        direction.Normalize();
        if(Physics.SphereCast(cameraPivot.transform.position, cameraCollisionOffset , direction , out hit, Mathf.Abs(targetPosition) , collisionLayer))
        {
            float distance = Vector3.Distance(cameraPivot.position, hit.point);
            targetPosition =- (distance - cameraCollisionOffset);
        }
        if(Mathf.Abs(targetPosition) < minCollisionOffset) 
        {
            targetPosition = targetPosition - minCollisionOffset;

        }
        cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, 0.2f);
        cameraTransform.localPosition = cameraVectorPosition;
    }
}
