using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;

public class SCR_Player_Bumping : MonoBehaviour
{
    [Header("References")]
    SCR_Player pS;

    [Header("Bumping Values")]
    public float knockbackForce;
    public float accelerationSpeed;
    public bool currentlyBumping;
    [SerializeField] private int bumpDamage;

    bool doKnockback = false;
    bool doPlayerAcceleration = false;

    bool startedAcceletarion = false;

    bool disableCollider = false;
    float currentColliderTimer;
    [SerializeField] float reEnableColliderTimer = 1;

    [Header("Counting Bumping Frames")]
    public bool countBumpingFrames;

    public float timeFromNegativeToZero;
    public float timeFromZeroToDefault;
    public float timeInABump;

    public float framesFromNegativeToZero;
    public float framesFromZeroToDefault;
    public float framesInABump;
    
    [SerializeField] List<SkinnedMeshRenderer> visualObj = new List<SkinnedMeshRenderer>();
    [SerializeField] float blinkDuration;
    float blinkTimer;
    float startBlinkTimer;
    bool hasPlayedAnim = false;
    bool turnOnRenderers = false;
    bool hasDoneFirstBlink = false;



    private void Start()
    {
        pS = GetComponent<SCR_Player>();
        blinkTimer = blinkDuration;
    }

    public void PlayerBumpingUpdate()
    {
        if (currentlyBumping) Bumping();

        if (disableCollider) ReEnableCollider();

        if (currentlyBumping && countBumpingFrames)
        {
            if (pS.movementScript.playerSpeed < 0)
            {
                AddToFrameCounting(true);
            }
            else
            {
                AddToFrameCounting(false);
            }
        }

        if (pS.animator.GetCurrentAnimatorStateInfo(0).IsName("collission")) hasPlayedAnim = true;

        // Blinking
        if(!pS.animator.GetCurrentAnimatorStateInfo(0).IsName("collission") && hasPlayedAnim && !pS.boxCollider.enabled)
        {
            if (blinkTimer <= 0)
            {
                blinkTimer += Time.deltaTime;


                if (blinkTimer >= 0)
                {
                    blinkTimer = blinkDuration / 2;

                    SwitchRenderersState(true);
                }
            }
            else
            {
                blinkTimer -= Time.deltaTime;

                if (!hasDoneFirstBlink)
                {
                    blinkTimer = -1000000;
                    hasDoneFirstBlink = true;  
                }

                if (blinkTimer <= 0)
                {
                    blinkTimer = -blinkDuration;

                    SwitchRenderersState(false);
                }
            }
        }
    }

    void ResetFrameCountingVariables()
    {
        timeFromNegativeToZero = 0;
        timeFromZeroToDefault = 0;
        timeInABump = 0;

        framesFromNegativeToZero = 0;
        framesFromZeroToDefault = 0;
        framesInABump = 0;
    }

    void AddToFrameCounting(bool fromNegativeToZero)
    {
        if (fromNegativeToZero)
        {
            timeFromNegativeToZero += Time.deltaTime;
            timeInABump += Time.deltaTime;

            framesFromNegativeToZero += 1;
            framesInABump += 1;
        }
        else
        {
            timeFromZeroToDefault += Time.deltaTime;
            timeInABump += Time.deltaTime;

            framesFromZeroToDefault += 1;
            framesInABump += 1;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(pS.obstacleTag) && !currentlyBumping)// && bumpDetectionAndOtherColliding)
        {
            currentlyBumping = true;
            pS.animator.SetTrigger("Collision");
            ResetFrameCountingVariables();
            pS.movementScript.onPlayerBump?.Invoke();
            startedAcceletarion = false;
            disableCollider = true;
            pS.boxCollider.enabled = false;
            currentColliderTimer = reEnableColliderTimer;
            doKnockback = true;
            other.GetComponent<Animator>().SetTrigger("Hit");
            pS.playerHealth.TakeDamage(bumpDamage);
        }
        if (other.CompareTag("TEST"))// && bumpDetectionAndOtherColliding)
        {
            transform.position = Vector3.zero;
        }
    }

    void ReEnableCollider()
    {
        currentColliderTimer -= Time.deltaTime;
        if (currentColliderTimer < 0)
        {
            currentColliderTimer = reEnableColliderTimer;
            pS.boxCollider.enabled = true;
            disableCollider = false;

            // Blinking and anim
            blinkTimer = blinkDuration;
            hasPlayedAnim = false;
            hasDoneFirstBlink = false;

            SwitchRenderersState(true);
        }
    }


    void SwitchRenderersState(bool abool)
    {
        foreach (SkinnedMeshRenderer renderer in visualObj)
        {
            renderer.enabled = abool;
        }
    }


    void Bumping()
    {
        if (doKnockback)
        {
            pS.movementScript.playerSpeed = -knockbackForce;
            doKnockback = false;
            doPlayerAcceleration = true;
        }
        else if (doPlayerAcceleration)
        {
            acceleratePlayer();
        }
    }

    public void StartPlayerAcceleration()
    {
        startedAcceletarion = true;
    }

    public void acceleratePlayer()
    {
        if (pS.movementScript.playerSpeed < pS.movementScript.defaultPlayerSpeed)
        {
            //Debug.Log("accelerating");
            pS.movementScript.playerSpeed += accelerationSpeed * Time.deltaTime;
        }
        else
        {
            pS.movementScript.playerSpeed = pS.movementScript.defaultPlayerSpeed;
            doPlayerAcceleration = false;
            currentlyBumping = false;
        }
    }

}