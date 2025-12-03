using UnityEngine;

public class SCR_EnemyDropMovement : MonoBehaviour
{
    public GameObject player;
    public int magnetSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerLocation = player.transform.position;
        transform.position = Vector3.MoveTowards(transform.position, playerLocation, magnetSpeed * Time.deltaTime);
    }
}
