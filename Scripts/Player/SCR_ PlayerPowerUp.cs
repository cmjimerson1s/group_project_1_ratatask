using System;
using UnityEngine;

public class SCR_PlayerPowerUp : MonoBehaviour
{
    [Header("Timer")]
    [SerializeField] float powerUpTime;
    float timer;

    [Header("Ray of light powerup")]
    [HideInInspector] public bool shooting;
    private string ENEMYTAG = "";
    [SerializeField] LayerMask TEMP_DoesLayersWorkLikeThis;

    [Header("Shield powerup")]
    public Action startShield;


    void Start()
    {
        timer = powerUpTime;
        ENEMYTAG = SCR_SceneManager.instance.enemyTag;
    }


    public void StartShield()
    {
        startShield?.Invoke();
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * 100);
    }
}
