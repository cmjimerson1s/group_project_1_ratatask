using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class SCR_DayNightCycle : MonoBehaviour
{

    [SerializeField] private float TimeOfDay = 0;
    [SerializeField] private float lenghtOfDay = 100;

    [SerializeField] private float stopDayTime = 45;
    [SerializeField] private float startNightTime = 70;

    [SerializeField] private Volume nightTimeVolume;

    [Header("Shield Spawning")]
    [SerializeField] private float percentToSpawnShield;
    private float spawnReactivateTimerShield;
    float spawnSecondsPassedShield = 20;
    private bool spawnReactivateDelayedShield = true;
    [Header("Ray Spawning")]
    [SerializeField] private float percentToSpawnRay;
    private float spawnReactivateTimerRay;
    float spawnSecondsPassedRay = 20;
    private bool spawnReactivateDelayedRay = true;

    private bool isNightTime;
    private bool isEndOfGame;
    private Quaternion rotation;
    private float endWeight = 1;

    public Action OnNightTime;
    public Action OnDayTime;
    public Action OnGameEnd;


    // Getters
    public float TimeOfTheDay => TimeOfDay;
    public float LenghtOfDay => lenghtOfDay;
    public bool IsNightTime => isNightTime;

    float remainingLenghtOfDay;

    private void Start()
    {
        nightTimeVolume.weight = 0;
        OnDayTime?.Invoke();
        remainingLenghtOfDay = lenghtOfDay - TimeOfDay;
        spawnSecondsPassedShield += (percentToSpawnShield / 100f) * remainingLenghtOfDay;
    }

    void SpawnShieldOverTime()
    {
        float tolerance = 0.1f;
        //Debug.Log($"{spawnSecondsPassed} and {TimeOfDay} = {Mathf.Abs(spawnSecondsPassed - TimeOfDay) < tolerance}");
        if (spawnReactivateDelayedShield == true && (Mathf.Abs(spawnSecondsPassedShield - TimeOfDay) < tolerance))
        {
            //Debug.Log("spawnShield is true");
            SCR_SceneManager.instance.spawnShield = true;
            spawnSecondsPassedShield += (percentToSpawnShield / 100f) * remainingLenghtOfDay;
            spawnReactivateDelayedShield = false;
            spawnReactivateTimerShield = 1f;
            return;
        }
        if (spawnReactivateTimerShield > 0f && spawnReactivateDelayedShield == false)
        {
            spawnReactivateTimerShield -= Time.deltaTime;
        }
        else if (spawnReactivateDelayedShield == false)
        {
            spawnReactivateDelayedShield = true;
        }
    }

    void SpawnRayOverTime()
    {
        float tolerance = 0.1f;
        if (spawnReactivateDelayedRay == true && (Mathf.Abs(spawnSecondsPassedRay - TimeOfDay) < tolerance))
        {
            //Debug.Log("spawnShield is true");
            SCR_SceneManager.instance.spawnRay = true;
            spawnSecondsPassedRay += (percentToSpawnRay / 100f) * remainingLenghtOfDay;
            spawnReactivateDelayedRay = false;
            spawnReactivateTimerRay = 1f;
            return;
        }
        if (spawnReactivateTimerRay > 0f && spawnReactivateDelayedRay == false)
        {
            spawnReactivateTimerRay -= Time.deltaTime;
        }
        else if (spawnReactivateDelayedRay == false)
        {
            spawnReactivateDelayedRay = true;
        }
    }

    public void UpdateDayNightCycle()
    {

        if (SceneManager.GetActiveScene().buildIndex == 0) return;

        if (!isEndOfGame)
        {
            TimeOfDay += Time.deltaTime;
            TimeOfDay %= lenghtOfDay;
            UpdateLightning(TimeOfDay / lenghtOfDay);
        }

        SpawnShieldOverTime();

        SpawnRayOverTime();
    }

    public void UpdateLightning(float timePercentage)
    {

        if (isEndOfGame) return;

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            nightTimeVolume.weight = Mathf.Lerp(0, 1, timePercentage);
        }


        if (TimeOfDay >= (lenghtOfDay - 1) && !isEndOfGame)
        {
            isEndOfGame = true;
            OnGameEnd?.Invoke();
        }

        if (TimeOfDay >= startNightTime && !isNightTime)
        {
            isNightTime = true;
            OnNightTime?.Invoke();
        }
    }
}
