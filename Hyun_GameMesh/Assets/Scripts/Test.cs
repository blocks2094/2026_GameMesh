using NUnit.Framework.Constraints;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 moveInput;

    Vector3 normalizedVector;
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
  
    void Update()
    {
        Vector3 direction = new Vector3(moveInput.x, moveInput.y, 0);

        float sqrManitude = direction.x * direction.x + direction.y * direction.y + direction.z * direction.z;
        float magnitude = Mathf.Sqrt(sqrManitude);

        if(magnitude > 0)
        {
            normalizedVector = direction / magnitude;
        }
        else
        {
            normalizedVector = Vector3.zero;
        }

            transform.Translate(direction * moveSpeed * Time.deltaTime);
    }
}
