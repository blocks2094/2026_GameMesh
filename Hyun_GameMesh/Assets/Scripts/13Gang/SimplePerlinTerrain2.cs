using UnityEngine;

public class SimplePerlinTerrain2 : MonoBehaviour
{
    [Header("맵 크기")]
    public int width = 50;
    public int depth = 50;

    [Header("노이즈 설정")]
    public float scale = 0.08f;
    public float heightMultiplier = 18f;

    [Header("산지 / 물 비율")]
    [Range(0f, 1f)]
    public float landRatio = 0.5f;

    [Header("산지 높이 설정")]
    public int mountainBaseHeight = 4;

    [Header("물 높이")]
    public int waterLevel = 2;

    [Header("잔디 두께")]
    public int grassDepth = 3;

    [Header("타일 프리팹")]
    public GameObject dirtPrefab;
    public GameObject grassPrefab;
    public GameObject waterPrefab;

    private SimpleperlinNoise2 simpleNoise;

    private GameObject[,,] tileMap;

    private void Start()
    {
        simpleNoise = GetComponent<SimpleperlinNoise2>();

        if (simpleNoise == null)
        {
            Debug.LogError("SimpleperlinNoise 스크립트가 같은 오브젝트에 없습니다.");
            return;
        }

        RandomizeSeed();

        Generate();
    }

    // 매 실행마다 다른 지형이 나오도록 seed 변경
    void RandomizeSeed()
    {
        simpleNoise.seed = Random.Range(0, 100000);
    }

    public void Generate()
    {
        int maxHeight = mountainBaseHeight + Mathf.RoundToInt(heightMultiplier);

        tileMap = new GameObject[width, maxHeight + 1, depth];

        float landThreshold = 1f - landRatio;

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                float xCoord = x * scale;
                float zCoord = z * scale;

                float noise = simpleNoise.Noise(xCoord, zCoord);

                // noise가 기준값 이상이면 산지
                if (noise >= landThreshold)
                {
                    float mountainNoise = Mathf.InverseLerp(landThreshold, 1f, noise);

                    int height = mountainBaseHeight + Mathf.RoundToInt(mountainNoise * heightMultiplier);

                    CreateGroundColumn(x, z, height);
                }
                // noise가 기준값보다 낮으면 물
                else
                {
                    CreateWaterColumn(x, z);
                }
            }
        }
    }

    void CreateGroundColumn(int x, int z, int height)
    {
        for (int y = 0; y <= height; y++)
        {
            GameObject prefab;

            if (y >= height - (grassDepth - 1))
            {
                prefab = grassPrefab;
            }
            else
            {
                prefab = dirtPrefab;
            }

            CreateTile(prefab, x, y, z);
        }
    }

    // 물 생성
    void CreateWaterColumn(int x, int z)
    {
        for (int y = 0; y <= waterLevel; y++)
        {
            CreateTile(waterPrefab, x, y, z);
        }
    }

    void CreateTile(GameObject prefab, int x, int y, int z)
    {
        Vector3 position = new Vector3(x, y, z);

        GameObject tile = Instantiate(prefab, position, Quaternion.identity, transform);

        tileMap[x, y, z] = tile;
    }
}