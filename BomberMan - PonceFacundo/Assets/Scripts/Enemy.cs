using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public int enemySpeed;
    [SerializeField] public float timeUntilChangeDirection;
    [SerializeField] public GameObject refRaycastPos;

    public delegate void PlayerHurt();
    public static PlayerHurt playerDamaged;

    private Vector3 moveVec;
    [SerializeField] private Vector3 newPosition;

    private RaycastHit myHitForward;
    private Ray forwardRay;
    private Ray leftRay;
    private RaycastHit myHitLeft;
    private Ray rightRay;
    private RaycastHit myHitRight;
    private float maxDistanceRaycasts;

    [SerializeField] private float minDelayRotate;
    [SerializeField] private float maxDelayRotate;
    [SerializeField] List<Vector3> positions;

    private float delayRoation;
    void Start()
    {
        Bomb.enemyHasBeenDamaged += EnemyDie;
        moveVec = Vector3.forward;
        maxDistanceRaycasts = 0.6f;
        newPosition = transform.position + Vector3.forward;
        delayRoation = Random.Range(minDelayRotate, maxDelayRotate);
    }
    private void OnDisable()
    {
        Bomb.enemyHasBeenDamaged -= EnemyDie;
    }
    private void Update()
    {
        MoveEnemy();

        DrawRays();
    }
    void DrawRays()
    {
        Debug.DrawRay(forwardRay.origin, forwardRay.direction * maxDistanceRaycasts, Color.cyan);
        Debug.DrawRay(rightRay.origin, rightRay.direction * maxDistanceRaycasts, Color.red);
        Debug.DrawRay(leftRay.origin, leftRay.direction * maxDistanceRaycasts, Color.green);
    }
    void UpdateRays()
    {
        forwardRay = new Ray(transform.position, transform.forward);
        rightRay = new Ray(transform.position, transform.right);
        leftRay = new Ray(transform.position, -transform.right);
    }
    void EnemyDie()
    {
        if (GameManager.Get() != null)
            GameManager.Get().DecreaseAmountEnemies();
    }
    void MoveEnemy()
    {
        UpdateRays();

        CheckRaycast();

        transform.position = Vector3.MoveTowards(transform.position, newPosition, enemySpeed * Time.deltaTime);

        if (PosReached())
            CheckNewPositions();
    }
    bool PosReached()
    {
        if (transform.position == newPosition)
            return true;

        return false;
    }
    void CheckNewPositions()
    {
        int random = Random.Range(0, 100);
        if (random < 90)
        {
            if (!Physics.Raycast(forwardRay, out myHitForward, maxDistanceRaycasts))
            {
                newPosition = transform.position + transform.forward;
                transform.LookAt(newPosition);
                return;
            }
        }

        positions.Clear();

        if (!Physics.Raycast(leftRay, out myHitLeft, maxDistanceRaycasts))
        {
            positions.Add(-transform.right);
        }
        if (!Physics.Raycast(rightRay, out myHitRight, maxDistanceRaycasts))
        {
            positions.Add(transform.right);
        }

        if (positions.Count > 0)
        {
            int index = Random.Range(0, positions.Count);
            newPosition = transform.position + positions[index];
            transform.LookAt(newPosition);
        }
        else
        {
            newPosition = transform.position + -transform.forward;
            transform.LookAt(newPosition);
        }
    }
    void CheckRaycast()
    {
        if (Physics.Raycast(forwardRay, out myHitForward, maxDistanceRaycasts))
        {
            if (myHitForward.collider.tag == "Player")
                playerDamaged?.Invoke();
        }
    }
}
