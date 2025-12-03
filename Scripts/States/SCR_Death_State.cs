using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SCR_Death_State : State
{
    [SerializeField] private GameObject deathCanvas;
    [SerializeField] private float inputLag;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private Button firstButton;
    private float timer;


    public override void EnterState()
    {
        base.EnterState();
        
        deathCanvas.SetActive(true);
        timer = inputLag;

        eventSystem.firstSelectedGameObject = firstButton.gameObject;
        eventSystem.SetSelectedGameObject(firstButton.gameObject);
    }


    public override void UpdateState()
    {
        base.UpdateState();

        timer -= Time.deltaTime;
    }


    public void Restart()
    {
        if(timer <= 0) 
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }


    public void BackToMainMenu()
    {
        if(timer <= 0) 
        {
            SceneManager.LoadScene(0);
        }
    }


    public override void ExitState()
    {
        base.ExitState();

        deathCanvas.SetActive(false);
    }
}
