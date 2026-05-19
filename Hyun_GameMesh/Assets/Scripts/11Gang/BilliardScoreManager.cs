using TMPro;
using UnityEngine;

public class BilliardScoreManager : MonoBehaviour
{
    public int player1Score;
    public int player2Score;

    public TMP_Text player1ScoreText;
    public TMP_Text player2ScoreText;

    private void Start()
    {
        RefreshUI();
    }

    public void AddScore(BilliardTurnManager.Turn turn, int amount)
    {
        if (turn == BilliardTurnManager.Turn.Player1)
        {
            player1Score += amount;

            if (player1Score < 0)
                player1Score = 0;
        }
        else
        {
            player2Score += amount;

            if (player2Score < 0)
                player2Score = 0;
        }

        RefreshUI();
    }

    private void RefreshUI()
    {
        player1ScoreText.text = "Player1 : " + player1Score;
        player2ScoreText.text = "Player2 : " + player2Score;
    }
}