using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public int lifes;
    [SerializeField] public float playerSpeed;
    [SerializeField] public bool isAlive;

    private Vector3 moveVec;
    private Ray frontRay;
    private RaycastHit myHitFront;
    private Ray backRay;
    private RaycastHit myHitBack;
    private Ray leftRay;
    private RaycastHit myHitLeft;
    private Ray rightRay;
    private RaycastHit myHitRight;

    enum MoveDirection
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

    void Start()
    {
        maxDistanceRaycasts = 0.8f;
        transform.position = new Vector3(CreateMap.scaleFloorX * 0.5f, 0.8f, CreateMap.scaleFloorY * 0.5f);
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
        {
            if (my_HitInfo.collider.tag == "Unbreakable" || my_HitInfo.collider.tag == "Breakable")
            {
                disableMove = false;
            }
        }
        else
            disableMove = true;
    }
    public void InputPlayer()
    {
        if(Input.GetKey(KeyCode.W) && canGoBack)
        {
            moveVec = Vector3.left;
            playerDirection = MoveDirection.Back;
        }
        else if (Input.GetKey(KeyCode.S) && canGoFront)
        {
            moveVec = Vector3.right;
            playerDirection = MoveDirection.Front;
        }
        else if (Input.GetKey(KeyCode.A) && canGoLeft)
        {
            moveVec = Vector3.back;
            playerDirection = MoveDirection.Left;
        }
        else if (Input.GetKey(KeyCode.D) && canGoRight)
        {
            moveVec = Vector3.forward;
            playerDirection = MoveDirection.Right;
        }
        else
            moveVec = Vector3.zero;

        if(moveVec == Vector3.zero)
            playerDirection = MoveDirection.None;
    }
    public void ReciveDamage()
    {
        lifes--;

        if(lifes < 0)
        {
            lifes = 0;
            isAlive = false;
        }
    }
}
