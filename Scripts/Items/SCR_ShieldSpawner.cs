using UnityEngine;
using UnityEngine.Splines;

public class SCR_ShieldSpawner : MonoBehaviour
{
    SplineContainer playerPath;
    public GameObject shieldPrefab;
    public GameObject rayOfLight;
    public int numberOfShields;
    public int numberOfRays;
    
    void Start()
    {
        playerPath = SCR_SceneManager.instance.playerPath;
        SpawnShields();
        SpawnRays();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnShields() {
        for (int i = 0; i < numberOfShields; i++)
        {
            float t = Random.Range(0f, 1f);
            Vector3 position = playerPath.EvaluatePosition(t);
            Instantiate(shieldPrefab, position, Quaternion.identity);

        }
    }

    void SpawnRays() {
        for (int i = 0; i < numberOfRays; i++)
        {
            float t = Random.Range(0f, 1f);
            Vector3 position = playerPath.EvaluatePosition(t);
            Instantiate(rayOfLight, position, Quaternion.identity);

        }
    }
}
