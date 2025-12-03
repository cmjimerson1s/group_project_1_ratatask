using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Splines;
using System;
using UnityEngine.Rendering;

/*
 *  This script manages the spawning of obstacles in the scene
 */

public class SCR_ObstacleManager : MonoBehaviour
{
    [Header("Testing")]
    [SerializeField] bool testingShield;

    [Header("ObstaclePools")]
    public SCR_Object_Pooling poolLeftObs;
    public SCR_Object_Pooling poolMidObs;
    public SCR_Object_Pooling poolRightObs;
    public SCR_Object_Pooling poolEndObs;

    [Header("NutPools")]
    public SCR_Object_Pooling poolLeftNut;
    public SCR_Object_Pooling poolMidNut;
    public SCR_Object_Pooling poolRightNut;

    [Header("ShieldPools")]
    public SCR_Object_Pooling poolLeftShield;
    public SCR_Object_Pooling poolMidShield;
    public SCR_Object_Pooling poolRightShield;

    [Header("RayPools")]
    public SCR_Object_Pooling poolLeftRay;
    public SCR_Object_Pooling poolMidRay;
    public SCR_Object_Pooling poolRightRay;

    [Header("EnemyPools")]
    public SCR_Object_Pooling poolEnemy;

    [Header("References")]
    public SCR_ObstacleSpawnTrigger trigger;
    [HideInInspector] public SCR_Random_Per_Seconds perSecondScript;
    SCR_Player pS;
    SplineContainer playerPath;

    [Header("Lists")]
    public List<SCR_Pattern> TutorialPatternsList = new List<SCR_Pattern>();
    public List<SCR_Pattern> PatternsList = new List<SCR_Pattern>();
    public List<SCR_Pattern> ShieldPatternsList = new List<SCR_Pattern>();
    public List<GameObject> obstaclePrefabs = new List<GameObject>();
    public List<GameObject> nutPrefabs = new List<GameObject>();
    public List<GameObject> shieldPrefabs = new List<GameObject>();
    public List<GameObject> rayPrefabs = new List<GameObject>();
    public List<GameObject> enemyPrefabs = new List<GameObject>();
    [HideInInspector] public List<Transform> obstacleTransforms = new List<Transform>();

    // Wind Stuff
    SCR_WindPathManager wind;

    [Header("Values")]
    public float spawnAheadOfPlayer;
    public float spacing;
    public float tutorialSpacing;
    public float shieldSpacing;
    public float shieldNutsSpawningHeight;
    public float progress = 0;
    bool spawnedTutorials = false;
    bool spawned = false;
    int pickedPattern;

    float shieldProgress;
    float aheadOfPlayerShieldPatternSpawn;

    SCR_Row latestEndRow = null;

    bool currentlySpawning;

    public bool currentlyWarping;
    bool currWindWarping;

    public bool spawnedShieldSet = false;


    private void Awake() // Sets references
    {
        perSecondScript = GetComponent<SCR_Random_Per_Seconds>();
        perSecondScript.onChanceHasLanded += PerSecondUpdate;
        wind = GetComponent<SCR_WindPathManager>();
    }

    void Start() // Sets references
    {
        pS = SCR_SceneManager.instance.pS;
        playerPath = SCR_SceneManager.instance.playerPath;
    }

    public void UpdateObstacleManger()
    {

        if (Input.GetKeyDown(KeyCode.Space) && testingShield) // Testing
        {
            SCR_SceneManager.instance.spawnShield = true;
        }

        /* If the game just stared and it hasnt spawned half the map, 
         * it does a loop of spawning all patterns
        */
        if (!spawned)
        {
            float normalSpacing = spacing;
            if (!spawnedTutorials)
            {
                foreach (SCR_Pattern tutorial in TutorialPatternsList)
                { // for each tutorial set in the list of tutorial patterns, spawn them
                    spacing = tutorialSpacing;
                    SpawnRows(tutorial);
                }
                spawnedTutorials = true;
            }
            for (int i = 0; i < 1; i++)
            { // This spawns patterns until the pattern's progress is at half the map
                if (progress < pS.movementScript.progress + (spawnAheadOfPlayer - .2))
                {
                    spacing = normalSpacing;
                    SpawnRows(PickRandomPattern(PatternsList));
                    i--;
                }
            }
            for (int i = 0; i < 1; i++)
            { // this spawns wind vfx the same way patterns spawn
                if (wind.progress < pS.movementScript.progress + (spawnAheadOfPlayer - .4))
                {
                    wind.SpawnWind();
                    i--;
                }
            }
            spawned = true;
        }

        if (pS.movementScript.shieldIsOn && !spawnedShieldSet || pS.movementScript.newShieldPicked)
        { // this gets triggered when you pick up a shield and it spawns a pattern for the shield power up
            pS.movementScript.newShieldPicked = false;
            SpawnShieldRows(PickRandomPattern(ShieldPatternsList));
            spawnedShieldSet = true;
        }

        /* This spawns a new pattern if the player progress + half the map progress is bigger than
        *  the pattern progress, making it so that the obstacle generation will always be at half the
        *  map ahead of the player
        */
        if (!currentlySpawning)
        {
            float spawnBoundary = pS.movementScript.progress + spawnAheadOfPlayer;

            if (spawnBoundary > 1)
                spawnBoundary -= 1;

            currentlyWarping = spawnBoundary < pS.movementScript.progress;

            if (!currentlyWarping)
            {
                if (progress > pS.movementScript.progress && progress < spawnBoundary)
                {
                    SpawnOneRow();
                }
            }
            else
            {
                ;
                if (progress > pS.movementScript.progress || progress < spawnBoundary)
                {
                    SpawnOneRow();
                }
            }
        }

        #region wind Stuff

        // This is the same thing as the obstacle spawning from before but for the wind vfx
        float windSpawnBoundary = pS.movementScript.progress + (spawnAheadOfPlayer - .4f);

        if (windSpawnBoundary > 1)
            windSpawnBoundary -= 1;

        currWindWarping = windSpawnBoundary < pS.movementScript.progress;

        if (!currWindWarping)
        {
            if (wind.progress > pS.movementScript.progress && wind.progress < windSpawnBoundary)
            {
                wind.SpawnWind();
            }
        }
        else
        {
            if (wind.progress > pS.movementScript.progress || wind.progress < windSpawnBoundary)
            {
                wind.SpawnWind();
            }
        }
        #endregion
    }

    // This triggers every second, its for resetting the rotation of the obstacles when reused from the pool
    void PerSecondUpdate()
    {
        foreach (Transform transform in obstacleTransforms)
        {
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        }
    }

    // Spawns only one row
    void SpawnOneRow()
    {
        SpawnRows(PickRandomPattern(PatternsList));
    }

    void SpawnTest() // This function was only for testing the spawning of the shield
    {
        GameObject shieldTest = poolMidShield.GetPooledObject();
        if (shieldTest != null)
        {
            shieldTest.SetActive(true);
            float testProg = pS.movementScript.progress;
            testProg += (spacing / 10000) * .5f;

            if (testProg >= 1) testProg -= 1;

            Vector3 position = playerPath.EvaluatePosition(testProg);

            shieldTest.transform.position = position;

            Vector3 tangent = playerPath.EvaluateTangent(testProg);
            Quaternion rotation = Quaternion.LookRotation(tangent);

            shieldTest.transform.rotation = rotation;
        }
    }

    // This function just returns a random pattern inside a list
    SCR_Pattern PickRandomPattern(List<SCR_Pattern> patternList)
    {
        int lastPattern = pickedPattern;
        pickedPattern = UnityEngine.Random.Range(0, patternList.Count);
        if (lastPattern == pickedPattern && patternList.Count > 1)
        {
            return PickRandomPattern(patternList);
        }
        return patternList[pickedPattern];
    }
    /* This is for setting the progress, position and rotation of each row when they are spawned, but
     * this function uses the script of the row, this is only used when we need to use "GetComponent"
     */
    void SetRowPosition(GameObject row, int pos, SCR_Row rowScr)
    {
        progress += (spacing / 10000) * pos;

        if (progress >= 1) progress -= 1;

        Vector3 position = playerPath.EvaluatePosition(progress);

        row.transform.position = position;

        Vector3 tangent = playerPath.EvaluateTangent(progress);
        Quaternion rotation = Quaternion.LookRotation(tangent);

        row.transform.rotation = rotation;

        rowScr.rotation = rotation;
    }

    // This is the same function as above but for when we dont use "GetComponent"
    void SetRowPosition(GameObject row, int pos)
    {
        progress += (spacing / 10000) * pos;

        if (progress >= 1) progress -= 1;

        Vector3 position = playerPath.EvaluatePosition(progress);

        row.transform.position = position;

        Vector3 tangent = playerPath.EvaluateTangent(progress);
        Quaternion rotation = Quaternion.LookRotation(tangent);

        row.transform.rotation = rotation;
    }

    // This is for spawning each row in a pattern
    void SpawnRows(SCR_Pattern pattern)
    {
        currentlySpawning = true;
        int pos = 0;

        foreach (SCR_Row row in pattern.rowList)
        {
            if (row.endRow)
            {
                GameObject poolRowEmpty = poolEndObs.GetPooledObject();
                if (poolRowEmpty != null)
                {
                    poolRowEmpty.SetActive(true);
                    if (spawned)
                    {
                        latestEndRow = poolRowEmpty.GetComponent<SCR_Row>();
                        latestEndRow.progress = progress;
                    }
                    SetRowPosition(poolRowEmpty, pattern.positionsList[pos]);
                }
                continue;
            }

            // This is for all the spawning of the left side
            void spawnLeftSide()
            {
                GameObject poolRowLeft = null;
                switch (row.type)
                {
                    case SCR_Row.rowType.obstacle:
                        poolRowLeft = poolLeftObs.GetPooledObject();
                        break;
                    case SCR_Row.rowType.nut:
                        poolRowLeft = poolLeftNut.GetPooledObject();
                        break;
                    case SCR_Row.rowType.shield:
                        poolRowLeft = poolLeftShield.GetPooledObject();
                        break;
                    case SCR_Row.rowType.ray:
                        poolRowLeft = poolLeftRay.GetPooledObject();
                        break;
                }
                if (poolRowLeft != null)
                {
                    poolRowLeft.SetActive(true);
                    SCR_Row rowScr = poolRowLeft.GetComponent<SCR_Row>();
                    SetRowPosition(poolRowLeft, pattern.positionsList[pos]);
                    if (row.type == SCR_Row.rowType.shield)
                    {
                        SCR_Row currScr = poolRowLeft.GetComponent<SCR_Row>();
                        if (SCR_SceneManager.instance.spawnShield)
                        {
                            currScr.ActivatePowerUp();
                        }
                        else
                        {
                            currScr.DeactivatePowerUp();
                        }
                    }
                    if (row.type == SCR_Row.rowType.ray)
                    {
                        SCR_Row currScr = poolRowLeft.GetComponent<SCR_Row>();
                        if (SCR_SceneManager.instance.spawnRay)
                        {
                            currScr.ActivatePowerUp();
                        }
                        else
                        {
                            currScr.DeactivatePowerUp();
                        }
                    }
                }
            }

            // This is for all the spawning of the middle
            void spawnMidSide()
            {
                GameObject poolRowMid = null;
                SCR_Row rowScript = null;
                switch (row.type)
                {
                    case SCR_Row.rowType.obstacle:
                        poolRowMid = poolMidObs.GetPooledObject();
                        break;
                    case SCR_Row.rowType.nut:
                        poolRowMid = poolMidNut.GetPooledObject();
                        break;
                    case SCR_Row.rowType.shield:
                        poolRowMid = poolMidShield.GetPooledObject();
                        break;
                    case SCR_Row.rowType.ray:
                        poolRowMid = poolMidRay.GetPooledObject();
                        break;
                    case SCR_Row.rowType.enemy:
                        poolRowMid = poolEnemy.GetPooledObject();
                        rowScript = poolRowMid.GetComponent<SCR_Row>();
                        break;
                }
                if (poolRowMid != null)
                {
                    poolRowMid.SetActive(true);
                    if (rowScript != null)
                    {
                        SetRowPosition(poolRowMid, pattern.positionsList[pos], rowScript);
                    }
                    else
                    {
                        SetRowPosition(poolRowMid, pattern.positionsList[pos]);
                    }
                    if (row.type == SCR_Row.rowType.shield)
                    {
                        SCR_Row currScr = poolRowMid.GetComponent<SCR_Row>();
                        if (SCR_SceneManager.instance.spawnShield)
                        {
                            currScr.ActivatePowerUp();
                        }
                        else
                        {
                            currScr.DeactivatePowerUp();
                        }
                    }
                    if (row.type == SCR_Row.rowType.ray)
                    {
                        SCR_Row currScr = poolRowMid.GetComponent<SCR_Row>();
                        if (SCR_SceneManager.instance.spawnRay)
                        {
                            currScr.ActivatePowerUp();
                        }
                        else
                        {
                            currScr.DeactivatePowerUp();
                        }
                    }
                }
            }

            // This is for all the spawning of the right side
            void spawnRightSide()
            {
                GameObject poolRowRight = null;
                switch (row.type)
                {
                    case SCR_Row.rowType.obstacle:
                        poolRowRight = poolRightObs.GetPooledObject();
                        break;
                    case SCR_Row.rowType.nut:
                        poolRowRight = poolRightNut.GetPooledObject();
                        break;
                    case SCR_Row.rowType.shield:
                        poolRowRight = poolRightShield.GetPooledObject();
                        break;
                    case SCR_Row.rowType.ray:
                        poolRowRight = poolRightRay.GetPooledObject();
                        break;
                }
                if (poolRowRight != null)
                {
                    poolRowRight.SetActive(true);
                    SCR_Row rowScr = poolRowRight.GetComponent<SCR_Row>();
                    SetRowPosition(poolRowRight, pattern.positionsList[pos]);
                    if (row.type == SCR_Row.rowType.shield)
                    {
                        SCR_Row currScr = poolRowRight.GetComponent<SCR_Row>();
                        if (SCR_SceneManager.instance.spawnShield)
                        {
                            currScr.ActivatePowerUp();
                        }
                        else
                        {
                            currScr.DeactivatePowerUp();
                        }
                    }
                    if (row.type == SCR_Row.rowType.ray)
                    {
                        SCR_Row currScr = poolRowRight.GetComponent<SCR_Row>();
                        if (SCR_SceneManager.instance.spawnRay)
                        {
                            currScr.ActivatePowerUp();
                        }
                        else
                        {
                            currScr.DeactivatePowerUp();
                        }
                    }
                }
            }

            // This actually spawns the row depending on which side they are and if they are mirrored
            if (!pattern.mirror)
            {
                switch (row.side)
                {
                    case -1:
                        spawnLeftSide();
                        break;

                    case 0:
                        spawnMidSide();
                        break;

                    case 1:
                        spawnRightSide();
                        break;
                }
            }
            else
            {
                switch (row.side)
                {
                    case 1:
                        spawnLeftSide();
                        break;

                    case 0:
                        spawnMidSide();
                        break;

                    case -1:
                        spawnRightSide();
                        break;
                }
            }

            pos++;
        }

        currentlySpawning = false;
    }

    // This is the same as above but is only for the shield power up
    void SetShieldRowPosition(GameObject row, int pos)
    {

        shieldProgress += (shieldSpacing / 10000) * pos;
        if (shieldProgress >= 1) shieldProgress -= 1;

        Vector3 position = playerPath.EvaluatePosition(shieldProgress);

        position += new Vector3(0, pS.movementScript.defaultShieldHeight, 0);

        row.transform.position = position;

        Vector3 tangent = playerPath.EvaluateTangent(shieldProgress);
        Quaternion rotation = Quaternion.LookRotation(tangent);

        row.transform.rotation = rotation;
    }

    // This is the same as above but is only for the shield power up
    void SpawnShieldRows(SCR_Pattern pattern)
    {
        shieldProgress = pS.movementScript.progress + aheadOfPlayerShieldPatternSpawn;

        int pos = 0;
        foreach (SCR_Row row in pattern.rowList)
        {
            if (row.endRow)
            {
                GameObject poolRowEmpty = poolEndObs.GetPooledObject();
                if (poolRowEmpty != null)
                {
                    poolRowEmpty.SetActive(true);
                    SetShieldRowPosition(poolRowEmpty, pattern.positionsList[pos]);
                }
                continue;
            }
            switch (row.side)
            {
                case -1:
                    GameObject poolRowLeft = null;
                    poolRowLeft = poolLeftNut.GetPooledObject();
                    if (poolRowLeft != null)
                    {
                        poolRowLeft.SetActive(true);
                        SetShieldRowPosition(poolRowLeft, pattern.positionsList[pos]);
                    }
                    break;

                case 0:
                    GameObject poolRowMid = null;
                    poolRowMid = poolMidNut.GetPooledObject();
                    if (poolRowMid != null)
                    {
                        poolRowMid.SetActive(true);
                        SetShieldRowPosition(poolRowMid, pattern.positionsList[pos]);
                    }
                    break;

                case 1:
                    GameObject poolRowRight = null;
                    poolRowRight = poolRightNut.GetPooledObject();
                    if (poolRowRight != null)
                    {
                        poolRowRight.SetActive(true);
                        SetShieldRowPosition(poolRowRight, pattern.positionsList[pos]);
                    }
                    break;
            }
            pos++;
        }
    }


}