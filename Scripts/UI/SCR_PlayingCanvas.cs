using TMPro;
using UnityEngine;

public class SCR_PlayingCanvas : MonoBehaviour
{
    [Header("Nut variables")]
    [SerializeField] TextMeshProUGUI nutText;

    
    void Start()
    {
        nutText.text = "";
    }

    
    public void UpdateScoreUI(int anAmount)
    {
        nutText.text = anAmount.ToString();
    }
}
