using UnityEngine;
using UnityEngine.InputSystem;

public class LineRender : MonoBehaviour
{
    public Transform startPos;
    public Transform endPos;

    [Range(1f, 5f)] public float extend = 1.5f;

    private LineRenderer lr;
    private bool isRender = false;
    private CameraSlerp cameraSlerp;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 0;
        lr.widthMultiplier = 0.05f;
        lr.material = new Material(Shader.Find("Unlit/Color")) { color = Color.red };
        cameraSlerp = FindObjectOfType<CameraSlerp>();
    }

    private void Update()
    {
        if (!isRender) return; 
        if (!startPos || !endPos) return;
        Vector3 a = startPos.position;
        Vector3 b = endPos.position;
        Vector3 pred = Vector3.LerpUnclamped(a, b, extend);
        lr.SetPosition(0, a);
        lr.SetPosition(1, pred);
    }

    public void OnRightClick(InputValue value)
    {
        if (!value.isPressed) return;
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                isRender = true;
                lr.positionCount = 2;
                endPos = hit.transform;
                cameraSlerp.target = endPos;
            }
        }
        else
        {
            isRender = false;
            lr.positionCount = 0;
        }
    }

    
}
