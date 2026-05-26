using UnityEngine;

public class ZiggsWBomb : MonoBehaviour
{
    [Header("폭발 시간")]
    [SerializeField] private float delay = 3f;

    [Header("폭발 범위")]
    [SerializeField] private float radius = 5f;

    [Header("띄우는 힘")]
    [SerializeField] private float maxUpForce = 14f;
    [SerializeField] private float sideForce = 4f;

    [Header("피격 대상")]
    [SerializeField] private LayerMask playerLayer;

    public GameObject explosionFx;
    private bool exploded;

    private void Start()
    {
        Invoke(nameof(Explode), delay);
    }

    private void Explode()
    {
        if (exploded)
            return;

        exploded = true;

        KnockUpPlayer();

        // 여기에 폭발 이펙트 생성 가능
        Instantiate(explosionFx, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    private void KnockUpPlayer()
    {
        Collider[] players = Physics.OverlapSphere(
            transform.position,
            radius,
            playerLayer
        );

        foreach (Collider player in players)
        {
            Rigidbody rb = player.attachedRigidbody;

            if (rb == null)
                continue;

            Vector3 toTarget = rb.position - transform.position;
            float dist = toTarget.magnitude;

            // 중심에 가까울수록 1, 멀수록 0
            float rate = 1f - Mathf.Clamp01(dist / radius);

            Vector3 horizontalDir = toTarget;
            horizontalDir.y = 0f;

            if (horizontalDir.sqrMagnitude > 0.01f)
                horizontalDir.Normalize();
            else
                horizontalDir = transform.forward;

            float upPower = maxUpForce * rate;
            float sidePower = sideForce * rate;

            Vector3 impulse = Vector3.up * upPower + horizontalDir * sidePower;

            rb.AddForce(impulse, ForceMode.Impulse);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}