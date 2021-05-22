using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] public float timeToExplode;
    [SerializeField] public float timer;
    [SerializeField] public bool isActive;
    [SerializeField] [Range(1, 8)] public int radiusExplode;
    [SerializeField] public GameObject prefabExplosion;

    public delegate void PlayerReciveDamage();
    public static PlayerReciveDamage playerHasBeenDamaged;

    public delegate void TheBombExplode();
    public static TheBombExplode bombExplode;

    private float timeTodestroyTrashObj;
    private float timerTrashObj;

    private float timeForActiveTrigger;
    private float timerToActiveCollision;

    private float timerExplosionActive;
    private float timeUntilExplosionActiveFalse;

    private bool isTrigger;
    private bool partyclesAlreadyActive;

    private bool hitLeft;
    private bool hitFront;
    private bool hitBack;
    private bool hitRight;

    private Ray leftRay;
    private Ray rightRay;
    private Ray frontRay;
    private Ray backRay;
    private RaycastHit frontHit;
    private RaycastHit backHit;
    private RaycastHit leftHit;
    private RaycastHit rightHit;

    //Funcionan como flags o bools
    bool ghostHitedRight;
    bool ghostHitedLeft;
    bool ghostHitedFront;
    bool ghostHitedBack;
    bool playerHited;
    bool wallHitedLeft;
    bool wallHitedRight;
    bool wallHitedFront;
    bool wallHitedBack;
    bool pointsGivedFront;
    bool pointsGivedBack;
    bool pointsGivedLeft;
    bool pointsGivedRight;
    public void Awake()
    {
        timerExplosionActive = 0;
        timeUntilExplosionActiveFalse = 1;
        ghostHitedRight = false;
        ghostHitedLeft = false;
        ghostHitedFront = false;
        ghostHitedBack = false;
        playerHited = false;
        wallHitedLeft = false;
        wallHitedRight = false;
        wallHitedFront = false;
        wallHitedBack = false;
        pointsGivedFront = false;
        pointsGivedBack = false;
        pointsGivedLeft = false;
        pointsGivedRight = false;

        partyclesAlreadyActive = false;
        hitLeft = false;
        hitFront = false;
        hitBack = false;
        hitRight = false;
        isTrigger = true;
        isActive = true;
        gameObject.GetComponent<SphereCollider>().isTrigger = true;
        Vector3 posCentred = new Vector3(Mathf.Round(transform.position.x), transform.position.y, Mathf.Round(transform.position.z));
        transform.position = posCentred;
        timeForActiveTrigger = 1;
        timeTodestroyTrashObj = 0.2f;
    }
    void Update()
    {
        CheckNearbyFoes();
    }
    public void CheckNearbyFoes()
    {
        if (timerToActiveCollision <= timeForActiveTrigger)
            timerToActiveCollision += Time.deltaTime;
        else
        {
            gameObject.GetComponent<SphereCollider>().isTrigger = false;
            isTrigger = false;
        }

        if (!isTrigger)
        {
            if (isActive)
            {
                if (timer <= timeToExplode)
                    timer += Time.deltaTime;
                else
                {
                    frontRay = new Ray(transform.position, transform.forward);
                    backRay = new Ray(transform.position, -transform.forward);
                    leftRay = new Ray(transform.position, -transform.right);
                    rightRay = new Ray(transform.position, transform.right);

                    DestroyWithRadius(ref frontRay, ref frontHit, ref hitFront, new Quaternion(0, 5, 1, 1),
                        ref ghostHitedFront, ref wallHitedFront, ref pointsGivedFront);

                    DestroyWithRadius(ref backRay, ref backHit, ref hitBack, new Quaternion(-1, 5, 0, 1),
                        ref ghostHitedBack, ref wallHitedBack, ref pointsGivedBack);

                    DestroyWithRadius(ref leftRay, ref leftHit, ref hitLeft, new Quaternion(1, 5, 0, 1),
                        ref ghostHitedLeft, ref wallHitedLeft, ref pointsGivedLeft);

                    DestroyWithRadius(ref rightRay, ref rightHit, ref hitRight, new Quaternion(0, -5, -1, 1),
                        ref ghostHitedRight, ref wallHitedRight, ref pointsGivedRight);

                    CenterExplosion();

                    if (timerExplosionActive <= timeUntilExplosionActiveFalse)
                        timerExplosionActive += Time.deltaTime;
                    else
                    {
                        isActive = false;
                        timer = 0;
                    }
                }
            }
            else
            {
                if (timerTrashObj <= timeTodestroyTrashObj)
                    timerTrashObj += Time.deltaTime;
                else
                {
                    bombExplode?.Invoke();
                    Destroy(gameObject);
                }
            }
        }
    }

    public void CenterExplosion()
    {
        if (!partyclesAlreadyActive)
        {
            GameObject mainExplode = Instantiate(prefabExplosion);
            mainExplode.transform.localScale = new Vector3(2, 2, 2);
            Instantiate(mainExplode, transform.position, Quaternion.identity);
            partyclesAlreadyActive = true;
        }
    }
    public void DrawRaysOnDebug()
    {
        Debug.DrawRay(frontRay.origin, frontRay.direction * radiusExplode, Color.magenta);
        Debug.DrawRay(backRay.origin, backRay.direction * radiusExplode, Color.red);
        Debug.DrawRay(leftRay.origin, leftRay.direction * radiusExplode, Color.blue);
        Debug.DrawRay(rightRay.origin, rightRay.direction * radiusExplode, Color.green);
    }

    private void KillEnemyGhost(ref RaycastHit hitInfo, ref bool hitFlagEnemy)
    {
        if (!hitFlagEnemy && hitInfo.collider.tag == "Enemy")
        {
            hitInfo.collider.gameObject.GetComponent<Enemy>().EnemyDied();
            hitFlagEnemy = true;
        }
    }

    private void KillPlayer()
    {
        playerHasBeenDamaged?.Invoke();
        playerHited = true;
    }

    private void DestroyWallsBreakable(ref RaycastHit hitInfo, ref bool hitFlagWall)
    {
        if (hitInfo.collider.tag != "Enemy" && !hitFlagWall)
        {
            Destroy(hitInfo.collider.gameObject);
            hitFlagWall = true;
        }
    }

    private void CreateParticlesWhenHit(ref Ray direction, ref RaycastHit hitInfo, Quaternion dirInstance, int distanceBetweenBombAndImpact)
    {
        if (!partyclesAlreadyActive)
            Instantiate(prefabExplosion, hitInfo.collider.gameObject.transform.position, dirInstance);

        for (int i = 1; i <= distanceBetweenBombAndImpact; i++)
        {
            if (!partyclesAlreadyActive)
                Instantiate(prefabExplosion, hitInfo.point - (direction.direction * i), dirInstance);
        }
    }

    private void CreateParticlesDefault(ref Ray direction, Quaternion dirInstance)
    {
        for (int i = 1; i <= radiusExplode; i++)
        {
            if (!partyclesAlreadyActive)
                Instantiate(prefabExplosion, transform.position + (direction.direction * i), dirInstance);
        }
    }

    private void GivePlayerPointsAfterDestroy(ref bool pointsSide)
    {
        if (GameManager.Get() != null && !pointsSide)
        {
            GameManager.Get().SetPlayerScore(50);
            pointsSide = true;
        }
    }

    public void DestroyWithRadius(ref Ray direction, ref RaycastHit hitInfo, ref bool hitThatSide,
        Quaternion dirInstance, ref bool hitFlagEnemy, ref bool hitFlagWall, ref bool pointsSide)
    {
        int distanceBetweenBombAndImpact;

        if (Physics.Raycast(direction, out hitInfo, radiusExplode))
        {
            hitThatSide = true;
            distanceBetweenBombAndImpact = (int)Vector3.Distance(transform.position, hitInfo.collider.gameObject.transform.position);

            if (hitInfo.collider.tag != "Unbreakable" && hitInfo.collider.tag != "Player")
            {

                KillEnemyGhost(ref hitInfo, ref hitFlagEnemy);

                GivePlayerPointsAfterDestroy(ref pointsSide);

                CreateParticlesWhenHit(ref direction, ref hitInfo, dirInstance, distanceBetweenBombAndImpact);

                DestroyWallsBreakable(ref hitInfo, ref hitFlagWall);

            }
            else if (hitInfo.collider.tag == "Player" && !playerHited)
                KillPlayer();
        }
        else
            CreateParticlesDefault(ref direction, dirInstance);

        DrawRaysOnDebug();
    }
}
