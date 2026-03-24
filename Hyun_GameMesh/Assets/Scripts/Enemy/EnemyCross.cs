using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyCross : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        Vector3 forward = transform.forward;
        Vector3 dirToTarget = (target.position - transform.position).normalized;

        Vector3 crossProduct = Vector3.Cross(forward, dirToTarget);

        if(crossProduct.y > 0.1f || crossProduct.y < -0.1f)
        {

        }
        

    }
}
