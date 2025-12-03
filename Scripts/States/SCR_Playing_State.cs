using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayingState : State
{
    [Header("Player")]
    [SerializeField] SCR_Player_Movement playerMovement;
    [SerializeField] SCR_Player_Bumping playerBumping;
    [SerializeField] SCR_PlayerShield playerShield;
    [Tooltip("Should not have anything in it in easier version")]
    [SerializeField] SCR_PlayerHealth playerHealth;

    [Header("Camera")]
    [SerializeField] SCR_CameraController cameraController;

    [Header("Managers")]
    [SerializeField] SCR_DayNightCycle dayNightCycle;
    [SerializeField] SCR_ObstacleManager obstacleManager;
    [SerializeField] SCR_ObstacleSpawnTrigger obstacleSpawnTrigger;

    [Header("UI")]
    [SerializeField] SCR_UI_Timer UITimer;
    [SerializeField] GameObject UITimerCanvas;
    [SerializeField] GameObject PlayingCanvas;

    [Header("PowerUps")]
    [SerializeField] List<SCR_PowerUp> powerUps = new();

    [Header("Misc")]
    [SerializeField] private InputActionReference pauseButton;
    [SerializeField] private float slowDownBuffer;
    [SerializeField] private float startSlowDown;
    private bool isPaused;


    public override void EnterState()
    {
        base.EnterState();

        isPaused = false;

        UITimerCanvas.SetActive(true);
        PlayingCanvas.SetActive(true);
    }

    public override void UpdateState()
    {
        base.UpdateState();

        // Updates Start

        UpdatePlayer();
        UpdateManagers();
        cameraController.UpdateCamera();
        UITimer.UpdateUITimer();
        foreach (SCR_PowerUp powerUp in powerUps)
        {
            powerUp.UpdatePowerUp();
        }

        foreach (SCR_Enemy enemy in SCR_SceneManager.instance.enemeis)
        {
            enemy.UpdateEnemy();
        }

        // Updates End 

        // Pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            myStateMachine.SwitchState<PauseState>();
        }

        // End of game
        if (dayNightCycle.TimeOfTheDay >= dayNightCycle.LenghtOfDay - 1)
        {
            myStateMachine.SwitchState<OutroState>();
        }

        // Death in harder version
        if (playerHealth != null)
        {
            if (playerHealth.IsDead())
            {
                myStateMachine.SwitchState<SCR_Death_State>();
            }
        }


        // Slower player movement
        if(dayNightCycle.TimeOfTheDay >=  dayNightCycle.LenghtOfDay - startSlowDown)
        {
            playerMovement.defaultPlayerSpeed -= Time.deltaTime * slowDownBuffer;
            if(playerMovement.defaultPlayerSpeed <= 0)
            {
                playerMovement.defaultPlayerSpeed = 0.5f;
            }
        }

    }


    private void OnEnable()
    {
        pauseButton.action.started += Switch;
        pauseButton.action.Enable();
    }


    private void OnDisable()
    {
        pauseButton.action.started -= Switch;
        pauseButton.action.Disable(); 
    }

    void Switch(InputAction.CallbackContext obj)
    {
        if (myStateMachine.currentState.GetType() != typeof(PlayingState) && myStateMachine.currentState.GetType() != typeof(PauseState)) return;

        if (isPaused)
        {
            isPaused = !isPaused;
            myStateMachine.SwitchState<PlayingState>();
        }
        else
        {
            isPaused = !isPaused;
            myStateMachine.SwitchState<PauseState>();
        }

        
    }

    private void UpdatePlayer()
    {
        playerMovement.PlayerMovementUpdate();
        playerBumping.PlayerBumpingUpdate();
        playerHealth.UpdatePlayerHealth();
    }


    private void UpdateManagers()
    {
        dayNightCycle.UpdateDayNightCycle();
        obstacleManager.UpdateObstacleManger();
        obstacleSpawnTrigger.UpdateObstacleTrigger();
    }


    public override void ExitState()
    {
        base.ExitState();

        UITimerCanvas.SetActive(false);
        PlayingCanvas.SetActive(false);
    }
}
