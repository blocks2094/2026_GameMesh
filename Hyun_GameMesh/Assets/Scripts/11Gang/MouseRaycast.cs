using UnityEngine;
using UnityEngine.InputSystem;

public class MouseRaycast : MonoBehaviour
{
    public float rayDistance = 100f;
    public float hitPower = 10f;

    float moveInput;

    public CamerOrbit cam;
    public BilliardTurnManager turnManager;

    public void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        moveInput = input.x;
        cam.moveInput = moveInput;
    }

    public void OnClick(InputValue value)
    {
        if (!value.isPressed) return;

        Vector2 mousePos = Mouse.current.position.ReadValue();

        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            Rigidbody rb = hit.collider.attachedRigidbody;

            if (rb != null)
            {
                if (!turnManager.CanShoot(rb))
                {
                    Debug.Log("현재 턴의 공이 아니거나 이미 쳤습니다.");
                    return;
                }

                Vector3 dir = Camera.main.transform.forward;
                dir.y = 0f;
                dir.Normalize();

                turnManager.Shot();
                rb.AddForce(dir * hitPower, ForceMode.Impulse);

               
            }
        }
    }
}