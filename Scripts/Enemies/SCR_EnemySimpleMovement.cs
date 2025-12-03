using TMPro.Examples;
using UnityEngine;
using UnityEngine.Splines;

public class SCR_EnemySimpleMovement : MonoBehaviour
{
    SCR_Player_Movement player;
    SplineContainer playerPath;
    public float splinePosition;
    private float offset;
    public float speed = 2.0f;
    private float timeElapsed = 0;
    private const int sampleCount = 100;
    private Vector3 startPoint;
    private Vector3 endPoint;
    private Vector3 centerPoint;
    private bool isMovingForward = true;  

    void Start()
    {
        player = SCR_SceneManager.instance.pS.movementScript;
        playerPath = SCR_SceneManager.instance.playerPath;
        offset = SCR_SceneManager.instance.laneOffset;
        splinePosition = GetNearestSplinePosition(transform.position);
        centerPoint = playerPath.EvaluatePosition(splinePosition);
        Vector3 tangent = ((Vector3)playerPath.EvaluateTangent(splinePosition)).normalized;
        Vector3 offsetDirectionUp = Vector3.Cross(tangent, Vector3.up).normalized;
        startPoint = centerPoint;
        endPoint = centerPoint;
        startPoint += offsetDirectionUp * offset;
        endPoint -= offsetDirectionUp * offset;
    }

    // Update is called once per frame
    public void UpdateEnemy()
    {
        timeElapsed += Time.deltaTime / speed;

        if (timeElapsed > 1f)
        {
            timeElapsed = 0f;
            isMovingForward = !isMovingForward; 
        }

        Vector3 currentPos = isMovingForward ?
            Vector3.Lerp(startPoint, endPoint, timeElapsed) :
            Vector3.Lerp(endPoint, startPoint, timeElapsed);

        // Set the sphere's position
        transform.position = currentPos;
    }

    private float GetNearestSplinePosition(Vector3 worldPosition) {
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
