using UnityEngine;

public class SCR_EnemyDeathVfx : MonoBehaviour
{
    float deathTimer = 4;
    float currentTimer;
    void Start()
    {
        currentTimer = deathTimer;
    }

    void Update()
    {
        currentTimer -= Time.deltaTime;
        if (currentTimer <= 0)
        {
            currentTimer = deathTimer;
            gameObject.SetActive(false);
        }
    }
}
