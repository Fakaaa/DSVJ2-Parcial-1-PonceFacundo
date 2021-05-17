using UnityEngine;

public class CreateMap : MonoBehaviour
{   
    [SerializeField][Range(20,32)] public int maxWidth; 
    [SerializeField][Range(20,32)] public int maxHeight;

    [SerializeField] public GameObject refFloor;
    [SerializeField] public GameObject refWallBreakable;
    [SerializeField] public GameObject refWallUnBreakable;

    private GameObject floor;
    private GameObject[,] wallsCenter;
    private GameObject[] wallsOuterMap;
    private float scaleTileXWalls;
    private float scaleTileZWalls;

    private float scaleFloorX;
    private float scaleFloorY;
    public void Start()
    {
        wallsCenter = new GameObject[maxWidth , maxHeight];
        wallsOuterMap = new GameObject[4];
        scaleTileXWalls = refWallUnBreakable.transform.localScale.x;
        scaleTileZWalls = refWallUnBreakable.transform.localScale.z;
        scaleFloorX = maxWidth;
        scaleFloorY = maxHeight;

        Vector3 initialPos = new Vector3(0.5f, -0.5f, 0.5f);

        wallsOuterMap[0] = Instantiate(refWallUnBreakable, new Vector3(scaleFloorX * 0.5f, 0.5f, 0.5f), refFloor.transform.localRotation, transform);
        wallsOuterMap[0].transform.localScale = new Vector3(scaleFloorX, 1 , 1);

        wallsOuterMap[1] = Instantiate(refWallUnBreakable, new Vector3(scaleFloorX * 0.5f, 0.5f, scaleFloorY - 0.5f), refFloor.transform.localRotation, transform);
        wallsOuterMap[1].transform.localScale = new Vector3(scaleFloorX, 1, 1);

        wallsOuterMap[2] = Instantiate(refWallUnBreakable, new Vector3(0.5f, 0.5f,scaleFloorY * 0.5f), refFloor.transform.localRotation, transform);
        wallsOuterMap[2].transform.localScale = new Vector3(1, scaleFloorY, 1);

        wallsOuterMap[3] = Instantiate(refWallUnBreakable, new Vector3(scaleFloorX - 0.5f, 0.5f, scaleFloorY * 0.5f), refFloor.transform.localRotation, transform);
        wallsOuterMap[3].transform.localScale = new Vector3(1, scaleFloorY, 1);

        for (int i = 1; i < maxWidth / 2.25f; i++)
        {
            for (int j = 1; j < maxHeight / 2.25f; j++)
            {
                Vector3 posWall = new Vector3((initialPos.x + (i * scaleTileXWalls)) * 2, 0.5f, (initialPos.z + (j * scaleTileZWalls)) * 2);
                wallsCenter[i, j] = Instantiate(refWallUnBreakable, posWall , refFloor.transform.localRotation, transform);
            }
        }

        floor = Instantiate(refFloor, new Vector3( scaleFloorX * 0.5f, 0.0f ,scaleFloorY * 0.5f), refFloor.transform.localRotation, transform);
        floor.transform.localScale = new Vector3(scaleFloorX, scaleFloorY, 1);
    }

    void FindPlaceWallsBreakable()
    {

    }
}
