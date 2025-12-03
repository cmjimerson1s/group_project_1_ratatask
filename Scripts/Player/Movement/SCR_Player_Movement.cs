using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Splines;
using UnityEngine.UIElements;

public class SCR_Player_Movement : MonoBehaviour
{
    [Header("References")]
    SCR_Player pS;

    [Header("Movement")]
    [SerializeField] bool movePlayer;
    public float smoothMoveSpeed;
    [HideInInspector] public float defaultPlayerSpeed;
    public float playerSpeed;

    //Actions
    public Action onPlayerLeftDash;
    public Action onPlayerRightDash;
    public Action onPlayerBump;

    [Header("Lanes")]
    public float laneSwitchingSpeed;
    float playerTargetPosition;
    float playerCurrentPosition;

    [Header("Spline Movement")]
    [SerializeField, Range(0f, 1f)] public float progress = 0f;

    [Header("Player Dash Control")]
    public bool leftDashActive;
    public bool rightDashActive;
    Vector3 laneOffsetDirection;
    public InputActionReference leftDash;
    public InputActionReference rightDash;
    Vector3 exactPosition;
    [HideInInspector] public Vector3 position;
    [HideInInspector] public Vector3 tangent;

    [Header("Shield")]
    public float heightSwitchingSpeed;
    public float camHeightSwitchingSpeed;
    public float camRotationSwitchingSpeed;
    public float camFovSwitchingSpeed;
    public float defaultShieldHeight;
    [HideInInspector] public bool shieldIsOn = false;
    [HideInInspector] public bool newShieldPicked = false;
    bool doShieldMovement = false;
    float heightOffset = 0;
    float lerpTime = 0f;
    float camPosLerpTime = 0f;
    float camRotLerpTime = 0f;
    float camFovLerpTime = 0f;

    GameObject camObj;
    Camera camComp;
    public Vector3 camPosOffset;
    public Vector3 camRotOffset;
    public float camFovOffset;
    Vector3 currCamPos;
    Vector3 currCamRot;
    float currFov;
    Vector3 oldCamPos;
    Vector3 oldCamRot;
    float oldFov;

    void Start()
    {
        pS = GetComponent<SCR_Player>();
        playerSpeed = defaultPlayerSpeed;
        camObj = SCR_SceneManager.instance.mainCamera;
        camComp = camObj.GetComponent<Camera>();
    }

    public void PlayerMovementUpdate()
    {
        if (movePlayer)
        {
            PlayerForwardMovement();
            OldInputMovement();
        }

        if (shieldIsOn) doShieldMovement = true;

        if (doShieldMovement) ShieldHeightMovement();

    }
    bool startedShield = false;
    void ShieldHeightMovement()
    {
        if (!startedShield)
        {startedShield = true;

            currCamPos = camPosOffset;
            currCamRot = camRotOffset;

            oldCamPos = camObj.transform.localPosition;
            oldCamRot = camObj.transform.localRotation.eulerAngles;

            oldFov = camComp.fieldOfView;

            pS.animator.SetBool("ShieldOn", true);
        }

        if (shieldIsOn)
        {
            lerpTime += Time.deltaTime * heightSwitchingSpeed;
            lerpTime = Mathf.Clamp01(lerpTime);
            camPosLerpTime += Time.deltaTime * camHeightSwitchingSpeed;
            camPosLerpTime = Mathf.Clamp01(camPosLerpTime);
            camRotLerpTime += Time.deltaTime * camHeightSwitchingSpeed;
            camRotLerpTime = Mathf.Clamp01(camRotLerpTime);
            camFovLerpTime += Time.deltaTime * camFovSwitchingSpeed;
            camFovLerpTime = Mathf.Clamp01(camFovLerpTime);
        }
        else
        {
            pS.animator.SetBool("ShieldOn", false);
            lerpTime -= Time.deltaTime * heightSwitchingSpeed;
            lerpTime = Mathf.Clamp01(lerpTime);
            camPosLerpTime -= Time.deltaTime * camHeightSwitchingSpeed;
            camPosLerpTime = Mathf.Clamp01(camPosLerpTime);
            camRotLerpTime -= Time.deltaTime * camHeightSwitchingSpeed;
            camRotLerpTime = Mathf.Clamp01(camRotLerpTime);
            camFovLerpTime -= Time.deltaTime * camFovSwitchingSpeed;
            camFovLerpTime = Mathf.Clamp01(camFovLerpTime);
        }

        heightOffset = Mathf.Lerp(position.y, position.y + defaultShieldHeight, lerpTime);
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(position.x, heightOffset, position.z), smoothMoveSpeed * Time.deltaTime);

        currCamPos.x = Mathf.Lerp(oldCamPos.x, oldCamPos.x + camPosOffset.x, camPosLerpTime);
        currCamPos.y = Mathf.Lerp(oldCamPos.y, oldCamPos.y + camPosOffset.y, camPosLerpTime);
        currCamPos.z = Mathf.Lerp(oldCamPos.z, oldCamPos.z + camPosOffset.z, camPosLerpTime);

        camObj.transform.localPosition = currCamPos;

        currCamRot.x = Mathf.Lerp(oldCamRot.x, oldCamRot.x + camRotOffset.x, camRotLerpTime);
        currCamRot.y = Mathf.Lerp(oldCamRot.y, oldCamRot.y + camRotOffset.y, camRotLerpTime);
        currCamRot.z = Mathf.Lerp(oldCamRot.z, oldCamRot.z + camRotOffset.z, camRotLerpTime);

        camObj.transform.localRotation = Quaternion.Euler(currCamRot);

        currFov = Mathf.Lerp(oldFov, oldFov + camFovOffset, camFovLerpTime);

        camComp.fieldOfView = currFov;


        if (lerpTime == 0 && camPosLerpTime == 0 && camRotLerpTime == 0 && camFovLerpTime == 0 && !shieldIsOn)
        {
            startedShield = false;
            doShieldMovement = false;
            SCR_SceneManager.instance.obstacleManager.spawnedShieldSet = false;
        }
    }

    void PlayerForwardMovement()
    {
        if (!pS.bumpingScript.currentlyBumping)
        {
            playerSpeed = defaultPlayerSpeed;
        }
        progress += playerSpeed * Time.deltaTime / pS.playerPath.CalculateLength();
        progress %= 1f;

        //exactPosition = pS.playerPath.EvaluatePosition(progress);
        position = pS.playerPath.EvaluatePosition(progress); //Vector3.MoveTowards(transform.position, position, smoothMoveSpeed * Time.deltaTime);
        tangent = pS.playerPath.EvaluateTangent(progress);
        Quaternion rotation = Quaternion.LookRotation(tangent);

        laneOffsetDirection = Vector3.Cross(tangent, Vector3.up).normalized;

        if ((!leftDashActive && !rightDashActive) || (leftDashActive && rightDashActive))
        {
            pS.animator.SetInteger("oldTarget", pS.animator.GetInteger("targetPos"));
            pS.animator.SetInteger("targetPos", 0);
            playerTargetPosition = 0;
        }
        else if (leftDashActive)
        {
            pS.animator.SetInteger("oldTarget", pS.animator.GetInteger("targetPos"));
            pS.animator.SetInteger("targetPos", -1);
            playerTargetPosition = 1 * pS.laneOffset;
        }
        else if (rightDashActive)
        {
            pS.animator.SetInteger("oldTarget", pS.animator.GetInteger("targetPos"));
            pS.animator.SetInteger("targetPos", 1);
            playerTargetPosition = -1 * pS.laneOffset;
        }
        if (!Mathf.Approximately(playerCurrentPosition, playerTargetPosition))//playerCurrentPosition != playerTargetPosition)
        {
            playerCurrentPosition = Mathf.Lerp(playerCurrentPosition, playerTargetPosition, laneSwitchingSpeed * Time.deltaTime);
        }
        if (!(Mathf.Abs(playerCurrentPosition - playerTargetPosition) < 0.5))
        {
            pS.animator.SetBool("Idle", false);
        }
        else
        {
            pS.animator.SetBool("Idle", true);
        }

        position += laneOffsetDirection * playerCurrentPosition;

        if (!doShieldMovement) transform.position = Vector3.MoveTowards(transform.position, position, smoothMoveSpeed * Time.deltaTime);

        transform.rotation = rotation;

    }

    void OldInputMovement()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            leftDashActive = true;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            rightDashActive = true;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            leftDashActive = false;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            rightDashActive = false;
        }

    }

    private void OnEnable()
    {
        leftDash.action.started += LeftDashPress;
        leftDash.action.canceled += LeftDashUp;
        leftDash.action.Enable();
        rightDash.action.started += RightDashPress;
        rightDash.action.canceled += RightDashUp;
        rightDash.action.Enable();

    }

    private void OnDisable()
    {
        leftDash.action.started -= LeftDashPress;
        leftDash.action.canceled -= LeftDashUp;
        leftDash.action.Disable();
        rightDash.action.started -= RightDashPress;
        rightDash.action.canceled -= RightDashUp;
        rightDash.action.Disable();
    }

    private void LeftDashPress(InputAction.CallbackContext obj)
    {
        leftDashActive = true;
        onPlayerLeftDash?.Invoke();
    }

    private void LeftDashUp(InputAction.CallbackContext obj)
    {
        leftDashActive = false;
        onPlayerRightDash?.Invoke();
    }

    private void RightDashPress(InputAction.CallbackContext obj)
    {
        rightDashActive = true;
        onPlayerRightDash?.Invoke();
    }
    private void RightDashUp(InputAction.CallbackContext obj)
    {
        rightDashActive = false;
        onPlayerLeftDash?.Invoke();
    }

}
