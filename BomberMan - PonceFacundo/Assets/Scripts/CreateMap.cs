using UnityEngine;

public class CreateMap : MonoBehaviour
{   
    [SerializeField] public int maxWidth; 
    [SerializeField] public int maxHeight;

    [SerializeField] public GameObject refFloor;
    [SerializeField] public GameObject refWallBreakable;
    [SerializeField] public GameObject refWallUnBreakable;

    private GameObject[,] floor;
    private GameObject[,] walls;
    private float scaleTileX;
    private float scaleTileZ;
    public void Start()
    {
        GameObject [,] mapAux = new GameObject[maxWidth , maxHeight];
        floor = mapAux;
        walls = mapAux;
        scaleTileX = refFloor.transform.localScale.x;
        scaleTileZ = refFloor.transform.localScale.z;

        Vector3 initialPos = new Vector3(0.5f, -0.5f, 0.5f);

        for (int i = 0; i < maxWidth; i++)
        {
            walls[i,0] = Instantiate(refWallUnBreakable, new Vector3(initialPos.x + (i * scaleTileX), 0.5f, initialPos.z + (0 * scaleTileZ)),
                    refFloor.transform.localRotation, transform);
            for (int j = 0; j < maxHeight; j++)
            {
                walls[0, j] = Instantiate(refWallUnBreakable, new Vector3(initialPos.x + (0 * scaleTileX), 0.5f, initialPos.z + (j * scaleTileZ)),
                    refFloor.transform.localRotation, transform);
            }
        }
        for (int i = 0; i < maxWidth; i++)
        {
            walls[i, maxHeight-1] = Instantiate(refWallUnBreakable, new Vector3(initialPos.x + (i * scaleTileX), 0.5f, initialPos.z + (maxHeight - 1 * scaleTileZ)),
                    refFloor.transform.localRotation, transform);
            for (int j = 0; j < maxHeight; j++)
            {
                walls[maxWidth-1, j] = Instantiate(refWallUnBreakable, new Vector3(initialPos.x + (maxWidth - 1 * scaleTileX), 0.5f, initialPos.z + (j * scaleTileZ)),
                    refFloor.transform.localRotation, transform);
            }
        }

        for (int i = 1; i < maxWidth * 0.4f; i++)
        {
            for (int j = 1; j < maxHeight * 0.4f; j++)
            {
                walls[i, j] = Instantiate(refWallUnBreakable, new Vector3(initialPos.x + (i * (scaleTileX*2.1f)), 0.5f, initialPos.z + (j * (scaleTileZ* 2.1f))),
                    refFloor.transform.localRotation, transform);
            }
        }

        for (int i = 0; i < maxWidth; i++)
        {
            for (int j = 0; j < maxHeight; j++)
            {
                floor[i,j] = Instantiate(refFloor, new Vector3(initialPos.x + (i * scaleTileX), 0.0f, initialPos.z + (j * scaleTileZ)),
                    refFloor.transform.localRotation, transform);
            }
        }
    }
}
