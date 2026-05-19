using UnityEngine;

public class CamerOrbit : MonoBehaviour
{
    public Transform target;

    private float yaw = 0f;

    public float moveInput = 0f;
    public float rotateSpeed = 100f;

    public Vector3 offset = new Vector3(0f, 4f, -7f);

    [Header("Camera Smooth")]
    public float moveSmooth = 5f;
    public float lookSmooth = 8f;

    private void Update()
    {
        if (target == null) return;

        yaw += moveInput * rotateSpeed * Time.deltaTime;

        Quaternion rot = Quaternion.Euler(0f, yaw, 0f);
        Vector3 targetPos = target.position + rot * offset;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPos,
            moveSmooth * Time.deltaTime
        );

        Vector3 lookDir = target.position - transform.position;

        if (lookDir != Vector3.zero)
        {
            Quaternion lookRot = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                lookRot,
                lookSmooth * Time.deltaTime
            );
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        moveInput = 0f;
    }
}