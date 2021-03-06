using UnityEngine;

public class CreateMap : MonoBehaviour
{
    [SerializeField]
    [Range(18, 102)]
    [Tooltip("Recomendado: Multiplos de Dos")]
    public int maxWidth;
    [SerializeField]
    [Range(20, 102)]
    [Tooltip("Recomendado: Multiplos de Dos")]
    public int maxHeight;

    [SerializeField] public GameObject refFloor;
    [SerializeField] public GameObject refWallBreakable;
    [SerializeField] public GameObject refWallUnBreakable;
    [SerializeField] public GameObject refDoorPrefab;
    [SerializeField] public GameObject refEnemy;

    [SerializeField] public GameObject spawnerLocation1;
    [SerializeField] public GameObject spawnerLocation2;
    [SerializeField] public GameObject spawnerLocation3;
    [SerializeField] public GameObject spawnerLocation4;

    private GameObject floor;
    private GameObject[,] wallsCenterMap;
    private GameObject[] wallsOuterMap;
    private float scaleTileXWalls;
    private float scaleTileZWalls;

    private float offsetBetweenWalls;
    public static float scaleFloorX;
    public static float scaleFloorY;

    public bool doorPlaced;
    private float heightSpawn;
    private int offsetSpawn;

    public void Start()
    {
        heightSpawn = 0.2f;
        offsetSpawn = 2;
        doorPlaced = false;
        offsetBetweenWalls = 2.0f;
        wallsCenterMap = new GameObject[maxWidth, maxHeight];
        wallsOuterMap = new GameObject[4];
        scaleTileXWalls = refWallUnBreakable.transform.localScale.x;
        scaleTileZWalls = refWallUnBreakable.transform.localScale.z;
        scaleFloorX = maxWidth;
        scaleFloorY = maxHeight;

        spawnerLocation1.transform.position = new Vector3(scaleFloorX * 0.5f, heightSpawn, scaleFloorY * 0.5f);
        spawnerLocation2.transform.position = new Vector3(scaleFloorX - (scaleTileXWalls + offsetSpawn), heightSpawn, (scaleTileZWalls + offsetSpawn));
        spawnerLocation3.transform.position = new Vector3((scaleTileXWalls + offsetSpawn), heightSpawn, scaleFloorY - (scaleTileZWalls + offsetSpawn));
        spawnerLocation4.transform.position = new Vector3(scaleFloorX - (scaleTileXWalls + offsetSpawn), heightSpawn, scaleFloorY - (scaleTileZWalls + offsetSpawn));

        Vector3 initialPos = new Vector3(0.0f, -0.5f, 0.0f);
        float offsetOuterMap = 1.0f;

        wallsOuterMap[0] = Instantiate(refWallUnBreakable, new Vector3(scaleFloorX * 0.5f, 0.5f, 0.0f), refFloor.transform.localRotation, transform);
        wallsOuterMap[0].transform.localScale = new Vector3(scaleFloorX - offsetOuterMap, 1, 1);
        wallsOuterMap[0].gameObject.GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(scaleFloorX * 0.5f, 1);

        wallsOuterMap[1] = Instantiate(refWallUnBreakable, new Vector3(scaleFloorX * 0.5f, 0.5f, scaleFloorY), refFloor.transform.localRotation, transform);
        wallsOuterMap[1].transform.localScale = new Vector3(scaleFloorX - offsetOuterMap, 1, 1);
        wallsOuterMap[1].gameObject.GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(scaleFloorX * 0.5f, 1);

        wallsOuterMap[2] = Instantiate(refWallUnBreakable, new Vector3(0.0f, 0.5f, scaleFloorY * 0.5f), refFloor.transform.localRotation, transform);
        wallsOuterMap[2].transform.localScale = new Vector3(1, scaleFloorY, 1);
        wallsOuterMap[2].gameObject.GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(1, scaleFloorY * 0.5f);

        wallsOuterMap[3] = Instantiate(refWallUnBreakable, new Vector3(scaleFloorX, 0.5f, scaleFloorY * 0.5f), refFloor.transform.localRotation, transform);
        wallsOuterMap[3].transform.localScale = new Vector3(1, scaleFloorY, 1);
        wallsOuterMap[3].gameObject.GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(1, scaleFloorY * 0.5f);

        int randIteration = 0;
        int anotherRandIter = 0;

        for (int i = 1; i < maxWidth * 0.5f; i++)
        {
            for (int j = 1; j < maxHeight * 0.5f; j++)
            {
                Vector3 posWallUnbreakable = new Vector3((initialPos.x + (i * scaleTileXWalls)) * offsetBetweenWalls, 0.5f,
                    (initialPos.z + (j * scaleTileZWalls)) * offsetBetweenWalls);
                wallsCenterMap[i, j] = Instantiate(refWallUnBreakable, posWallUnbreakable, refFloor.transform.localRotation, transform);

                FindPlaceWallsBreakable(ref posWallUnbreakable, ref anotherRandIter, ref randIteration, i, j);
            }
        }

        FindPlaceEnemy();

        floor = Instantiate(refFloor, new Vector3(scaleFloorX * 0.5f, 0.0f, scaleFloorY * 0.5f), refFloor.transform.localRotation, transform);
        floor.transform.localScale = new Vector3(scaleFloorX, scaleFloorY, 1);
    }

    void FindPlaceWallsBreakable(ref Vector3 posWallUnbreakable, ref int anotherRandIter, ref int randIteration, int i, int j)
    {
        int randomPlaceBomb = 0;
        anotherRandIter = Random.Range(0, 30);

        if (anotherRandIter > 15)
            randIteration = Random.Range(0, 10);

        if (randIteration > 5)
        {
            randomPlaceBomb = Random.Range(0, 100);
            Vector3 posWallBreakable = Vector3.zero;
            if (anotherRandIter < 15)
            {
                if (((posWallUnbreakable.x + scaleTileXWalls) != (scaleFloorX * 0.5f) && (posWallUnbreakable.z + scaleTileZWalls) != (scaleFloorY * 0.5f)) &&
                    (posWallUnbreakable.x + scaleTileXWalls) != maxWidth && (posWallUnbreakable.z + scaleTileZWalls) != maxHeight)
                    posWallBreakable = new Vector3(posWallUnbreakable.x + scaleTileXWalls, 0.5f, posWallUnbreakable.z + scaleTileZWalls);
                else
                    return;
            }
            else
            {
                if (((posWallUnbreakable.x + scaleTileXWalls) != (scaleFloorX * 0.5f) && (posWallUnbreakable.z) != (scaleFloorY * 0.5f)) &&
                    (posWallUnbreakable.x + scaleTileXWalls) != maxWidth && (posWallUnbreakable.z + scaleTileZWalls) != maxHeight)
                    posWallBreakable = new Vector3(posWallUnbreakable.x + scaleTileXWalls, 0.5f, posWallUnbreakable.z);
                else
                    return;
            }

            if (!doorPlaced)
            {
                if (randomPlaceBomb > 90)
                {
                    wallsCenterMap[i, j] = Instantiate(refDoorPrefab, posWallBreakable, refFloor.transform.localRotation, transform);
                    doorPlaced = true;
                }
                if (i == (maxWidth * 0.5f) && j == (maxWidth * 0.5f) && !doorPlaced)
                {
                    wallsCenterMap[i, j] = Instantiate(refDoorPrefab, posWallBreakable, refFloor.transform.localRotation, transform);
                    doorPlaced = true;
                }
            }
            wallsCenterMap[i, j] = Instantiate(refWallBreakable, posWallBreakable, refFloor.transform.localRotation, transform);
        }
    }

    void FindPlaceEnemy()
    {
        if (GameManager.Get() != null)
        {
            for (int i = 0; i < GameManager.Get().GetMaxAmountEnemies(); i++)
            {
                Vector3 randomPos = Vector3.zero;
                int randSpawner = Random.Range(0, 4);

                switch (randSpawner)
                {
                    case 0:
                        randomPos = spawnerLocation1.transform.position;
                        break;
                    case 1:
                        randomPos = spawnerLocation2.transform.position;
                        break;
                    case 2:
                        randomPos = spawnerLocation3.transform.position;
                        break;
                    case 3:
                        randomPos = spawnerLocation4.transform.position;
                        break;
                }
                Instantiate(refEnemy, randomPos, Quaternion.identity);
                if (GameManager.Get().GetActualAmountEnemies() < GameManager.Get().GetMaxAmountEnemies())
                    GameManager.Get().IncreaseAmountEnemies();
            }
        }
    }
}
