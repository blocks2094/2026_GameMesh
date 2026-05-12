using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private GameObject bulletObject;
    [SerializeField] private int fireCount = 10;

    private void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            OnAttack();
        }
    }

    private void OnAttack()
    {
        for (int i = 0; i < fireCount; i++)
        {
            GameObject bullet = Instantiate(
                bulletObject,
                transform.position,
                Quaternion.identity
            );

            TestBezier bezier = bullet.GetComponent<TestBezier>();

            Destroy(bullet, 2f);
        }
    }
}