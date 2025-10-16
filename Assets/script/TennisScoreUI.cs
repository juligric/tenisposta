using TMPro;
using UnityEngine;

public class TennisScoreUI : MonoBehaviour
{
    public TMP_Text scoreText;
    public TennisScoreManager scoreManager;

    void Update()
    {
        scoreText.text = $"{scoreManager.playerGames}-{scoreManager.opponentGames}\n" +
                         $"{scoreManager.playerPoints}-{scoreManager.opponentPoints}";
    }
}
