using UnityEngine;

public class LerpMover : MonoBehaviour
{
    public Transform startPos;
    public Transform endPos;

    [SerializeField] private float duration = 2f;
    [SerializeField] private float t = 0f;

    private void Update()
    {
        float t = Mathf.PingPong(Time.time / duration, 1f);
        transform.position = Vector3.Lerp(startPos.position, endPos.position, t);
    }
}
