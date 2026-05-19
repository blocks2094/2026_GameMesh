using UnityEngine;

public enum BallType
{
    Player1,
    Player2,
    Red1,
    Red2
}

public class BilliardBall : MonoBehaviour
{
    public BallType ballType;

    private BilliardTurnManager turnManager;
    private ShotJudge shotJudge;

    private void Awake()
    {
        turnManager = FindFirstObjectByType<BilliardTurnManager>();
        shotJudge = FindFirstObjectByType<ShotJudge>();

    }

    private void OnCollisionEnter(Collision collision)
    {

        if (turnManager == null)
        {
            return;
        }

        if (shotJudge == null)
        {
            return;
        }

        if (!turnManager.IsCurrentCueBall(ballType))
            return;

        BilliardBall otherBall = collision.collider.GetComponentInParent<BilliardBall>();

        if (otherBall == null)
        {
            return;
        }


        shotJudge.OnCueBallHit(otherBall.ballType);
    }
}