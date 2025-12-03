using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseState : State
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] EventSystem eventSystem;
    [SerializeField] Button backButton;
    [SerializeField] InputActionReference pauseButton;


    public override void EnterState() // Set pause UI active
    {
        base.EnterState();

        eventSystem.firstSelectedGameObject = backButton.gameObject;
        eventSystem.SetSelectedGameObject(backButton.gameObject);

        pauseMenu.SetActive(true);
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }


    public void ExitPauseUI()
    {
        myStateMachine.SwitchState<PlayingState>();
    }


    public override void ExitState() // Deactivte pauseUI
    {
        base.ExitState();

        pauseMenu.SetActive(false);
    }
}
