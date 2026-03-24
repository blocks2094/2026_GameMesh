using UnityEngine;

public class AutoRotation : MonoBehaviour
{
   
    void Update()
    {
        transform.Rotate(0, 45 * Time.deltaTime, 0);
    }
}
