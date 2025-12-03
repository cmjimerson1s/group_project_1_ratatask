using UnityEngine;

public class SCR_DespawnWall : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("WIND"))
        {
            other.gameObject.SetActive(false);
        }
        if (other.CompareTag("ROW"))
        {
            SCR_Row scr = other.GetComponent<SCR_Row>();
            switch (scr.type)
            {
                case SCR_Row.rowType.shield:
                    scr.DeactivatePowerUp();
                    break;
                case SCR_Row.rowType.ray:
                    scr.DeactivatePowerUp();
                    break;
                default:
                    other.gameObject.SetActive(false);
                    break;
            }
        }
    }
}
