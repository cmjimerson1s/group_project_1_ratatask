using UnityEngine;

public class StartRayOfLight : MonoBehaviour
{
    [SerializeField] GameObject ray;
    SCR_Player pS;

    private void Start()
    {
        pS = GetComponent<SCR_Player>();
    }

    public void StartRay()
    {
        ray.GetComponent<SCR_PlayerRayLight>().StartPowerUp();
        pS.animator.SetBool("RayOn", true);
    }
}
