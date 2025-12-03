using UnityEngine;
using System;
using UnityEngine.UIElements;

public class SCR_PlayerShield : SCR_PowerUp
{
    [Header("References")]
    private SCR_Player pS;
    [Header("Great Header Here")]
    public float playerSpeedBoost;
    private float oldPlayerSpeed;
    [HideInInspector] public float shieldUpTime;
    private float shieldTimer;

    private string oldObstacleTag;
    private string oldEnemyTag;
    private float maxSpeed;

    public Action startShield;

    void Start()
    {
        pS = GetComponent<SCR_Player>();
        oldPlayerSpeed = pS.movementScript.defaultPlayerSpeed;
        oldObstacleTag = pS.obstacleTag;
        maxSpeed = pS.movementScript.defaultPlayerSpeed + playerSpeedBoost;
        oldEnemyTag = pS.enemyTag;

        shieldTimer = 0;
    }


    public override void StartPowerUp()
    {
        base.StartPowerUp();

        pS.animator.SetTrigger("Speed_Up");
        if (pS.movementScript.shieldIsOn)
        {
            pS.movementScript.newShieldPicked = true;
        }
        pS.movementScript.shieldIsOn = true;

        if(maxSpeed != pS.movementScript.defaultPlayerSpeed + playerSpeedBoost) maxSpeed = pS.movementScript.defaultPlayerSpeed + playerSpeedBoost;

        if (pS.movementScript.defaultPlayerSpeed < maxSpeed)
        {
            oldPlayerSpeed = pS.movementScript.defaultPlayerSpeed;
            pS.movementScript.defaultPlayerSpeed += playerSpeedBoost;
        }

        if (pS.obstacleTag != "SHIELD" && SCR_SceneManager.instance.enemyTag != "SHIELD")
        {
            oldObstacleTag = pS.obstacleTag;
            oldEnemyTag = pS.enemyTag;
        }
        pS.obstacleTag = "SHIELD";
        pS.enemyTag = "SHIELD";
    }


    protected override void EndPowerUp()
    {
        base.EndPowerUp();

        pS.movementScript.defaultPlayerSpeed = oldPlayerSpeed;
        pS.movementScript.shieldIsOn = false;

        pS.obstacleTag = oldObstacleTag;
        pS.enemyTag = oldEnemyTag;
    }


    public override void UpdatePowerUp()
    {
        if (powerUpTimer > 0)
        {
            powerUpTimer -= Time.deltaTime;

            base.UpdatePowerUp();
            if (powerUpTimer <= 0) EndPowerUp();
        }
    }


    public void TurnOnShield()
    {

        pS.animator.SetTrigger("Speed_Up");
        pS.movementScript.shieldIsOn = true;

        if (pS.movementScript.defaultPlayerSpeed < maxSpeed)
        {
            oldPlayerSpeed = pS.movementScript.defaultPlayerSpeed;
            pS.movementScript.defaultPlayerSpeed += playerSpeedBoost;
        }

        shieldTimer = shieldUpTime;

        if (pS.obstacleTag != "SHIELD" && SCR_SceneManager.instance.enemyTag != "SHIELD")
        {
            oldObstacleTag = pS.obstacleTag;
            oldEnemyTag = pS.enemyTag;
        }
        pS.obstacleTag = "SHIELD";
        pS.enemyTag = "SHIELD";
    }


    void TurnOffShield()
    {

        pS.movementScript.defaultPlayerSpeed = oldPlayerSpeed;
        pS.movementScript.shieldIsOn = false;

        pS.obstacleTag = oldObstacleTag;
        pS.enemyTag = oldEnemyTag;
    }
}
