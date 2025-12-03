using UnityEngine;

public class SCR_PlayerScores : MonoBehaviour
{
    [SerializeField] SCR_PlayingCanvas playingCanvas;

    [HideInInspector] public int collectableScore = 0;


    public void AddCollectableScore(int anAmount)
    {
        collectableScore += anAmount;
        playingCanvas.UpdateScoreUI(collectableScore);
    }


    public void RemoveColletablescore(int anAmount)
    {
        collectableScore -= anAmount;
        if (collectableScore < 0) collectableScore = 0;
        playingCanvas.UpdateScoreUI(collectableScore);
    }
}
