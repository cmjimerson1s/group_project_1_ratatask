using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class SCR_Pattern : MonoBehaviour
{
    [Header("References")]
    [HideInInspector] public SCR_ObstacleManager manager;

    [Header("Pattern")]
    public bool tutorial = false;
    public bool mirror = false;
    public List<SCR_Row> rowList = new List<SCR_Row>();
    public List<int> positionsList = new List<int>();

    private void Start()
    {
        manager = transform.parent.GetComponent<SCR_ObstacleManager>();

        if (rowList.Count != positionsList.Count)
        {
            Debug.Log($"PatternError: The number of obstacles is not the same as the number of positions. (Go to the ObstacleParent object to fix this)");
        }
    }
}
