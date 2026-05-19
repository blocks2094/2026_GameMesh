using UnityEngine;

public class CueBallHitDetector : MonoBehaviour
{
    public BilliardTurnManager turnManager;
    public ShotJudge shotJudge;

    private BilliardBall myBall;

    private void Awake()
    {
        myBall = GetComponent<BilliardBall>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (myBall == null) return;

        if (!turnManager.IsCurrentCueBall(myBall.ballType))
            return;

        BilliardBall otherBall = collision.collider.GetComponent<BilliardBall>();

        if (otherBall == null)
            return;

        shotJudge.OnCueBallHit(otherBall.ballType);
    }
}