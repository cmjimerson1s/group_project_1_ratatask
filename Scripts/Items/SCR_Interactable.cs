using Unity.VisualScripting;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("Interactable variables")]
    [SerializeField] private string COLLIDETAG = "";
    [SerializeField] private float colliderSize;
    SphereCollider sphereCollider;


    void Start()
    {
        sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.radius = colliderSize;
    }

    void Update()
    {

    }


    protected virtual void OnTriggerAction(Collider collider) // Don't use base if object shouldn't be deactivated
    {
        gameObject.transform.parent.gameObject.SetActive(false);
    }


    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag(COLLIDETAG))
        {
            OnTriggerAction(collider);
        }
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(transform.position, colliderSize);
    }
}
