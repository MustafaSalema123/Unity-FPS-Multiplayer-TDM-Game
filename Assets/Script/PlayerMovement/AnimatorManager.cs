
using System.Collections.Generic;
using UnityEngine;


public class AnimatorManager : MonoBehaviour
{

   public Animator animator;

    int horizontalValue , verticalValue ;
    void Awake()
    {
        animator= GetComponent<Animator>();
        horizontalValue = Animator.StringToHash("Horizontal");
        verticalValue = Animator.StringToHash("Vertical");
    }

   public void ChangeAnimationValues(float HorizontalMovment , float VerticalMovment , bool isSprinting) 
    {
        float snappedHorizontalMovement;
        float snappedVerticleMovement;

        #region Snapped Horizontal
        if (HorizontalMovment > 0 && HorizontalMovment < 0.55f)
        {

            snappedHorizontalMovement = 0.5f;
        }
        
        else if (HorizontalMovment > 0.55f) { 
                    
        snappedHorizontalMovement = 1f;
        }
        else if (HorizontalMovment < 0 && HorizontalMovment > -0.55f) {
            snappedHorizontalMovement  = -0.55f;
        }
        else if (HorizontalMovment < -0.55f) {
            snappedHorizontalMovement  = -1f;

        }
        else {
            snappedHorizontalMovement = 0f;
        }
        #endregion
        #region  SnappedVerticleMovement
        if (VerticalMovment > 0 && VerticalMovment < 0.55f)
        {

            snappedVerticleMovement = 0.5f;
        }

        else if (VerticalMovment > 0.55f)
        {

            snappedVerticleMovement = 1f;
        }
        else if (VerticalMovment < 0 && VerticalMovment > -0.55f)
        {
            snappedVerticleMovement = -0.55f;
        }
        else if (VerticalMovment < -0.55f)
        {
            snappedVerticleMovement = -1f;

        }
        else
        {
            snappedVerticleMovement = 0f;
        }
        #endregion

        if (isSprinting) 
        {
            snappedHorizontalMovement = HorizontalMovment;
            snappedVerticleMovement = 2;
        }

        animator.SetFloat(horizontalValue, snappedHorizontalMovement, 0.1f , Time.deltaTime);
        animator.SetFloat(verticalValue, snappedVerticleMovement, 0.1f , Time.deltaTime);
    }

    public void PlayTargetAnim(string targetAnim,  bool isInteracting)
    {
        animator.SetBool("isInteracting", isInteracting);
        animator.CrossFade(targetAnim, 0.2f);
    }
}
