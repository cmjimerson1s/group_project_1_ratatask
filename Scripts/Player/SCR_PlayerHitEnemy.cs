using System;
using UnityEngine;

public class SCR_PlayerHitEnemy : MonoBehaviour
{

    SCR_Player pS;
    [SerializeField] int loseCollectableAmount;
    [SerializeField] int enemyDamage;
    [SerializeField] GameObject itemDropVFX;

    public Action OnEnemyHitByPlayer;

    private void Start()
    {
        pS = gameObject.GetComponent<SCR_Player>();
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag(pS.enemyTag))
        {
            //Debug.Log(pS.enemyTag);
            OnEnemyHitByPlayer?.Invoke();
            collider.gameObject.SetActive(false);
            SCR_SceneManager.instance.PlayerGotHit();
            gameObject.GetComponent<SCR_PlayerScores>().RemoveColletablescore(loseCollectableAmount);
            pS.playerHealth.TakeDamage(enemyDamage);
            itemDropVFX.SetActive(true);
        }
    }

}
