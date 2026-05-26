using UnityEngine;

public class ZiggsQBomb : MonoBehaviour
{
    [Header("이동 설정")]
    [SerializeField] private Vector3 velocity;
    [SerializeField] private Vector3 gravity = new Vector3(0f, -25f, 0f);
    [SerializeField] private float damping = 0.75f;

    [Header("충돌 설정")]
    [SerializeField] private int maxBounce = 3;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask enemyLayer;

    [Header("폭발 설정")]
    [SerializeField] private float explosionRadius = 3f;

    [Header("띄우는 힘")]
    [SerializeField] private float maxUpForce = 10f;   // 중심에 가까울 때 위로 띄우는 최대 힘
    [SerializeField] private float sideForce = 4f;     // 바깥쪽으로 밀어내는 힘

    public GameObject explosionFx;

    private int bounceCount;
    private bool exploded;

    public void Init(Vector3 dir, float power, float upPower)
    {
        velocity = dir.normalized * power;
        velocity.y = upPower;
    }

    private void Update()
    {
        if (exploded)
            return;

        velocity += gravity * Time.deltaTime;
        transform.position += velocity * Time.deltaTime;

        if (velocity != Vector3.zero)
            transform.forward = velocity.normalized;
    }

    private void OnCollisionEnter(Collision col)
    {
        if (exploded)
            return;

        int colLayer = col.gameObject.layer;

        // Enemy에게 직접 맞으면 즉시 폭발
        if (IsInLayerMask(colLayer, enemyLayer))
        {
            Explode();
            return;
        }

        // 바닥에 닿았을 때
        if (IsInLayerMask(colLayer, groundLayer))
        {
            bounceCount++;

            // 3번째 바닥 충돌이면 폭발
            if (bounceCount >= maxBounce)
            {
                Explode();
                return;
            }

            Bounce(col);
        }
    }

    private void Bounce(Collision col)
    {
        Vector3 normal = col.contacts[0].normal.normalized;

        // Reflect 함수 사용하지 않고 직접 반사 공식 사용
        float dot = Vector3.Dot(velocity, normal);
        Vector3 reflected = velocity - 2f * dot * normal;

        velocity = reflected * damping;

        // 너무 약해져서 거의 멈추는 것 방지
        if (velocity.magnitude < 3f)
            velocity = velocity.normalized * 3f;
    }

    private void Explode()
    {
        exploded = true;

        Collider[] enemies = Physics.OverlapSphere(
            transform.position,
            explosionRadius,
            enemyLayer
        );

        foreach (Collider enemy in enemies)
        {
            // 데미지는 주지 않고 적을 띄우기만 함
            KnockUpEnemy(enemy);
        }

        // 폭발 이펙트가 있다면 여기에서 생성
        Instantiate(explosionFx, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    private void KnockUpEnemy(Collider enemy)
    {
        Rigidbody rb = enemy.attachedRigidbody;

        if (rb == null)
            return;

        Vector3 toTarget = rb.position - transform.position;
        float dist = toTarget.magnitude;

        // 중심에 가까울수록 1, 멀수록 0
        float rate = 1f - Mathf.Clamp01(dist / explosionRadius);

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

    private bool IsInLayerMask(int layer, LayerMask mask)
    {
        return (mask.value & (1 << layer)) != 0;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}