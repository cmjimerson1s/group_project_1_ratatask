using UnityEngine;
using UnityEngine.Splines;
using System.Collections.Generic;
using TMPro.Examples;


public class SCR_TreeSpawner : MonoBehaviour
{
    SCR_Object_Pooling pool;

    SCR_Player_Movement player;
    SplineContainer playerPath;
    public int treesInLane;
    public List<Vector3> treePositionsMainLane = new List<Vector3>();
    public List<Vector3> treePositionsLeftLane = new List<Vector3>();
    public List<Vector3> treePositionsRightLane = new List<Vector3>();
    public Dictionary<Vector3, float> treePositionsMainLaneDic = new Dictionary<Vector3, float>();
    private Vector3 tangent;
    private Vector3 laneOffSetDirection;

    public GameObject treePrefab;
    bool spawnedTrees = false;
    void Start()
    {
        pool = GetComponent<SCR_Object_Pooling>();
        player = SCR_SceneManager.instance.pS.movementScript;
        playerPath = SCR_SceneManager.instance.playerPath;
    }

    // Update is called once per frame
    void Update()
    {
        if (!spawnedTrees)
        {
            MainLaneTreePopulate();
            LeftLaneTreePopulate();
            RightLaneTreePopulate();
            spawnedTrees = true;
        }
        MainLaneShuffle();

    }

    void MainLaneTreePopulate()
    {
        treePositionsMainLane.Clear();
        for (int i = 0; i < treesInLane; i++)
        {
            float t = Random.Range(0f, 1f);
            Vector3 position = playerPath.EvaluatePosition(t);
            treePositionsMainLane.Add(position);
            treePositionsMainLaneDic.Add(position, t);

        }
        foreach (KeyValuePair<Vector3, float> tree in treePositionsMainLaneDic)
        {
            //Instantiate(treePrefab, position, Quaternion.identity);
            Vector3 pos = tree.Key;
            float pathProgress = tree.Value;
            GameObject currentTree = pool.GetPooledObject();
            if (currentTree != null)
            {
                currentTree.SetActive(true);
                currentTree.transform.position = pos;
                SCR_Tree treeScript = currentTree.GetComponent<SCR_Tree>();
                if (treeScript != null)
                {
                    treeScript.pathProgress = pathProgress; // Set the pathProgress value
                }
            }
        }

    }

    void LeftLaneTreePopulate()
    {
        treePositionsLeftLane.Clear();
        for (int i = 0; i < treesInLane; i++)
        {
            float t = Random.Range(0f, 1f);
            Vector3 position = playerPath.EvaluatePosition(t);
            tangent = playerPath.EvaluateTangent(t);
            laneOffSetDirection = Vector3.Cross(tangent, Vector3.up).normalized;
            //position += laneOffSetDirection * player.laneOffset;                           uncomment
            treePositionsLeftLane.Add(position);
        }
        foreach (Vector3 position in treePositionsLeftLane)
        {
            //Instantiate(treePrefab, position, Quaternion.identity);
            GameObject currentTree = pool.GetPooledObject();
            if (currentTree != null)
            {
                currentTree.SetActive(true);
                currentTree.transform.position = position;
            }
        }
    }
    void RightLaneTreePopulate()
    {
        treePositionsRightLane.Clear();
        for (int i = 0; i < treesInLane; i++)
        {
            float t = Random.Range(0f, 1f);
            Vector3 position = playerPath.EvaluatePosition(t);
            tangent = playerPath.EvaluateTangent(t);
            laneOffSetDirection = Vector3.Cross(tangent, Vector3.up).normalized;
            //position += laneOffSetDirection * (player.laneOffset * -1);                   uncomment
            treePositionsRightLane.Add(position);
        }
        foreach (Vector3 position in treePositionsRightLane)
        {
            //Instantiate(treePrefab, position, Quaternion.identity);
            GameObject currentTree = pool.GetPooledObject();
            if (currentTree != null)
            {
                currentTree.SetActive(true);
                currentTree.transform.position = position;
            }
        }
    }

    void MainLaneShuffle()
    {
        if (player.progress > 0.25f)
        {
            foreach (KeyValuePair<Vector3, float> tree in treePositionsMainLaneDic)
            {
                float pathProgress = tree.Value;
                if (pathProgress >= 0.0f && pathProgress <= 0.25f)
                {
                    // Debug.Log("Tree");
                }

            }

        }
    }
}
