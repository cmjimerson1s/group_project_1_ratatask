using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class SCR_EndOfGameScript : MonoBehaviour
{
    [SerializeField] private GameObject gameEndCanvas;
    [SerializeField] private int smallScore;
    [SerializeField] private int mediumScore;
    [SerializeField] private int largeScore;
    [SerializeField] private GameObject smallScoreImage;
    [SerializeField] private GameObject mediumScoreImage;
    [SerializeField] private GameObject largeScoreImage;

    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private Button restartButton;

    private SCR_DayNightCycle dayNightCycle;
    private SCR_PlayerScores playerScores;
    private int collectables;
    
    private void Awake() {
        dayNightCycle = FindFirstObjectByType<SCR_DayNightCycle>();
        playerScores = FindFirstObjectByType<SCR_PlayerScores>();
    }

    private void OnEnable() {
        dayNightCycle.OnGameEnd += GameEnd;
    }

    private void OnDisable() {
        dayNightCycle.OnGameEnd -= GameEnd;
    }

    void GameEnd() {
        collectables = playerScores.collectableScore;
        gameEndCanvas.SetActive(true);  
        //mainMenuCanvas.SetActive(false);

        eventSystem.firstSelectedGameObject = null;
        eventSystem.firstSelectedGameObject = restartButton.gameObject;
        eventSystem.SetSelectedGameObject(restartButton.gameObject);
        
        if (collectables <= smallScore)
        {
            smallScoreImage.SetActive(true);
        }
        if (collectables <= mediumScore && collectables > smallScore)
        {
            mediumScoreImage.SetActive(true);
        }
        if (collectables >= largeScore)
        {
            largeScoreImage.SetActive(true);
        }
    }


      
}
