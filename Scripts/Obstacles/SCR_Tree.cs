using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UIElements;

public class SCR_Tree : MonoBehaviour
{
    [SerializeField] CapsuleCollider treeCollider;
    [SerializeField] float reEnableTimer = 1;
    float currentTimer;
    public float pathProgress;

    private void Start()
    {
        currentTimer = reEnableTimer;

    }
    void Update()
    {
        // temp code to re enable box colliders after the player hits them

        if (treeCollider.enabled == false)
        {
            currentTimer -= Time.deltaTime;
            if (currentTimer < 0)
            {
                currentTimer = reEnableTimer;
                treeCollider.enabled = true;
            }
        }


    }
}
