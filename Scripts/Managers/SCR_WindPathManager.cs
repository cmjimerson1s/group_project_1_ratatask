using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Splines;

public class SCR_WindPathManager : MonoBehaviour
{
    [Header("Object")]
    public SCR_Object_Pooling pool;

    [Header("Values")]
    public float spacing;
    public List<float> offsets = new List<float>();
    SplineContainer playerPath;
    [HideInInspector] public float progress = 0;
    int maxItterations = 10000;
    public GameObject windLane;
    public GameObject visibleLanePrefab;
    private bool laneVisible;

    void Start()
    {
        playerPath = SCR_SceneManager.instance.playerPath;
        //for (int i = 0; i < 1; i++)
        //{
        //    maxItterations--;
        //    if (maxItterations <= 0) break;
        //    if (progress >= 1)
        //    {
        //        break;
        //    }
        //    SpawnWind();
        //    i--;
        //}
    }

    void SetWindPosition(GameObject obj)
    {
        progress += (spacing / 10000);

        if (progress >= 1) progress -= 1;

        Vector3 position = playerPath.EvaluatePosition(progress);

        obj.transform.position = position;

        Vector3 tangent = playerPath.EvaluateTangent(progress);
        Quaternion rotation = Quaternion.LookRotation(tangent);

        obj.transform.rotation = rotation;
    }

    public void SpawnWind()
    {
        GameObject windObj = pool.GetPooledObject();
        if (windObj != null)
        {
            windObj.SetActive(true);
            SetWindPosition(windObj);
        }
    }

    public void VisibleLane() {
        laneVisible = !laneVisible;
        if (laneVisible) {
            pool.prefab = visibleLanePrefab;
        } else { 
            pool.prefab = windLane;
        }

    }
}
