using System;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UIElements;

public class SCR_ObstacleSpawnTrigger : MonoBehaviour
{

    public Action OnEndRowHit;
    float progress;
    Vector3 position;
    SCR_Player pS;
    SCR_ObstacleManager manager;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ENDROW"))
        {
            OnEndRowHit?.Invoke();
            other.gameObject.SetActive(false);
        }
    }
    void Start()
    {
        pS = SCR_SceneManager.instance.pS;
        manager = transform.parent.GetComponent<SCR_ObstacleManager>();
        manager.trigger = this;
    }

    public void UpdateObstacleTrigger()
    {
        progress = pS.movementScript.progress + (manager.spawnAheadOfPlayer - .3f);
        if (progress >= 1) progress -= 1;

        position = pS.playerPath.EvaluatePosition(progress);

        transform.position = position;

    }
}
