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

    private GameObject floor;
    private GameObject[,] wallsCenterMap;
    private GameObject[] wallsOuterMap;
    private float scaleTileXWalls;
    private float scaleTileZWalls;

    private float offsetBetweenWalls;
    public static float scaleFloorX;
    public static float scaleFloorY;

    public bool doorPlaced;

    public void Awake()
    {
        doorPlaced = false;
        offsetBetweenWalls = 2.0f;
        wallsCenterMap = new GameObject[maxWidth, maxHeight];
        wallsOuterMap = new GameObject[4];
        scaleTileXWalls = refWallUnBreakable.transform.localScale.x;
        scaleTileZWalls = refWallUnBreakable.transform.localScale.z;
        scaleFloorX = maxWidth;
        scaleFloorY = maxHeight;

        Vector3 initialPos = new Vector3(0.0f, -0.5f, 0.0f);
        float offsetOuterMap = 1.0f;

        wallsOuterMap[0] = Instantiate(refWallUnBreakable, new Vector3(scaleFloorX * 0.5f, 0.5f, 0.0f), refFloor.transform.localRotation, transform);
        wallsOuterMap[0].transform.localScale = new Vector3(scaleFloorX - offsetOuterMap, 1, 1);
        wallsOuterMap[0].gameObject.GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(scaleFloorX * 0.5f, 1);

        wallsOuterMap[1] = Instantiate(refWallUnBreakable, new Vector3(scaleFloorX * 0.5f, 0.5f, scaleFloorY), refFloor.transform.localRotation, transform);
        wallsOuterMap[1].transform.localScale = new Vector3(scaleFloorX-offsetOuterMap, 1, 1);
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
                wallsCenterMap[i,j] = Instantiate(refWallUnBreakable, posWallUnbreakable, refFloor.transform.localRotation, transform);

                FindPlaceWallsBreakable(ref posWallUnbreakable, ref anotherRandIter, ref randIteration, i, j);
            }
        }

        floor = Instantiate(refFloor, new Vector3(scaleFloorX * 0.5f, 0.0f, scaleFloorY * 0.5f), refFloor.transform.localRotation, transform);
        floor.transform.localScale = new Vector3(scaleFloorX, scaleFloorY, 1);
    }

    void FindPlaceWallsBreakable(ref Vector3 posWallUnbreakable, ref int anotherRandIter, ref int randIteration, int i, int j)
    {
        anotherRandIter = Random.Range(0, 30);

        if (anotherRandIter > 15)
            randIteration = Random.Range(0, 10);

        if (randIteration > 5)
        {
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
                    (posWallUnbreakable.x + scaleTileXWalls) != maxWidth && (posWallUnbreakable.z + scaleTileZWalls) != maxHeight )
                    posWallBreakable = new Vector3(posWallUnbreakable.x + scaleTileXWalls, 0.5f, posWallUnbreakable.z);
                else
                    return;
            }

            if (!doorPlaced)
            {
                wallsCenterMap[i, j] = Instantiate(refDoorPrefab, posWallBreakable, refFloor.transform.localRotation, transform);
                doorPlaced = true;
            }
            wallsCenterMap[i, j] = Instantiate(refWallBreakable, posWallBreakable, refFloor.transform.localRotation, transform);
        }
    }
}
