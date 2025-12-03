using UnityEngine;
using UnityEngine.Splines;

public class SCR_EnemyDirectionAnchor : MonoBehaviour
{
    private float sampleCount = 100f;
    private float splineLocation;
    private SplineContainer playerPath;

    void Start()
    {
        playerPath = SCR_SceneManager.instance.playerPath;
        splineLocation = GetNearestPointOnSpline();
        Vector3 position = playerPath.EvaluatePosition(splineLocation);
        transform.position = position;


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    float GetNearestPointOnSpline() {
        Vector3 worldPosition = transform.position;
        float closestT = 0f;
        float closestDistance = float.MaxValue;

        for (int i = 0; i <= sampleCount; i++)
        {
            float t = i / (float)sampleCount;
            Vector3 splinePoint = playerPath.EvaluatePosition(t);
            float distance = Vector3.Distance(worldPosition, splinePoint);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestT = t;
            }
        }

        return closestT;
    }
}
