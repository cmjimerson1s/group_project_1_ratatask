using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SCR_AccessRender : MonoBehaviour
{
    public SCR_Access_ColorChoice_SO colorChoice_SO;
    public GameObject player;
    public Image playerSprite;
    public Material highContrastMaterial;
    public Material transparentMaterial;
    public Material playerMaterial;
    public Material obstacleMaterial;
    public Material shieldMaterial;
    public Material rayMaterial;
    public Material nutMaterial;
    public Material trailMaterial;
    public Material enemyMaterial;
    public ParticleSystem treeGlow;
    public ParticleSystem leavesFall;


    void Start()
    {
        Renderer[] allRenderers = FindObjectsOfType<Renderer>();

        foreach (Renderer renderer in allRenderers) {
            if (renderer.gameObject == player)
            {
                renderer.material = playerMaterial;
            }
            else if (renderer.GetComponent<ParticleSystemRenderer>() != null)
            {
                renderer.material = transparentMaterial;
            }
            else
            {
                renderer.material = highContrastMaterial;
            }
        }
        playerMaterial.SetColor("_BaseColor", colorChoice_SO.playerColor);
        obstacleMaterial.SetColor("_BaseColor", colorChoice_SO.obstaclelColor);
        shieldMaterial.SetColor("_BaseColor", colorChoice_SO.powerUPColor);
        rayMaterial.SetColor("_BaseColor", colorChoice_SO.powerUPColor);
        nutMaterial.SetColor("_BaseColor", colorChoice_SO.acornColor);
        trailMaterial.SetColor("_BaseColor", colorChoice_SO.trailColor);
        enemyMaterial.SetColor("_BaseColor", colorChoice_SO.enemyColor);
        playerSprite.color = colorChoice_SO.playerColor;

    }

    void Update()
    {
        
    }
}
