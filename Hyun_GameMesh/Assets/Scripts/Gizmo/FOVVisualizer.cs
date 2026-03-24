using UnityEngine;
// 내적 범위를 씬 뷰에서 기즈모로 보이게 하는 스크립트

public class FOVVisualizer : MonoBehaviour
{
    
    public float viewAngle = 60f;

    public float viewDistance = 5f;


    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Vector3 forward = transform.forward * viewDistance;

        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2, 0) * forward;

        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2, 0) * forward;

        Gizmos.DrawRay(transform.position, leftBoundary);
        Gizmos.DrawRay(transform.position, rightBoundary);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, forward);
    }
}
