using UnityEngine;
using UnityEngine.Splines;

public class SCR_Player : MonoBehaviour
{
    [Header("Player References")]
    public GameObject model;
    [HideInInspector] public Animator animator;
    [HideInInspector] public SCR_Player_Movement movementScript;
    [HideInInspector] public SCR_Player_Bumping bumpingScript;
    [HideInInspector] public SCR_PlayerShield shieldScript;
    [HideInInspector] public SCR_PlayerHealth playerHealth;
    [HideInInspector] public BoxCollider boxCollider;
    [HideInInspector] public Rigidbody rigidBody;

    [Header("Manager References")]
    [HideInInspector] public SplineContainer playerPath;
    [HideInInspector] public string obstacleTag;
    [HideInInspector] public string enemyTag;

    [Header("Values")]
    [HideInInspector] public float laneOffset;

    void Start()
    {
        model.GetComponent<SCR_Player_AnimationEvents>().pS = this;
        animator = model.GetComponent<Animator>();
        movementScript = GetComponent<SCR_Player_Movement>();
        bumpingScript = GetComponent<SCR_Player_Bumping>();
        shieldScript = GetComponent<SCR_PlayerShield>();
        playerHealth = GetComponent<SCR_PlayerHealth>();
        boxCollider = GetComponent<BoxCollider>();
        rigidBody = GetComponent<Rigidbody>();
        playerPath = SCR_SceneManager.instance.playerPath;
        obstacleTag = SCR_SceneManager.instance.obstacleTag;
        enemyTag = SCR_SceneManager.instance.enemyTag;
        laneOffset = SCR_SceneManager.instance.laneOffset;
    }

    void Update()
    {

    }
}
