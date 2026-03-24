using UnityEngine;

public class DotExample : MonoBehaviour
{
    public Transform player;

    public float viewAngle = 60f;
    public float viewDistance = 10f;
    public float scaleMultiplier = 1.5f;

    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;  
    }

    private void Update()
    {
  
       if(player == null) return;   

       float distance = Vector3.Distance(transform.position, player.position);

        Vector3 toPlayer = (player.position - transform.position).normalized;
        Vector3 forward = transform.forward;

        float dot = (forward.x * toPlayer.x) + (forward.y * toPlayer.y) + (forward.z * toPlayer.z);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        if(distance <= viewDistance && angle <= viewAngle / 2f)
        {
            transform.localScale = originalScale * scaleMultiplier;
        }
        else
        {
            transform.localScale = originalScale;
        }
    
    }
}
