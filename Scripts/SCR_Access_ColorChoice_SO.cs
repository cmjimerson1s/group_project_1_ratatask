using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "SCR_Access_ColorChoice_SO", menuName = "Scriptable Objects/SCR_Access_ColorChoice_SO")]
public class SCR_Access_ColorChoice_SO : ScriptableObject
{
    public Color playerColor = Color.blue;
    public Color acornColor = Color.yellow;
    public Color obstaclelColor = Color.red;
    public Color powerUPColor = Color.green;
    public Color trailColor = Color.cyan;
    public Color enemyColor = Color.magenta;


}
