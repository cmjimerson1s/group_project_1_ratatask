using UnityEngine;

public class SCR_ValueAdjustmentManager : MonoBehaviour
{

    public static SCR_ValueAdjustmentManager instance; 
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public enum diffSets
    {
        veryEasy,
        easy,
        normal,
        hard,
        extreme
    }

    //[Header("General Values")]
    public diffSets Difficulty = diffSets.normal;
    //public float dayLength;

    [Header("Lane Values")]
    public float laneOffset = 3.5f;
    public float spacing = 40;

    [Header("Player Values")]
    public float defaultPlayerSpeed = 50;
    public float laneSwitchingSpeed = 6;
    public float bumpKnockback = 30;
    public float accelerationSpeed = 100;

    [Header("Enemy Values")]
    public float enemySpeed = 6;
    public float enemyPathScale = 4;

    [Header("Power Ups")]
    public float shieldDuration;
    public float shieldSpeedBoost;
    public float rayDuration;
    public float rayRange;

}
