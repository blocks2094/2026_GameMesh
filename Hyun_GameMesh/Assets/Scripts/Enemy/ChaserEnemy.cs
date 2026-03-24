using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; 

public class ChaserEnemy : MonoBehaviour
{
    public Transform player;
    public float rotationSpeed = 50f;
    public float detectionRange = 8f;
    public float dashSpeed = 2f;
 
    public float stopDistance = 1.5f;
    public bool isDashing = false;
    public float parryAngle = 120f; 

    public float viewAngle = 60f;

    private Rigidbody rb;
    private bool isAttacking = false; // 공격 중인지 확인 

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isAttacking) return;


        if (!isDashing) 
        {
            // 제자리 회전
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

            // 플레이어와의 거리 계산
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // 플레이어가 탐지 거리 이내에 있는지 확인
            if (distanceToPlayer <= detectionRange)
            {
                // 적이 플레이어를 바라보는 방향 벡터 계산 
                Vector3 directionToPlayer = (player.position - transform.position).normalized;

                // '전방 시야각' 판정
                float dotProduct = Vector3.Dot(transform.forward, directionToPlayer);

                // 시야각(viewAngle)의 절반에 대한 코사인 값 계산 (좌우 대칭이므로 절반)
                float viewCos = Mathf.Cos(viewAngle * 0.5f * Mathf.Deg2Rad);

                // 내적값이 코사인 값보다 크면 시야 안에 있음 -> 발견!
                if (dotProduct >= viewCos)
                {
                    isDashing = true; // 돌진 모드로 전환
                }
            }
        }
        else // Dash 모드 
        {
            // 플레이어와의 2D 거리 계산 (Y축 높이 무시)
            Vector3 flatPlayerPos = new Vector3(player.position.x, 0f, player.position.z);
            Vector3 flatEnemyPos = new Vector3(transform.position.x, 0f, transform.position.z);
            float distanceToPlayer = Vector3.Distance(flatEnemyPos, flatPlayerPos);

            // 거리 판단해서 플레이어랑 가까우면 '공격 및 패링 대기' 코루틴 시작
            if (distanceToPlayer <= stopDistance)
            {
                StartCoroutine(AttackRoutine());
            }
            else
            {
                // 돌진: 플레이어 방향으로 이동
                Vector3 directionToPlayer = (flatPlayerPos - flatEnemyPos).normalized;
                transform.position += directionToPlayer * dashSpeed * Time.deltaTime;

                // 돌진 시 플레이어 쪽을 바라보도록 회전
                transform.LookAt(flatPlayerPos);
            }
        }
    }

    // 반응할 시간을 주는 핵심 로직 
    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        isDashing = false; // 이동 멈춤

        // 반응 시간 0.5초
        float reactionWindow = 0.5f;
        float timer = 0f;

        Debug.Log("<color=yellow>적 공격 시작! 패링하세요!</color>");

        // 반응 시간동안 키를 누르는지 감시
        while (timer < reactionWindow)
        {
            timer += Time.deltaTime;

            // 매 프레임 패링 성공 여부를 확인
            if (CheckParryLogic())
            {
                Debug.Log("<color=green>★패링 성공! 적 오브젝트 파괴★</color>");
                Destroy(gameObject); // 적 삭제
                yield break; 
            }

            yield return null; 
        }

        // [과제 Snippet 2]의 else 부분 (코루틴용 수정)
        // 0.5초 안에 패링 성공 못하면 실패 처리 (씬 재시작)
        Debug.Log("<color=red>패링 실패! 게임 오버</color>");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    bool CheckParryLogic()
    {
        PlayerController pc = player.GetComponent<PlayerController>();

        Vector3 flatPlayerPos = new Vector3(player.position.x, 0f, player.position.z);
        Vector3 flatEnemyPos = new Vector3(transform.position.x, 0f, transform.position.z);
        Vector3 directionToEnemy = (flatEnemyPos - flatPlayerPos).normalized;
        Vector3 flatPlayerForward = new Vector3(player.forward.x, 0f, player.forward.z).normalized;

        Vector3 crossProduct = Vector3.Cross(flatPlayerForward, directionToEnemy);

 
        if (crossProduct.y >= -0.1f && crossProduct.y <= 0.1f)
        {
            if (pc.isLeftParrying || pc.isRightParrying) return true;
        }
        // 왼쪽에 있을 때
        else if (crossProduct.y > 0.1f)
        {
            if (pc.isLeftParrying) return true;
        }
        // 오른쪽에 있을 때
        else if (crossProduct.y < -0.1f)
        {
            if (pc.isRightParrying) return true;
        }

        return false;
    }
}