using UnityEngine;
using UnityEngine.InputSystem;

public class ZiggsSkillController : MonoBehaviour
{
    [Header("폭탄 프리팹")]
    [SerializeField] private GameObject qBombPrefab;
    [SerializeField] private GameObject wBombPrefab;

    [Header("Q 생성 위치")]
    [SerializeField] private Transform throwPoint;

    [Header("W 생성 위치")]
    [SerializeField] private Transform wSpawnPoint;

    [Header("Q 설정")]
    [SerializeField] private float qPower = 14f;
    [SerializeField] private float qUpPower = 5f;
    [SerializeField] private float qEnemySearchRange = 15f;
    [SerializeField] private LayerMask enemyLayer;

    private void Awake()
    {
        if (throwPoint == null)
            throwPoint = transform;

        if (wSpawnPoint == null)
            wSpawnPoint = transform;
    }

    // PlayerInput Behavior가 Send Messages일 때 호출됨
    public void OnQ(InputValue value)
    {
        if (value.isPressed)
            CastQ();
    }

    public void OnW(InputValue value)
    {
        if (value.isPressed)
            CastW();
    }

    public void CastQ()
    {
        if (qBombPrefab == null)
        {
            Debug.LogWarning("Q 폭탄 프리팹이 없습니다.");
            return;
        }

        Vector3 dir = GetEnemyDir();

        GameObject bomb = Instantiate(
            qBombPrefab,
            throwPoint.position,
            Quaternion.identity
        );

        ZiggsQBomb qBomb = bomb.GetComponent<ZiggsQBomb>();

        if (qBomb != null)
            qBomb.Init(dir, qPower, qUpPower);
    }

    public void CastW()
    {
        if (wBombPrefab == null)
        {
            Debug.LogWarning("W 폭탄 프리팹이 없습니다.");
            return;
        }

        if (wSpawnPoint == null)
        {
            Debug.LogWarning("W 생성 위치가 없습니다.");
            return;
        }

        Instantiate(
            wBombPrefab,
            wSpawnPoint.position,
            wSpawnPoint.rotation
        );
    }

    private Vector3 GetEnemyDir()
    {
        Collider[] enemies = Physics.OverlapSphere(
            transform.position,
            qEnemySearchRange,
            enemyLayer
        );

        Transform nearestEnemy = null;
        float nearestDist = Mathf.Infinity;

        foreach (Collider enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);

            if (dist < nearestDist)
            {
                nearestDist = dist;
                nearestEnemy = enemy.transform;
            }
        }

        Vector3 dir;

        if (nearestEnemy != null)
        {
            dir = nearestEnemy.position - transform.position;
        }
        else
        {
            dir = transform.forward;
        }

        dir.y = 0f;
        dir.Normalize();

        return dir;
    }
}