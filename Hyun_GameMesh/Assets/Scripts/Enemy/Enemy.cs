using UnityEngine;
using UnityEngine.SceneManagement;

public class ChaserEnemy : MonoBehaviour
{
    public Transform player;
    public float rotationSpeed = 50f;
    public float detectionRange = 8f;
    public float dashSpeed = 2f;
    public float stopDistance = 10f;
    public bool isDashing = false;
    public float parryAngle = 120f;


    // 과제의 3가지 적 타입(시야각 60, 90, 180) 조절을 위한 변수 추가
    public float viewAngle = 60f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!isDashing) // 회전 모드
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

            // [과제] 내적을 사용하여 '전방 시야각' 판정 추가
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= detectionRange)
            {
                // 적과 플레이어 사이의 방향 벡터
                Vector3 directionToPlayer = (player.position - transform.position).normalized;

                // 적의 정면(forward)과 플레이어 방향 벡터의 내적(Dot) 계산
                float dotProduct = Vector3.Dot(transform.forward, directionToPlayer);

                // 시야각(viewAngle)의 절반에 대한 코사인 값과 내적값을 비교
                // 예: 시야각이 60도면 좌우 30도. Mathf.Cos(30도)
                float viewCos = Mathf.Cos(viewAngle * 0.5f * Mathf.Deg2Rad);

                // 내적값이 코사인 값보다 크면 시야 안에 있음
                if (dotProduct >= viewCos)
                {
                    isDashing = true;
                }
            }
        }
        else
        {
            // Dash 모드 일 때 플레이어 쪽으로 가기
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // 거리 판단해서 플레이어랑 가까우면 CheckParry 수행
            if (distanceToPlayer <= stopDistance)
            {
                CheckParry();
            }
            else
            {
                // 돌진: 플레이어 방향으로 이동
                Vector3 directionToPlayer = (player.position - transform.position).normalized;
                transform.position += directionToPlayer * dashSpeed * Time.deltaTime;

                // 돌진 시 플레이어 쪽을 바라보도록 회전 (선택 사항)
                transform.LookAt(player);
            }
        }
    }



    void CheckParry()
    {
        PlayerMove pc = player.GetComponent<PlayerMove>();

        // Y축 높이를 무시한 평면(XZ) 기준 방향 벡터 계산
        Vector3 flatPlayerPos = new Vector3(player.position.x, 0f, player.position.z);
        Vector3 flatEnemyPos = new Vector3(transform.position.x, 0f, transform.position.z);
        Vector3 directionToEnemy = (flatEnemyPos - flatPlayerPos).normalized;
        Vector3 flatPlayerForward = new Vector3(player.forward.x, 0f, player.forward.z).normalized;

        // 1. [추가된 부분] 내적을 사용하여 적이 패링 가능한 각도(앞쪽) 안에 있는지 확인
        float dotProduct = Vector3.Dot(flatPlayerForward, directionToEnemy);
        float parryCos = Mathf.Cos(parryAngle * 0.5f * Mathf.Deg2Rad);

        bool isParrySuccess = false;

        // 적이 패링 가능 각도(정면 기준 parryAngle 안)에 있을 때만 좌/우 판별 실행
        if (dotProduct >= parryCos)
        {
            // 2. 외적을 사용한 좌/우 판별
            Vector3 crossProduct = Vector3.Cross(flatPlayerForward, directionToEnemy);

            if (crossProduct.y > 0 && pc.isRightParrying) // 적이 오른쪽 & 오른쪽 패링 중
            {
                isParrySuccess = true;
            }
            else if (crossProduct.y < 0 && pc.isLeftParrying) // 적이 왼쪽 & 왼쪽 패링 중
            {
                isParrySuccess = true;
            }
        }
        else
        {
            // 각도를 벗어남 (예: 등 뒤에서 공격당함)
            Debug.Log("패링 각도를 벗어났습니다! (등 뒤 피격)");
        }

        // 패링 결과 처리
        if (isParrySuccess)
        {
            Destroy(gameObject); // 패링 성공
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // 패링 실패 (씬 재시작)
        }
    }
}