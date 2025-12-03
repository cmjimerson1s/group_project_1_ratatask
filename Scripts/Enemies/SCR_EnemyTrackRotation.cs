using UnityEngine;

public class SCR_EnemyTrackRotation : MonoBehaviour
{
    public Transform childObject;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 directionToChild = childObject.position - transform.position;
        transform.rotation = Quaternion.LookRotation(directionToChild, Vector3.up);

    }
}
