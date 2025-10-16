using UnityEngine;

public class TennisScoreManager : MonoBehaviour
{
    public int playerPoints = 0;
    public int opponentPoints = 0;
    public int playerGames = 0;
    public int opponentGames = 0;
    public int playerSets = 0;
    public int opponentSets = 0;

    public void AddPoint(string whoScored)
    {
        if (whoScored == "Player")
            playerPoints++;
        else
            opponentPoints++;

        CheckGameProgress();
    }

    void CheckGameProgress()
    {
        if (playerPoints >= 4 && playerPoints - opponentPoints >= 2)
        {
            playerGames++;
            ResetPoints();
            Debug.Log("🎾 Game for Player! " + playerGames);
        }
        else if (opponentPoints >= 4 && opponentPoints - playerPoints >= 2)
        {
            opponentGames++;
            ResetPoints();
            Debug.Log("🎾 Game for Opponent! " + opponentGames);
        }

        // Podés agregar lógica de sets acá también
    }

    void ResetPoints()
    {
        playerPoints = 0;
        opponentPoints = 0;
    }
}
