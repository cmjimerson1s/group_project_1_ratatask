using UnityEngine;

public class SCR_RayOfLightPickUp : Interactable
{
    [SerializeField] private int scoreWorth;

    
    protected override void OnTriggerAction(Collider collider)
    {
        base.OnTriggerAction(collider);
        SCR_SceneManager.instance.OnRayPickup?.Invoke();
        // collider.GetComponent<SCR_PlayerScores>().AddCollectableScore(scoreWorth);

        collider.GetComponent<StartRayOfLight>().StartRay();
    }
}
