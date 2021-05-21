using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public float timeUntilChangeDirection;
    [SerializeField] public GameObject refRaycastPos;

    public delegate void PlayerHurt();
    public static PlayerHurt playerDamaged;

    [SerializeField] private Vector3 newPosition;

    private RaycastHit myHitForward;
    private Ray forwardRay;
    private Ray enemyCheckRay;
    private RaycastHit iHitAnEnemy;
    private Ray leftRay;
    private RaycastHit myHitLeft;
    private Ray rightRay;
    private RaycastHit myHitRight;
    private float maxDistanceRaycasts;

    [SerializeField] List<Vector3> positions;

    private Vector3 auxPosition;

    [SerializeField] private float timerPerHit;
    private float delayAttack;

    private float enemySpeed;
    void Start()
    {
        if (GameManager.Get() != null)
            enemySpeed = GameManager.Get().GetEnemySpeed();
        maxDistanceRaycasts = 0.8f;
        newPosition = transform.position + Vector3.forward;
        auxPosition = newPosition;
        Bomb.bombHitEnemy += EnemyDied;
    }
    private void OnDisable()
    {
        Bomb.bombHitEnemy -= EnemyDied;
    }
    private void Update()
    {
        MoveEnemy();

        DrawRays();
    }
    void DrawRays()
    {
        Debug.DrawRay(forwardRay.origin, forwardRay.direction * maxDistanceRaycasts, Color.cyan);
        Debug.DrawRay(enemyCheckRay.origin, enemyCheckRay.direction * maxDistanceRaycasts, Color.magenta);
        Debug.DrawRay(rightRay.origin, rightRay.direction * maxDistanceRaycasts, Color.red);
        Debug.DrawRay(leftRay.origin, leftRay.direction * maxDistanceRaycasts, Color.green);
    }
    void UpdateRays()
    {
        forwardRay = new Ray(transform.position, transform.forward);
        enemyCheckRay = new Ray(transform.position + transform.forward, transform.forward);
        rightRay = new Ray(transform.position, transform.right);
        leftRay = new Ray(transform.position, -transform.right);
    }
    void MoveEnemy()
    {
        UpdateRays();

        CheckRaycastPlayer();

        CheckRaycastEnemy();

        transform.position = Vector3.MoveTowards(transform.position, newPosition, enemySpeed * Time.deltaTime);

        if (PosReached())
            CheckNewPositions();
    }
    public void EnemyDied()
    {
        if (GameManager.Get() != null)
        {
            GameManager.Get().DecreaseAmountEnemies();
            GameManager.Get().SetPlayerScore(100);
        }
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
    void CheckRaycastPlayer()
    {
        if (delayAttack <= timerPerHit)
            delayAttack += Time.deltaTime;
        else
        {
            if (Physics.Raycast(forwardRay, out myHitForward, maxDistanceRaycasts))
            {
                if (myHitForward.collider.tag == "Player")
                {
                    playerDamaged?.Invoke();
                    delayAttack = 0;
                }
            }
        }
    }
    void CheckRaycastEnemy()
    {
        if (Physics.Raycast(enemyCheckRay, out iHitAnEnemy, maxDistanceRaycasts))
        {
            auxPosition = newPosition;

            if (iHitAnEnemy.collider.tag == "Enemy")
            {
                Enemy enemyHit = iHitAnEnemy.collider.gameObject.GetComponent<Enemy>();

                if(enemyHit != null)
                {
                    newPosition = enemyHit.newPosition;
                    enemyHit.newPosition = auxPosition;
                }
            }
        }
    }
}
