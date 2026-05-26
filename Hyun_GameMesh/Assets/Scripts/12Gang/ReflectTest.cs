using UnityEngine;

public class ReflectTest : MonoBehaviour
{
    public Vector3 velocity = new Vector3(2f, -3f, 0f);
    public Vector3 gravity = new Vector3(0, -9.81f, 0);
    float damping = 0.9f; // 감쇄 계수

    void Update()
    {
        // 중력 적용 및 위치 이동
        velocity += gravity * Time.deltaTime;
        transform.position += velocity * Time.deltaTime;
    }

    void OnCollisionEnter(Collision col)
    {
        // 충돌한 면의 법선 벡터(Normal Vector) 구하기
        Vector3 normal = col.contacts[0].normal.normalized;

        // 입사 벡터(속도)와 법선 벡터의 내적 계산
        float dot = Vector3.Dot(velocity, normal);

        // 반사 벡터 계산 공식: R = I - 2 * (I · N) * N
        Vector3 reflect = velocity - 2f * dot * normal;

        // 반사된 속도에 감쇄 계수를 곱해 에너지 감소 표현
        velocity = reflect * damping;
    }
}