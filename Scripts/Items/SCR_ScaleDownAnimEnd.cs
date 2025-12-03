using UnityEngine;

public class SCR_ScaleDownAnimEnd : MonoBehaviour
{
    public void OnAnimStart()
    {
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
    }
    public void OnAnimEnd()
    {
        transform.localScale = Vector3.one;
        gameObject.GetComponent<CapsuleCollider>().enabled = true;
        transform.parent.gameObject.SetActive(false);
    }
    public void ResetObstacle()
    {
        gameObject.GetComponent<CapsuleCollider>().enabled = true;
    }
}
