using UnityEngine;

public class SCR_PowerUp : MonoBehaviour
{
    [Header("Powerup variables")]
    [SerializeField] protected float powerUpDuration;
    protected float powerUpTimer;

    [Header("Blinking variables")]
    [SerializeField] private bool shouldBlink;
    [SerializeField] private GameObject visualObj;
    [SerializeField] private ParticleSystem particleS;
    Renderer particleSRenderer;
    [Tooltip("Use as a percentage - Example: 0.6 whould start blinkning efter 60% off the timer has gone")]
    [SerializeField] float startBlinking = 0f;
    [SerializeField] float baseBlinkDuration;
    [Tooltip("Decides when it should start blinking faster")]
    [SerializeField] float baseStartBlinkingFaster;
    [Tooltip("Lowers [Base Start Blinking Faster] by the decided amount so that the blinks happen faster")]
    [SerializeField] float fasterBlinking = 0f;
    [Tooltip("An amount of how much faster the blinking will be")]
    [SerializeField] float fasterBlinkingAmount = 0f;
    private float blinkTimer;
    private float startBlinkingFaster;
    private float blinkDuration;


    private void Start()
    {
        particleSRenderer = particleS.GetComponent<Renderer>();
    }



    public virtual void StartPowerUp()
    {
        visualObj.SetActive(true);
        if (particleS != null)
        {
            particleS.gameObject.SetActive(true);
            particleS.GetComponent<Renderer>().enabled = true;
        }
        powerUpTimer = powerUpDuration;
        if(shouldBlink) ResetBlinking();
    }


    protected void ResetBlinking()
    {
        startBlinkingFaster = baseStartBlinkingFaster;
        blinkDuration = baseBlinkDuration;
    }


    public virtual void UpdatePowerUp()
    {
        if(shouldBlink) UpdateBlinking();
    }


    protected void UpdateBlinking()
    {
        if (powerUpTimer <= powerUpDuration * startBlinking)
        {
            if (blinkTimer <= 0)
            {
                visualObj.SetActive(true);
                if(particleS != null) particleS.GetComponent<Renderer>().enabled = true;

                

                blinkTimer += Time.deltaTime;
                if (blinkTimer >= 0) blinkTimer = blinkDuration / 2;
            }
            else
            {
                visualObj.SetActive(false);
                if (particleS != null) particleS.GetComponent<Renderer>().enabled = false;

                blinkTimer -= Time.deltaTime;
                if (blinkTimer <= 0) blinkTimer = -blinkDuration;
            }

            if (powerUpTimer <= powerUpDuration * startBlinkingFaster)
            {
                startBlinkingFaster -= fasterBlinking;
                blinkDuration -= fasterBlinkingAmount;
                if (blinkDuration <= 0)
                {
                    blinkDuration = 0.1f;
                }
            }
        }
    }


    protected virtual void EndPowerUp()
    {
        visualObj.SetActive(false);
        if (particleS != null)
        {
            particleS.gameObject.SetActive(false);
        }
    }
}
