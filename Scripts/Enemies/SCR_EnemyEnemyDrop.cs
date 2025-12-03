using UnityEngine;

public class SCR_EnemyEnemyDrop : MonoBehaviour
{
    public GameObject player;
    public GameObject spawningNuts;
    public int nutSpawnAmount;
    public float spawnDistance;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            SpawnNuts();
        }
    }


    public void SpawnNuts() {
        for (int i = 0; i < nutSpawnAmount; i++) {
            Vector3 instantiatePosition = player.transform.position + transform.forward * spawnDistance;
            Instantiate(spawningNuts, instantiatePosition, Quaternion.identity);
        }
    }

}
