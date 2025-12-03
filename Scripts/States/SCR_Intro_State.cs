using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.UI;

public class IntroState : State
{
    [SerializeField] GameObject introCanvas;
    [SerializeField] SCR_PlayerHealth playerHealth;
    [SerializeField] private PlayableDirector introCinamatic;
    [SerializeField] GameObject cam;
    [SerializeField] bool skipIntroCinamatic;
    [SerializeField] EventSystem eventSystem;
    [SerializeField] Button firstButton;
    SCR_CameraController camScr;
    GameObject introObj;


    void Awake()
    {
        introCinamatic.played += CinamaticStart;
        introCinamatic.stopped += CinamaticEnd;
        introObj = introCinamatic.gameObject;
    }

    private void Start()
    {
        camScr = SCR_SceneManager.instance.camScr;
    }

    public override void EnterState()
    {
        base.EnterState();

        eventSystem.firstSelectedGameObject = firstButton.gameObject;
        eventSystem.SetSelectedGameObject(firstButton.gameObject);

        introCanvas.SetActive(true);
    }

    public override void UpdateState()
    {
        base.UpdateState();

    }


    public void ShouldTakeDamage(bool aBool)
    {
        playerHealth.SetDamageBool(aBool);

        if (!skipIntroCinamatic)
        {
            introCinamatic.Play();
        }
        else
        {
            myStateMachine.SwitchState<PlayingState>();
            introCinamatic.gameObject.SetActive(false);

            camScr.SaveCamValues();

            camScr.transform.position = cam.transform.position;
            camScr.transform.rotation = cam.transform.rotation;
        }
    }


    public override void ExitState()
    {
        base.ExitState();

        introCanvas.SetActive(false);
    }


    private void CinamaticStart(PlayableDirector obj)
    {
        introCanvas.SetActive(false);
    }


    private void CinamaticEnd(PlayableDirector obj)
    {
        myStateMachine.SwitchState<PlayingState>();
        introCinamatic.gameObject.SetActive(false);

        camScr.SaveCamValues();

        camScr.transform.position = cam.transform.position;
        camScr.transform.rotation = cam.transform.rotation;
    }
}
