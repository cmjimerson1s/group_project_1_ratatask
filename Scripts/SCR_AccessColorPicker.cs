using UnityEngine;
using UnityEngine.UI;


public class SCR_AccessColorPicker : MonoBehaviour
{
    public SCR_Access_ColorChoice_SO colorChoice_SO;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void PlayerColor(Button button) {
        Image childImage = button.transform.Find("Color").GetComponent<Image>();
        colorChoice_SO.playerColor = childImage.color;
    }

    public void AcornColor(Button button) {
        Image childImage = button.transform.Find("Color").GetComponent<Image>();
        colorChoice_SO.acornColor = childImage.color;
    }

    public void ObstacleColor(Button button) {
        Image childImage = button.transform.Find("Color").GetComponent<Image>();
        colorChoice_SO.obstaclelColor = childImage.color;
    }

    public void PowerUpColor(Button button) {
        Image childImage = button.transform.Find("Color").GetComponent<Image>();
        colorChoice_SO.powerUPColor = childImage.color;
    }

    public void TrailColor(Button button) {
        Image childImage = button.transform.Find("Color").GetComponent<Image>();
        colorChoice_SO.trailColor = childImage.color;
    }

    public void EnemyColor(Button button) {
        Image childImage = button.transform.Find("Color").GetComponent<Image>();
        colorChoice_SO.enemyColor = childImage.color;
    }
}
