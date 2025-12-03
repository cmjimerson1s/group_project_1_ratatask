using TMPro;
using UnityEngine.InputSystem;
using UnityEngine;

public class SCR_Player_Speed_Testing : MonoBehaviour
{
    [Header("References")]
    SCR_Player pS;

    TextMeshProUGUI text;
    [Header("SpeedChange")]
    [SerializeField] float speedChangeMultiplier;
    public InputActionReference speedUp;
    public InputActionReference speedDown;
    private bool speedIncrease;
    private bool speedDecrease;

    private void Start()
    {
        pS = GetComponent<SCR_Player> ();
        text = SCR_SceneManager.instance.speedText;
    }

    void Update()
    {
        ChangeDefaultSpeed();
        text.text = $"Speed: {pS.movementScript.defaultPlayerSpeed}";
    }

    void ChangeDefaultSpeed()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            AddSpeed(false);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            AddSpeed(true);
        }
        if (speedDecrease)
        {
            AddSpeed(false);
        }
        if (speedIncrease)
        {
            AddSpeed(true);
        }
    }

    void AddSpeed(bool increase)
    {
        if (!increase)
        {
            pS.movementScript.defaultPlayerSpeed -= speedChangeMultiplier * Time.deltaTime;
        }
        if (increase)
        {
            pS.movementScript.defaultPlayerSpeed += speedChangeMultiplier * Time.deltaTime;
        }
    }

    private void OnEnable() {
        speedUp.action.started += IncreaseSpeed;
        speedUp.action.canceled += IncreaseSpeedStop;
        speedUp.action.Enable();
        speedDown.action.started += DecreaseSpeed;
        speedDown.action.canceled += DecreaseSpeedStop;
        speedDown.action.Enable();
    }

    private void OnDisable() {
        speedUp.action.started -= IncreaseSpeed;
        speedUp.action.canceled -= IncreaseSpeedStop;
        speedUp.action.Disable();
        speedDown.action.started -= DecreaseSpeed;
        speedDown.action.canceled -= DecreaseSpeedStop;
        speedDown.action.Disable();
    }

    private void IncreaseSpeed(InputAction.CallbackContext obj) {
        speedIncrease = true;
    }
    private void IncreaseSpeedStop(InputAction.CallbackContext obj) {
        speedIncrease = false;
    }

    private void DecreaseSpeed(InputAction.CallbackContext obj) {
        speedDecrease = true;
    }
    private void DecreaseSpeedStop(InputAction.CallbackContext obj) {
        speedDecrease = false;
    }
}
