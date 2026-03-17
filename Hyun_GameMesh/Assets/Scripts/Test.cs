using NUnit.Framework.Constraints;
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


    private void Start()
    {
        float degrees = 45f;
        float radians = degrees * Mathf.Deg2Rad;
        Debug.Log("45도 -> 라디안 : " + radians);

        float radianValue = Mathf.PI / 3;
        float degreeValue = radianValue * Mathf.Rad2Deg;
        Debug.Log("파이/3 라디안 -> 도 변환 : " + degreeValue);
    }

    void Update()
    {
        #region 이동 구현
        /*
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
        */
        #endregion

        float speed = 5f;
        float angle = 90f;
        float radians = angle * Mathf.Deg2Rad;

        Vector3 direction = new Vector3(Mathf.Cos(radians), 0, Mathf.Sin(radians)); 
        transform.position += direction * speed * Time.deltaTime;   
        
    }

}
