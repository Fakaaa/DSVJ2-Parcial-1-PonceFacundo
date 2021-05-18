using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float playerSpeed;
    [SerializeField] public GameObject ghostModel;

    private Vector3 moveVec;
    private Ray frontRay;
    private RaycastHit myHitFront;
    private Ray backRay;
    private RaycastHit myHitBack;
    private Ray leftRay;
    private RaycastHit myHitLeft;
    private Ray rightRay;
    private RaycastHit myHitRight;

    public enum MoveDirection
    {
        Front,
        Back,
        Left,
        Right,
        None
    }
    [SerializeField] private MoveDirection playerDirection;

    [SerializeField] private bool canGoFront;
    [SerializeField] private bool canGoBack;
    [SerializeField] private bool canGoLeft;
    [SerializeField] private bool canGoRight;

    private float maxDistanceRaycasts;
    [SerializeField] private bool bombPlaced;
    void Start()
    {
        maxDistanceRaycasts = 0.8f;
        transform.position = new Vector3(CreateMap.scaleFloorX - (CreateMap.scaleFloorX-2), 0.2f, CreateMap.scaleFloorY - (CreateMap.scaleFloorY-2));
        moveVec = Vector3.zero;
        canGoFront = true;
        canGoBack = true;
        canGoLeft = true;
        canGoRight = true;
        frontRay = new Ray(transform.position, transform.right);
        backRay = new Ray(transform.position, -transform.right);
        leftRay = new Ray(transform.position, -transform.forward);
        rightRay = new Ray(transform.position, transform.forward);
        playerDirection = MoveDirection.None;
    }

    void Update()
    {
        InputPlayer();

        MovePlayer();
    }
    public void DrawRaysOnDebug()
    {
        Debug.DrawRay(frontRay.origin, frontRay.direction, Color.magenta);
        Debug.DrawRay(backRay.origin, backRay.direction, Color.red);
        Debug.DrawRay(leftRay.origin, leftRay.direction, Color.blue);
        Debug.DrawRay(rightRay.origin, rightRay.direction, Color.green);
    }
    public void UpdateRaysFromPlayer()
    {
        frontRay = new Ray(transform.position, transform.right);
        backRay = new Ray(transform.position, -transform.right);
        leftRay = new Ray(transform.position, -transform.forward);
        rightRay = new Ray(transform.position, transform.forward);
    }
    public void MovePlayer()
    {
        UpdateRaysFromPlayer();

        DrawRaysOnDebug();

        CheckRaycastDirection(ref frontRay, ref myHitFront, ref canGoFront);

        CheckRaycastDirection(ref backRay, ref myHitBack, ref canGoBack);
        
        CheckRaycastDirection(ref leftRay, ref myHitLeft, ref canGoLeft);
        
        CheckRaycastDirection(ref rightRay, ref myHitRight, ref canGoRight);


        if (playerDirection != MoveDirection.None)
            transform.Translate(moveVec * playerSpeed * Time.deltaTime);
    }
    public void CheckRaycastDirection(ref Ray rayDirAndOrigin, ref RaycastHit my_HitInfo, ref bool disableMove)
    {
        if (Physics.Raycast(rayDirAndOrigin, out my_HitInfo, maxDistanceRaycasts))
            disableMove = false;
        else
            disableMove = true;
    }
    public void InputPlayer()
    {
        if(Input.GetKey(KeyCode.W) && canGoBack)
            ApplyDirection(transform.position.z, -1, MoveDirection.Back);
        else if (Input.GetKey(KeyCode.S) && canGoFront)
            ApplyDirection(transform.position.z, 1, MoveDirection.Front);
        else if (Input.GetKey(KeyCode.A) && canGoLeft)
            ApplyDirection(transform.position.x, -1, MoveDirection.Left);
        else if (Input.GetKey(KeyCode.D) && canGoRight)
            ApplyDirection(transform.position.x, 1, MoveDirection.Right);
        else
        {
            moveVec = Vector3.zero;
        }

        if (moveVec == Vector3.zero)
            playerDirection = MoveDirection.None;
    }
    public void ApplyDirection(float offsetToCenter, int direction, MoveDirection newDirection)
    {
        float clamp = Mathf.Round(offsetToCenter) - offsetToCenter;
        if(newDirection == MoveDirection.Front || newDirection == MoveDirection.Back)
            moveVec = new Vector3(direction, 0, clamp);
        else
            moveVec = new Vector3(clamp, 0, direction);

        ghostModel.transform.LookAt(transform.position + moveVec);
        playerDirection = newDirection;
    }

}
