using UnityEngine;

public class SCR_GameManager : StateMachine
{
    #region Singleton

    public static SCR_GameManager instance = null;

    private void Awake()
    {
        if(instance == null) instance = this;
    }

    #endregion

    [SerializeField] bool startInPlayState_Test;


    void Start()
    {
        StartStateMachine();
        if(startInPlayState_Test)
        {
            SwitchState<PlayingState>();
        }
        else
        {
            SwitchState<IntroState>();
        }
    }


    void Update()
    {
        UpdateStateMachine();
    }
}
