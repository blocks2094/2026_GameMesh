using UnityEngine;
using UnityEngine.InputSystem;

public class ClickMove : MonoBehaviour
{
    public float moveSpeed = 3f;
    private Vector2 mouseScreenPosition;
    
    private Vector3 targetPosition;
    private bool isMoving = false;
    private bool isSprinting = false;


    public void OnPoint(InputValue value)
    {
        mouseScreenPosition = value.Get<Vector2>();
    }

    public void OnClick(InputValue value)
    {
        if(value.isPressed)
        {
            Ray ray = Camera.main.ScreenPointToRay(mouseScreenPosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);

            foreach (RaycastHit hit in hits)
            {
                if(hit.collider.gameObject != gameObject)
                {
                    targetPosition = hit.point;
                    targetPosition.y = transform.position.y;
                    isMoving = true;

                    break;
                }
            }
        }
    }
    public void OnSprint(InputValue value)
    {
        isSprinting = value.isPressed;
    }

    void Update()
    {
        if (isMoving)
        {
            Vector3 direction = targetPosition - transform.position;

            float sqrMagnitude = direction.x * direction.x + direction.y * direction.y + direction.z * direction.z;
            float magnitude = Mathf.Sqrt(sqrMagnitude);
            Vector3 normalizedVector = Vector3.zero;

            if (magnitude >= 0.1f)
            {
                normalizedVector = direction / magnitude;
            }
            transform.position += normalizedVector * moveSpeed * Time.deltaTime;
        }

        if (isSprinting)
        {
            moveSpeed = 6f;
        }
        if (!isSprinting)
        {
            moveSpeed = 3f;
        }
    }

}
