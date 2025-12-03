using System;
using UnityEngine;

public class SCR_PlayerTriggerObstacleAnim : MonoBehaviour
{
    [SerializeField] Transform followObj;
    
    private void OnTriggerStay(Collider other) 
    {
        if (other.gameObject.CompareTag("OBSTACLE")) 
        {
            //Debug.Log("hit");
            other.GetComponent<Animator>().SetTrigger("Hit2");
        } 
    }

    private void Update() {
        transform.SetPositionAndRotation(followObj.transform.position, followObj.transform.rotation);
    }
}
