using UnityEngine;

public class SCR_Player_AnimationEvents : MonoBehaviour
{
    public SCR_Player pS;

    public void StartPlayerAcceleration()
    {
        pS.bumpingScript.StartPlayerAcceleration();
    }
}
