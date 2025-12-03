using UnityEngine;
using UnityEngine.Splines;

public class SCR_EnemySpawnerSimple : MonoBehaviour
{
    SplineContainer playerPath;
    public GameObject enemyPrefab;
    public int numberOfEnemies;

    void Start()
    {
        playerPath = SCR_SceneManager.instance.playerPath;
        EnemySpawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EnemySpawn() {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            float t = Random.Range(0f, 1f);
            Vector3 position = playerPath.EvaluatePosition(t);
            Instantiate(enemyPrefab, position, Quaternion.identity);

        }
    }
}
