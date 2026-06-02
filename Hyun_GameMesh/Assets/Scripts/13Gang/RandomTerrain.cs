using UnityEngine;

public class RandomTerrain : MonoBehaviour
{
    public int width = 30;

    public int depth = 30;

    public int minHeight = 0;

    public int maxHeight = 8;

    public GameObject cubePrefab;

    private void Start()
    {
        Generate();
    }

    public void Generate()
    {
        for(int x = 0; x < width; x++)
        {
            for(int z = 0; z < depth; z++)
            {
                int height = Random.Range(minHeight, maxHeight + 1);
                CreateCube(x, z, height);
            }
        }
    }

    void CreateCube(int x, int z, int height)
    {
        for(int y = 0; y <= height; y++)
        {
            Vector3 position = new Vector3(x, y, z);

            Instantiate(cubePrefab, position, Quaternion.identity, transform);  
        }
    }
}
