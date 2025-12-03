using System;
using UnityEngine;

public class SCR_Random_Per_Seconds : MonoBehaviour
{
    public Action onChanceHasLanded;
    float sec;

    [Header("Randomness")]
    [SerializeField] float secondsBetweenSpawnChance;
    [Tooltip("This means the chance of spawning will be one in {number}, and it is ran every second")]
    [SerializeField] int oneInNumberPerSecondChance;
    [SerializeField] int landedChance;

    void Start()
    {
        sec = secondsBetweenSpawnChance;
    }

    void Update()
    {
        sec -= Time.deltaTime;
        if (sec < 0)
        {
            PerSecondUpdate();
            sec = secondsBetweenSpawnChance;
        }
    }

    void PerSecondUpdate()
    {
        landedChance = UnityEngine.Random.Range(1, oneInNumberPerSecondChance + 1);
        if (landedChance == 1)
        {
            onChanceHasLanded?.Invoke();
        }
    }
}
