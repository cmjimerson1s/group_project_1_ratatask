using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SCR_WorldGenerationManager : MonoBehaviour
{
    [Header("Spawning variables")]
    [SerializeField] private SplineContainer splinePath;
    [Tooltip("Use like a precentage of the map, Example, 0.8 whould spawn everything 80% ahead of the player")]
    [SerializeField, Range(0f, 1f)] private float spawnAheadOffset;
    [Tooltip("Trial and error too find the right length")]
    [SerializeField, Range(0f, 1f)] private float distBetweenCollectablesInLine;

    [Header("Object pools")]
    [SerializeField] private SCR_Object_Pooling nutPool;

    [Header("Randomizing variables")]
    [SerializeField] private int randomShouldBeLine = 20;
    [SerializeField] private int minLaneLength;
    [SerializeField] private int maxLaneLength;


    void Start()
    {

    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnNut();
        }
    }


    private void SpawnNut() 
    {
        int random = Random.Range(0, 99);
        random = 1;

        if (random > randomShouldBeLine) // Spawn single nut
        {
            GameObject nut = nutPool.GetPooledObject();

            // nut.transform.position = splinePath.EvaluatePosition(SCR_GameManager.instance.playerScript.progress * spawnAheadOffset);
            nut.SetActive(true);
        }
        else // Spawn nut lane
        {
            int randomLaneLength = Random.Range(minLaneLength, maxLaneLength);
            randomLaneLength++;

            float nutDistance = 0;
            float laneOffset = GetLaneOffset();
            for (int i = 1; i <= randomLaneLength; i++)
            {
                GameObject nut = nutPool.GetPooledObject();
                
                nut.transform.position = GetSpawnPosition(nutDistance, laneOffset);
                nut.SetActive(true);

                nutDistance += distBetweenCollectablesInLine;
            }
        }
    }

    /*
        -- Prosess --

        Change GetSpawnPosition() to take in a progress/pathPosition and spawn them there

        Before spawning, get the laneOffset and the pathPosition for the spawn

        Check if there is a tree in the same lane that has a pathPosition that is less than somevalue where the line of collectables wont reach the tree

        If there is a tree change the laneOffset and try again, and if that also dosent work change to the last lane and try again

        -- Pseudocode -- 

        float spawnPathProgress = Get the random float depending on which quadrant the player is in;
        float laneOffset = GetLaneOffset();

        foreach(trees in that quadrant)
        {
            if(trees pathPosition - spawnPathProgress < Serialized float)
            {
                Get new laneOffset
            }
        }

        for(random amount of nuts that should spawn)
        {
            spawn on GetSpawnPosition();
        }
    */


    private Vector3 GetSpawnPosition(float distBetweenItems, float laneOffset) 
    {
        if(laneOffset < 0)
        {
            distBetweenItems *= 1.2f;
        }
        else if(laneOffset > 0)
        {
            distBetweenItems *= 0.7f;
        }

        // float spawnProgress = SCR_GameManager.instance.playerScript.progress + spawnAheadOffset + distBetweenItems;

        // if (spawnProgress > 1)
        // {
        //     spawnProgress -= 1;
        // }

        // Vector3 playerPos = splinePath.EvaluatePosition(spawnProgress);
        // Vector3 laneOffsetDirection = Vector3.Cross(splinePath.EvaluateTangent(spawnProgress), Vector3.up).normalized;

        // if(laneOffset != 0)
        // {
        //     playerPos += laneOffsetDirection * laneOffset;
        // }

        return default;
    }


    private float GetLaneOffset()
    {
        float offset = 0;

        int random = Random.Range(0, 99);

        if(random <= 33)
        {
            offset = -2f;
        }
        else if(random > 33 && random <= 66)
        {
            offset = 0;
        }
        else if(random > 66)
        {
            offset = 2;
        }

        return offset;
    }
}
