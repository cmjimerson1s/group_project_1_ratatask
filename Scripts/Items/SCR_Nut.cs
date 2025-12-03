using UnityEngine;

public class SCR_Nut : Interactable
{
    [Header("Nut variables")]
    [SerializeField] int scoreWorth = 1;


    protected override void OnTriggerAction(Collider collider)
    {
        base.OnTriggerAction(collider);
        SCR_SceneManager.instance.OnNutPickup?.Invoke();
        collider.GetComponent<SCR_PlayerScores>().AddCollectableScore(scoreWorth);
    }
}
