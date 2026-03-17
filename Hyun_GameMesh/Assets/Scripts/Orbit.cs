using UnityEngine;

public class Orbit : MonoBehaviour
{
    public Transform centerObject;

    public float radius = 5f;
    public float speed = 2f;
    private float currentAngle = 0f;

    private void Update()
    {
        if (centerObject == null) return;
        currentAngle += speed * Time.deltaTime; 

        float x = centerObject.position.x + Mathf.Cos(currentAngle) * radius;   
        float z = centerObject.position.z + Mathf.Sin(currentAngle) * radius;
        float y = centerObject.position.y;

        transform.position = new Vector3(x, y, z);

    }
}
