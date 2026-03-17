using TMPro;
using UnityEngine;

public class SimpleAngleLauncher : MonoBehaviour
{
    public TMP_InputField angleInputField;
    public GameObject spherePrefab;
    public Transform firePoint;
    public float force = 15f;


    public void Launch()
    {
        float angle = float.Parse(angleInputField.text);
        float rad = angle * Mathf.Deg2Rad;

        Vector3 dir = new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad));

        GameObject sphere = Instantiate(spherePrefab, firePoint.position, Quaternion.identity); 
        Rigidbody rd = sphere.GetComponent<Rigidbody>();

        rd.AddForce((dir + Vector3.up * 1.5f).normalized * force, ForceMode.Impulse);
    }
}
