using NUnit.Framework;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Splines;
using System.Collections.Generic;
using Unity.Mathematics;

public class SCR_SceneManager : MonoBehaviour
{
    public static SCR_SceneManager instance;

    [Header("Default References")]
    public SCR_Player pS;
    public GameObject mainCamera;
    public SCR_CameraController camScr;
    public List<SCR_Enemy> enemies = new();

    public SCR_DayNightCycle dayAndNightScr;
    SCR_ValueAdjustmentManager valueInstance;

    public SplineContainer playerPath;
    public SCR_ObstacleManager obstacleManager;

    public TextMeshProUGUI speedText;
    public GameObject hitText;

    public string obstacleTag = "OBSTACLE";
    public string enemyTag = "ENEMY";
    [HideInInspector] public float laneOffset;

    [Header("Power Ups")]
    public bool spawnShield = false;
    public bool spawnRay = false;

    // other values
    [Header("Actions")]
    public Action OnNutPickup;
    public Action OnShieldPickup;
    public Action OnRayPickup;

    [Header("Vfx")]
    public SCR_Object_Pooling enemyDeathVfxPool;

    [Header("Frames Per Second")]
    public bool countFps;
    public int fps;
    public int difference;
    public TextMeshProUGUI fpsText;
    int oldFps;
    int fpsCount;
    float secs;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void GetValueAdjustments()
    {
        valueInstance = SCR_ValueAdjustmentManager.instance;
        switch (SCR_ValueAdjustmentManager.instance.Difficulty)
        {
            case SCR_ValueAdjustmentManager.diffSets.normal:

                break;
        }
        laneOffset = valueInstance.laneOffset;
        obstacleManager.spacing = valueInstance.spacing;
        pS.movementScript.defaultPlayerSpeed = valueInstance.defaultPlayerSpeed;
        pS.movementScript.laneSwitchingSpeed = valueInstance.laneSwitchingSpeed;
        pS.bumpingScript.knockbackForce = valueInstance.bumpKnockback;
        pS.bumpingScript.accelerationSpeed = valueInstance.accelerationSpeed;
        pS.shieldScript.shieldUpTime = valueInstance.shieldDuration;
        pS.shieldScript.playerSpeedBoost = valueInstance.shieldSpeedBoost;
        // ray duration
        // ray range
    }

    void CountFps()
    {
        fpsCount++;
        secs += Time.deltaTime;
        if (secs >= 1)
        {
            oldFps = fps;
            fps = fpsCount;
            difference = fps - oldFps;
            fpsCount = 0;
            secs = 0;
            if (fpsText != null) { fpsText.text = $"Fps:{fps}\nDiff:{difference}"; }
        }
    }

    public void PlayerGotHit()
    {


        //hitText.SetActive(true);
        //StartCoroutine(GotHitLife());
    }

    IEnumerator GotHitLife()
    {
        yield return new WaitForSeconds(1f);
        hitText.SetActive(false);
        yield return null;
    }

    void Start()
    {
        GetValueAdjustments();
    }

    void Update()
    {
        if (countFps) CountFps();
        else { if (fpsText != null) fpsText.text = ""; }
    }
}
