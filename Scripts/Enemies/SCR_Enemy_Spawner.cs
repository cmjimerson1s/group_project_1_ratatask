using UnityEngine;

public class SCR_Enemy_Spawner : MonoBehaviour
{
    //[SerializeField] SCR_Object_Pooling poolScript;
    //[SerializeField] SCR_Random_Per_Seconds randomScript;
    //SCR_Player pS;
    //SCR_ChangeEnemyValues changeEnemyValues;

    //[Header("Spawning")]
    //[SerializeField] bool speedDependantSpawning;
    //[SerializeField] float spawnFromPlayerDistance;
    //Vector3 spawnPosition;

    //private void Awake()
    //{
    //    randomScript.onChanceHasLanded += ActivateEnemy;
    //}

    //private void Start()
    //{
    //    pS = SCR_SceneManager.instance.pS;
    //    changeEnemyValues = GetComponent<SCR_ChangeEnemyValues>();
    //}

    //void ChangeEnemyValues(SCR_Enemy enemy)
    //{
    //    enemy.lifeSpan = changeEnemyValues.lifeSpawn;
    //    enemy.speed = changeEnemyValues.speed;
    //    enemy.offset = changeEnemyValues.offset;
    //    enemy.horizontalAngle = changeEnemyValues.horizontalAngle;
    //    enemy.heightOffset = changeEnemyValues.heightOffset;
    //    enemy.verticalAngle = changeEnemyValues.verticalAngle;
    //    enemy.rollAngle = changeEnemyValues.rollAngle;
    //}

    //void ActivateEnemy()
    //{
    //    //Debug.Log("ActivateEnemy");
    //    GameObject enemy = poolScript.GetPooledObject();
    //    if (enemy != null && poolScript.spawning)
    //    {
    //        enemy.SetActive(true);
    //        SCR_Enemy enemyScr = enemy.GetComponent<SCR_Enemy>();
    //        float progressPosition;
    //        if (!speedDependantSpawning)
    //            progressPosition = pS.movementScript.progress + (spawnFromPlayerDistance / 1000);
    //        else
    //            progressPosition = pS.movementScript.progress + (((1.33f * pS.movementScript.defaultPlayerSpeed) + 3.33f) / 1000);

    //        if (progressPosition > 1) { progressPosition -= 1; }
    //        enemyScr.progress = progressPosition;
    //        enemy.transform.position = pS.playerPath.EvaluatePosition(progressPosition);
    //        enemyScr.playerPath = pS.playerPath;
    //        enemyScr.playerScr = pS.movementScript;
    //        enemyScr.ReEnableFunctions();
    //        ChangeEnemyValues(enemyScr);
    //    }
    //    else if (poolScript.spawning)
    //    {
    //        Debug.Log("All active");
    //    }
    //}
}
