using System.Timers;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class SCR_Enemy : MonoBehaviour
{
    #region OldCode
    //public float progress;
    //public SCR_Player_Movement playerScr;
    //public SplineContainer playerPath;
    //Vector3 position;
    //Vector3 tangent;

    //[Header("Stats")]
    //public float lifeSpan;
    //float deathTimer;
    //public float speed;
    //public float offset;
    //public float horizontalAngle;
    //public float heightOffset;
    //public float verticalAngle;
    //public float rollAngle;
    //int startingSide = 0;

    //public void ReEnableFunctions()
    //{
    //    deathTimer = lifeSpan;
    //    SetStartingPosition();
    //}

    //void Update()
    //{
    //    Life();
    //    EnemyMovement();
    //}

    //void Life()
    //{
    //    deathTimer -= Time.deltaTime;
    //    if (deathTimer < 0)
    //    {
    //        deathTimer = lifeSpan;
    //        gameObject.SetActive(false);
    //    }
    //}

    //void SetStartingPosition()
    //{
    //    if (Random.Range(0, 2) == 0)
    //    {
    //        startingSide = 1;
    //    }
    //    else
    //    {
    //        startingSide = -1;
    //    }

    //    tangent = playerPath.EvaluateTangent(progress);
    //    Quaternion rotation = Quaternion.LookRotation(tangent);

    //    position = transform.position + (Vector3.Cross(tangent, Vector3.up).normalized * startingSide * offset);
    //    //position += new Vector3(0, heightOffset, 0);
    //    transform.position = position;

    //    rotation = Quaternion.Euler(new Vector3(verticalAngle, rotation.eulerAngles.y + (horizontalAngle * startingSide), rotation.eulerAngles.z + rollAngle));

    //    transform.rotation = rotation;
    //}

    //void EnemyMovement()
    //{
    //    transform.Translate(Vector3.forward * speed * Time.deltaTime);
    //}


    //public void Death()
    //{
    //    gameObject.SetActive(false);

    //    // Code for spawning the enemy nuts
    //}
    #endregion

    [Header("References")]
    public GameObject pathObj;
    public SplineContainer path;
    public GameObject enemyObject;

    [Header("Values")]
    public float speed; 
    [SerializeField, Range(0f, 1f)] public float progress;
    Vector3 position;
    Vector3 tangent;
    bool spawned = false;

    private void Start()
    {
        speed = SCR_ValueAdjustmentManager.instance.enemySpeed;
        float pathScale = SCR_ValueAdjustmentManager.instance.enemyPathScale;
        pathObj.transform.localScale = new Vector3(pathScale, pathScale, pathScale);
    }

    public void UpdateEnemy()
    {
        if (!spawned)
        {
            float ran = Random.Range(0, 100);
            float prog = ran / 100;
            progress = prog;
            spawned = true;
        }
        progress += speed * Time.deltaTime / path.CalculateLength();
        progress %= 1f;
        position = path.EvaluatePosition(progress);
        tangent = path.EvaluateTangent(progress);
        Quaternion rotation = Quaternion.LookRotation(tangent);

        enemyObject.transform.position = position;
        enemyObject.transform.rotation = rotation;
    }

    public void Death()
    {
        transform.parent.gameObject.SetActive(false);
    }
}

