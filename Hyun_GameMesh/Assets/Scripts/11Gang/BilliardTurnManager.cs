using UnityEngine;

public class BilliardTurnManager : MonoBehaviour
{
    public enum Turn
    {
        Player1,
        Player2
    }

    [Header("Turn")]
    public Turn currentTurn = Turn.Player1;

    [Header("Player Balls")]
    public Rigidbody player1Ball;
    public Rigidbody player2Ball;

    [Header("All Balls")]
    public Rigidbody[] allBalls;

    [Header("Camera")]
    public CamerOrbit cam;

    [Header("Judge")]
    public ShotJudge shotJudge;

    [Header("Stop Check")]
    public float stopVelocity = 0.05f;
    public float stopAngularVelocity = 0.05f;
    public float checkDelay = 0.3f;

    private bool hasShot;
    private bool isWaitingStop;
    private float waitTimer;

    private void Start()
    {
        StartTurn();
    }

    private void Update()
    {
        if (!isWaitingStop) return;

        waitTimer += Time.deltaTime;

        if (waitTimer < checkDelay) return;

        if (IsAllBallsStopped())
        {
            FinishShot();
        }
    }

    public bool CanShoot(Rigidbody rb)
    {
        if (hasShot) return false;
        if (isWaitingStop) return false;

        if (currentTurn == Turn.Player1)
            return rb == player1Ball;

        if (currentTurn == Turn.Player2)
            return rb == player2Ball;

        return false;
    }

    public void Shot()
    {
        hasShot = true;
        isWaitingStop = true;
        waitTimer = 0f;

        shotJudge.BeginShot(currentTurn);
    }

    private void FinishShot()
    {
        shotJudge.EndShot();

        bool success = shotJudge.IsSuccess();

        if (success)
        {
            StartTurn();
        }
        else
        {
            NextTurn();
        }
    }

    private void StartTurn()
    {
        hasShot = false;
        isWaitingStop = false;
        waitTimer = 0f;

        if (currentTurn == Turn.Player1)
        {
            cam.SetTarget(player1Ball.transform);
            Debug.Log("Player1 턴 시작");
        }
        else
        {
            cam.SetTarget(player2Ball.transform);
            Debug.Log("Player2 턴 시작");
        }
    }

    private void NextTurn()
    {
        if (currentTurn == Turn.Player1)
            currentTurn = Turn.Player2;
        else
            currentTurn = Turn.Player1;

        StartTurn();
    }

    public bool IsCurrentCueBall(BallType ballType)
    {
        if (currentTurn == Turn.Player1)
            return ballType == BallType.Player1;

        if (currentTurn == Turn.Player2)
            return ballType == BallType.Player2;

        return false;
    }

    private bool IsAllBallsStopped()
    {
        foreach (Rigidbody rb in allBalls)
        {
            if (rb == null) continue;

            if (rb.linearVelocity.magnitude > stopVelocity)
                return false;

            if (rb.angularVelocity.magnitude > stopAngularVelocity)
                return false;
        }

        return true;
    }
}