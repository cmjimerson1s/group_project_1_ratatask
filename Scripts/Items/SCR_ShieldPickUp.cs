using UnityEngine;

public class SCR_ShieldPickUp : Interactable
{
    protected override void OnTriggerAction(Collider collider)
    {
        base.OnTriggerAction(collider);
        SCR_SceneManager.instance.OnShieldPickup?.Invoke();
        collider.GetComponent<SCR_PlayerShield>().StartPowerUp();
    }
}
