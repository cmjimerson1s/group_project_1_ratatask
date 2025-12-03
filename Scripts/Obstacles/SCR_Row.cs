using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UIElements;
using UnityEngine.Splines;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

public class SCR_Row : MonoBehaviour
{
    public int timesActivated;
    public int timesDeactivated;

    [Header("Prefabs")]
    public Quaternion rotation;
    SCR_Random_Per_Seconds perSeconds;
    [HideInInspector] public GameObject thisPrefab;
    [HideInInspector] public bool goingToSpawnShield = false;
    int chosenObject = 0;

    SCR_Player pS;

    public enum rowType
    {
        obstacle,
        nut,
        shield,
        ray,
        enemy
    }

    [Header("Obstacle")]
    public GameObject objPos;
    public rowType type;
    public bool endRow;
    public int side;
    float offset;
    public float progress = 0;

    GameObject obstacle;
    bool spawned = false;

    SplineContainer playerPath;
    Vector3 tangent;
    Vector3 position;

    void Start()
    {
        SetReferences();
        if (type != rowType.shield) { SpawnObstacle(); }
    }

    void SetReferences()
    {
        pS = SCR_SceneManager.instance.pS;
        playerPath = SCR_SceneManager.instance.playerPath;
        offset = SCR_SceneManager.instance.laneOffset;
        if (!endRow)
        {
            perSeconds = GetComponent<SCR_Random_Per_Seconds>();
            perSeconds.onChanceHasLanded += PerSecondUpdate;
        }
    }

    void PerSecondUpdate()
    {
        if (thisPrefab != null) thisPrefab.transform.rotation = rotation;
    }
    public void DeactivatePowerUp()
    {
        //Debug.Log("deactivating");
        timesDeactivated++;
        if (thisPrefab != null)
        {
            thisPrefab.SetActive(false);
            //Debug.Log("deactivated power up");
        }
        gameObject.SetActive(false);
    }
    public void ActivatePowerUp()
    {
        Debug.Log("activating");
        timesActivated++;
        if (!spawned && type == rowType.shield)
        {
            if (SCR_SceneManager.instance.obstacleManager.shieldPrefabs.Count > 0)
            {
                chosenObject = Random.Range(0, SCR_SceneManager.instance.obstacleManager.shieldPrefabs.Count);
            }
            objPos.transform.localPosition = new Vector3(side * offset, 0, 0);

            Debug.Log(SCR_SceneManager.instance.obstacleManager.shieldPrefabs[0]);
            thisPrefab = Instantiate(SCR_SceneManager.instance.obstacleManager.shieldPrefabs[chosenObject], objPos.transform.position, Quaternion.identity, this.gameObject.transform);
            thisPrefab.transform.rotation = Quaternion.Euler(0, thisPrefab.transform.rotation.eulerAngles.y, 0);
            SCR_SceneManager.instance.obstacleManager.obstacleTransforms.Add(thisPrefab.transform);
            spawned = true;
        }

        if (thisPrefab != null && type == rowType.shield)
        {
            thisPrefab.SetActive(true);
            SCR_SceneManager.instance.spawnShield = false;
            Debug.Log("activated shield");
        }

        if (!spawned && type == rowType.ray)
        {
            if (SCR_SceneManager.instance.obstacleManager.rayPrefabs.Count > 0)
            {
                chosenObject = Random.Range(0, SCR_SceneManager.instance.obstacleManager.rayPrefabs.Count);
            }
            objPos.transform.localPosition = new Vector3(side * offset, 0, 0);

            Debug.Log(SCR_SceneManager.instance.obstacleManager.rayPrefabs[0]);
            thisPrefab = Instantiate(SCR_SceneManager.instance.obstacleManager.rayPrefabs[chosenObject], objPos.transform.position, Quaternion.identity, this.gameObject.transform);
            thisPrefab.transform.rotation = Quaternion.Euler(0, thisPrefab.transform.rotation.eulerAngles.y, 0);
            SCR_SceneManager.instance.obstacleManager.obstacleTransforms.Add(thisPrefab.transform);
            spawned = true;
        }

        if (thisPrefab != null && type == rowType.ray)
        {
            thisPrefab.SetActive(true);
            SCR_SceneManager.instance.spawnRay = false;
            Debug.Log("activated ray");
        }

    }

    void SpawnObstacle()
    {
        switch (type)
        {
            case rowType.obstacle:
                //Debug.Log("obs");
                if (SCR_SceneManager.instance.obstacleManager.obstaclePrefabs.Count > 0)
                {
                    chosenObject = Random.Range(0, SCR_SceneManager.instance.obstacleManager.obstaclePrefabs.Count);
                }

                if (!endRow)
                {
                    objPos.transform.localPosition = new Vector3(side * offset, 0, 0);
                    thisPrefab = Instantiate(SCR_SceneManager.instance.obstacleManager.obstaclePrefabs[chosenObject], objPos.transform.position, Quaternion.identity, this.gameObject.transform);
                    //thisPrefab.transform.rotation = Quaternion.Euler(0, thisPrefab.transform.rotation.eulerAngles.y, 0);
                    //manager.obstacleTransforms.Add(thisPrefab.transform);
                }

                break;

            case rowType.nut:
                //Debug.Log("nut");
                if (SCR_SceneManager.instance.obstacleManager.nutPrefabs.Count > 0)
                {
                    chosenObject = Random.Range(0, SCR_SceneManager.instance.obstacleManager.nutPrefabs.Count);
                }
                objPos.transform.localPosition = new Vector3(side * offset, 0, 0);
                thisPrefab = Instantiate(SCR_SceneManager.instance.obstacleManager.nutPrefabs[chosenObject], objPos.transform.position, Quaternion.identity, this.gameObject.transform);
                thisPrefab.transform.rotation = Quaternion.Euler(0, thisPrefab.transform.rotation.eulerAngles.y, 0);
                SCR_SceneManager.instance.obstacleManager.obstacleTransforms.Add(thisPrefab.transform);

                break;

            case rowType.enemy:
                //Debug.Log("ene");
                if (SCR_SceneManager.instance.obstacleManager.enemyPrefabs.Count > 0)
                {
                    chosenObject = Random.Range(0, SCR_SceneManager.instance.obstacleManager.enemyPrefabs.Count);
                }
                thisPrefab = Instantiate(SCR_SceneManager.instance.obstacleManager.enemyPrefabs[chosenObject], transform.position, Quaternion.identity, this.gameObject.transform);
                SCR_SceneManager.instance.enemies.Add(thisPrefab.GetComponent<SCR_Enemy>());
                break;
        }
        spawned = true;
    }
}