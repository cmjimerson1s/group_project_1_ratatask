using System;
using UnityEngine;

public class SCR_PlayerRayLight : SCR_PowerUp
{
    private string ENEMYTAG;
    private string OBSTACLETAG;

    [Header("Collider vairables")]
    [SerializeField] Transform followObj;
    private BoxCollider boxCollider;
    [Tooltip("Use to set the width of the box collider")]
    [SerializeField] float xSize;
    [Tooltip("Use to set the height of the box collider")]
    [SerializeField] float ySize;
    [Tooltip("Use to set the length of the box collider")]
    [SerializeField] float zSize;
    [Tooltip("Use to set the height offset for the box collider (Should probably be kept at 0.75 unless changes occur)")]
    [SerializeField] float yOffset = 0.75f;


    public Action OnEnemyHit;
    public Action OnTreeHit;

    void Start()
    {
        ENEMYTAG = SCR_SceneManager.instance.enemyTag;
        OBSTACLETAG = SCR_SceneManager.instance.obstacleTag;

        boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3(xSize, ySize, zSize);
        transform.position = followObj.transform.position;
        boxCollider.center = new Vector3(0, 0 + yOffset, 0 + zSize / 2);
        boxCollider.isTrigger = true;

        StartPowerUp();
    }

    public override void StartPowerUp()
    {
        base.StartPowerUp();
        gameObject.SetActive(true);

        transform.position = followObj.transform.position;
    }

    protected override void EndPowerUp()
    {
        base.EndPowerUp();
        SCR_SceneManager.instance.pS.animator.SetBool("RayOn", false);
        gameObject.SetActive(false);
    }


    public override void UpdatePowerUp()
    {
        if(powerUpTimer > 0)
        {
            powerUpTimer -= Time.deltaTime;

            transform.SetPositionAndRotation(followObj.position, followObj.rotation);

            base.UpdatePowerUp();
            if(powerUpTimer <= 0) EndPowerUp();
        }
    }


    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag(ENEMYTAG))
        {
            collider.GetComponentInParent<SCR_Enemy>().Death();
            GameObject vfx = SCR_SceneManager.instance.enemyDeathVfxPool.GetPooledObject();
            vfx.SetActive(true);
            vfx.transform.position = collider.transform.position;
            vfx.transform.rotation = collider.transform.parent.transform.rotation;
            OnEnemyHit?.Invoke();
        }

        if (collider.CompareTag(OBSTACLETAG))
        {
            collider.GetComponent<Animator>().SetTrigger("Hit");
            OnTreeHit?.Invoke();
        }
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z + zSize / 2), new Vector3(xSize, ySize, zSize));
    }
}
