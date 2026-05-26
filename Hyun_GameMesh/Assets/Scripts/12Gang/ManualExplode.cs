using UnityEngine;

public class ManualExplode : MonoBehaviour
{
    public float delay = 1.5f;
    public float radius = 5f;
    public float force = 300f;
    public float upwardsModifier = 1f;

    void Start()
    {
        Invoke("Explode", delay);
    }

    void Explode()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);

        foreach (var col in colliders)
        {
            Rigidbody rb = col.attachedRigidbody;
            if (rb == null) continue;

            // 폭발 중심에서 타겟까지의 방향과 거리 계산
            Vector3 toTarget = rb.position - explosionPos;
            float distance = toTarget.magnitude;
            Vector3 dir = toTarget.normalized;

            // 거리에 따른 폭발력 감쇄 (멀어질수록 힘이 약해짐)
            float attenuation = 1f - Mathf.Clamp01(distance / radius);

            // 위쪽 방향 보정 (물체가 위로 붕 뜨는 효과 추가)
            dir += Vector3.up * upwardsModifier;
            dir = dir.normalized;

            // 최종 폭발 충격량 계산 및 적용
            Vector3 impulse = dir * force * attenuation;
            rb.AddForce(impulse, ForceMode.Impulse);
        }

        Destroy(gameObject);
    }

    // 에디터에서 폭발 반경을 시각적으로 확인하기 위한 기즈모 코드
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}