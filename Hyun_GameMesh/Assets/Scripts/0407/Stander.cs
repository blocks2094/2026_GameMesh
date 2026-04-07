using System.Linq;
using UnityEngine;

public class Stander : MonoBehaviour
{
    //public int sampleCount = 1000;
    //public int RandomMin = 0;
    //public int RandomMax = 100;

    public float u1 = 1.0f;
    public float u2 = 1.0f;

    private void Start()
    {
        //StandarDeviation();
        
    }
    public void OnButtonClick()
    {
        float GenerateGaussian(float mean, float stdDev)
        {
            float u1 = 1.0f - Random.value;
            float u2 = 1.0f - Random.value;

            float randStadNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1))
                * Mathf.Sin(2.0f * Mathf.PI * u2);
            Debug.Log("randStadNormal");
            return mean + stdDev * randStadNormal;

        }
        
       
    }

    



    /*
    void StandarDeviation()
    {
        int n = sampleCount;
        float[] samples = new float[n];
        for (int i = 0; i < n; i++)
        {
            samples[i] = Random.Range(RandomMin, RandomMax);  

        }

        float mean = samples.Average(); // 평균
        float sumOfSquares = samples.Sum(x => Mathf.Pow(x = mean, 2));   // 표준편차
        float stdDev = Mathf.Sqrt(sumOfSquares / n);   

        Debug.Log($"평균: {mean}, 표준편차: {stdDev}");
   }
    */


}
