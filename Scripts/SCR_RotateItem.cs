using UnityEngine;

public class SCR_RotateItem : MonoBehaviour
{

    [SerializeField] private float degreesPerSecond = 15.0f;
    [SerializeField] private float amplitude = 0.5f;
    [SerializeField] private float frequency = 1f;
    int dir;

    private void Start()
    {
        dir = Random.Range(0, 2);
        if (dir == 0) { dir = -1; } else { dir = 1; }
    }

    void Update()
    {
        Vector3 posOffset = new Vector3(0, 0, 0);
        Vector3 tempPos;

        transform.Rotate(new Vector3(0f, (Time.deltaTime * degreesPerSecond) * dir, 0f), Space.World);

        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

        transform.localPosition = tempPos;
    }
}
