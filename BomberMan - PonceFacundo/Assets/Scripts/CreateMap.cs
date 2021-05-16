using UnityEngine;

public class CreateMap : MonoBehaviour
{   
    [SerializeField] public int maxWidth; 
    [SerializeField] public int maxHeight;
    [SerializeField] public Color colorFloor;

    [SerializeField] public GameObject refFloor;

    private GameObject[,] map;
    private float scaleTileX;
    private float scaleTileZ;
    public void Start()
    {
        GameObject [,] mapAux = new GameObject[maxWidth , maxHeight];
        map = mapAux;
        scaleTileX = refFloor.transform.localScale.x;
        scaleTileZ = refFloor.transform.localScale.z;

        Vector3 initialPos = new Vector3(0.5f, -0.5f, 0.5f);

        for (int i = 0; i < maxWidth; i++)
        {
            for (int j = 0; j < maxHeight; j++)
            {
                map[i,j] = Instantiate(refFloor, new Vector3(initialPos.x + (i * scaleTileX), 0.0f, initialPos.z + (j * scaleTileZ)),
                    refFloor.transform.localRotation, transform);
                map[i,j].GetComponent<MeshRenderer>().material.color = colorFloor;
            }
        }
    }
}
