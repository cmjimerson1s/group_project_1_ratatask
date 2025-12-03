using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;

public class SCR_KeyBinding : MonoBehaviour
{
    public InputActionReference leftDash;
    public InputActionReference rightDash;
    public InputActionReference menuNav;
    public InputActionReference back;
    public InputActionReference select;
    public InputActionReference pause;

    public TextMeshProUGUI leftDashText;
    public Text leftDashNormalText;
    public TextMeshProUGUI rightDashText;
    public Text rightDashNormalText;
    public TextMeshProUGUI menuNavText;
    public Text menuNavNormalText;
    public TextMeshProUGUI backText;
    public Text backNormalText;
    public TextMeshProUGUI selectText;
    public Text selectNormalText;
    public TextMeshProUGUI pauseText;
    public Text pauseNormalText;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        leftDashText.text = leftDashNormalText.text;
        rightDashText.text = rightDashNormalText.text;
        menuNavText.text = menuNavNormalText.text;
        backText.text = backNormalText.text;
        selectText.text = selectNormalText.text;
        pauseText.text = pauseNormalText.text;
    }

    private void OnEnable() {

    }

    private void OnDisable() {

    }
}
