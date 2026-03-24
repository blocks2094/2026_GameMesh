using UnityEngine;

public class FollorCam : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 4f, -5f);

    public float smoothSpeed = 5f;

    private void LateUpdate()
    {
        Vector3 latePosition = target.position + Quaternion.Euler(0f, target.eulerAngles.y, 0f) * offset;
        transform.position = latePosition;

        transform.LookAt(target);
    }
}
