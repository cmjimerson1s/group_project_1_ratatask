using UnityEngine;
using UnityEngine.Splines;

public class SCR_TEST_Enemey : MonoBehaviour
{
    public SplineContainer enemyTrack;
    [SerializeField, Range(0f, 1f)] public float progress = 0f;
    public float enemySpeed;
    public Vector3 position;
    public Vector3 tangent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        progress += enemySpeed * Time.deltaTime / enemyTrack.CalculateLength();
        progress %= 1f;
        position = enemyTrack.EvaluatePosition(progress);
        tangent = enemyTrack.EvaluateTangent(progress);
        Quaternion rotation = Quaternion.LookRotation(tangent);
        transform.position = position;
        transform.rotation = rotation;
    }
}
