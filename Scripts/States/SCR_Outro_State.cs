using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OutroState : State
{
    [SerializeField] GameObject endOfGameCanvas;
    [SerializeField] TextMeshProUGUI nutsEndText;
    [SerializeField] SCR_PlayerScores playerScores;
    [SerializeField] private PlayableDirector outroCinamatic;
    [SerializeField] private bool skipOutroCinamatic;

    [Header("UI")]
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private Button firstButton;
    bool endGame = false;
    float outroTimeDelay = 4;
    float outroTime = 0;


    void Awake()
    {
        outroCinamatic.played += CinamaticStart;
        outroCinamatic.stopped += CinamaticEnd;
    }


    public override void EnterState()
    {
        base.EnterState(); 
        endGame = true;

        nutsEndText.text += playerScores.collectableScore;
        

        outroCinamatic.gameObject.SetActive(true);
        endOfGameCanvas.SetActive(false);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if(skipOutroCinamatic)
        {
            outroCinamatic.gameObject.SetActive(false);

            endOfGameCanvas.SetActive(true);
            eventSystem.firstSelectedGameObject = firstButton.gameObject;
            eventSystem.SetSelectedGameObject(firstButton.gameObject);
        }
        else
        {
            outroCinamatic.Play();
        }

        if (outroTime < outroTimeDelay + 1)
        {
            outroTime += Time.deltaTime;
        }
    }


    public void Restart()
    {
        if (outroTime > outroTimeDelay || !endGame)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            endGame = false;
        }
    }


    public void GoToMainMenu()
    {
        if (outroTime > outroTimeDelay || !endGame)
        {
            SceneManager.LoadScene(0); // 0 = Main menu scene 
            endGame = false;
        }
    }


    public override void ExitState()
    {
        base.ExitState();

        endOfGameCanvas.SetActive(false);
    }


    private void CinamaticStart(PlayableDirector obj) // An action for when the cinamatics starts
    {
        SCR_SceneManager.instance.dayAndNightScr.UpdateLightning(199);
    }


    private void CinamaticEnd(PlayableDirector obj) // An action for when the cinamatics ends
    {
        outroCinamatic.gameObject.SetActive(false);

        endOfGameCanvas.SetActive(true);
        eventSystem.firstSelectedGameObject = firstButton.gameObject;
        eventSystem.SetSelectedGameObject(firstButton.gameObject);
    }
}
