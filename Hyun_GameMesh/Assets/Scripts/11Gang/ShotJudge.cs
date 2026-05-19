using UnityEngine;

public class ShotJudge : MonoBehaviour
{
    public BilliardScoreManager scoreManager;

    private BilliardTurnManager.Turn currentTurn;

    private bool hitRed1;
    private bool hitRed2;
    private bool hitOpponentBall;

    private bool judging;
    private bool success;

    private void Awake()
    {
        if (scoreManager == null)
            scoreManager = GetComponent<BilliardScoreManager>();
    }

    public void BeginShot(BilliardTurnManager.Turn turn)
    {
        currentTurn = turn;

        hitRed1 = false;
        hitRed2 = false;
        hitOpponentBall = false;
        success = false;

        judging = true;

        Debug.Log("샷 판정 시작: " + currentTurn);
    }

    public void EndShot()
    {
        if (!judging)
        {
            Debug.LogWarning("판정 중이 아니므로 EndShot 무시");
            return;
        }

        judging = false;

        Debug.Log(
            "샷 종료 / Red1: " + hitRed1 +
            " / Red2: " + hitRed2 +
            " / 상대 수구: " + hitOpponentBall
        );

        if (scoreManager == null)
        {
            Debug.LogError("ScoreManager가 연결되지 않았습니다.");
            success = false;
            return;
        }

        if (hitOpponentBall)
        {
            scoreManager.AddScore(currentTurn, -1);
            success = false;
            Debug.Log("상대 수구 맞힘: -1점");
            return;
        }

        if (hitRed1 && hitRed2)
        {
            scoreManager.AddScore(currentTurn, 1);
            success = true;
            Debug.Log("빨간 공 2개 맞힘: +1점");
        }
        else
        {
            success = false;
            Debug.Log("득점 실패");
        }
    }

    public void OnCueBallHit(BallType hitBallType)
    {
        Debug.Log("ShotJudge로 전달됨: " + hitBallType);

        if (!judging)
        {
            Debug.Log("판정 중이 아니라 무시됨: " + hitBallType);
            return;
        }

        if (hitBallType == BallType.Red1)
        {
            hitRed1 = true;
            Debug.Log("Red1 맞힘");
        }
        else if (hitBallType == BallType.Red2)
        {
            hitRed2 = true;
            Debug.Log("Red2 맞힘");
        }
        else if (currentTurn == BilliardTurnManager.Turn.Player1 && hitBallType == BallType.Player2)
        {
            hitOpponentBall = true;
            Debug.Log("Player2 공 맞힘");
        }
        else if (currentTurn == BilliardTurnManager.Turn.Player2 && hitBallType == BallType.Player1)
        {
            hitOpponentBall = true;
            Debug.Log("Player1 공 맞힘");
        }
    }

    public bool IsSuccess()
    {
        return success;
    }
}