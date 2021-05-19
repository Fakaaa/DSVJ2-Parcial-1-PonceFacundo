using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] public float timeToExplode;
    [SerializeField] public float timer;
    [SerializeField] public bool isActive;
    [SerializeField][Range(1,8)] public int radiusExplode;
    [SerializeField] public GameObject prefabExplosion;

    public delegate void PlayerReciveDamage();
    public static PlayerReciveDamage playerHasBeenDamaged;

    public delegate void EnemyHasDie();
    public static EnemyHasDie enemyHasBeenDamaged;

    public delegate void TheBombExplode();
    public static TheBombExplode bombExplode;

    private float timeTodestroyTrashObj;
    private float timerTrashObj;

    private float timeForActiveTrigger;
    private float timerToActiveCollision;

    private bool isTrigger;

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
    public void Awake()
    {
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

                    DrawRaysOnDebug();

                    DestroyWithRadius(ref frontRay, ref frontHit, ref hitFront, new Quaternion(0, 5, 1, 1));
                    DestroyWithRadius(ref backRay, ref backHit, ref hitBack, new Quaternion(-1, 5, 0, 1));
                    DestroyWithRadius(ref leftRay, ref leftHit, ref hitLeft, new Quaternion(1, 5, 0, 1));
                    DestroyWithRadius(ref rightRay, ref rightHit, ref hitRight, new Quaternion(0, -5, -1, 1));
                    CenterExplosion();

                    isActive = false;
                    timer = 0;
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
        GameObject mainExplode = Instantiate(prefabExplosion);
        mainExplode.transform.localScale = new Vector3(2, 2, 2);
        Instantiate(mainExplode, transform.position, Quaternion.identity);
    }
    public void DrawRaysOnDebug()
    {
        Debug.DrawRay(frontRay.origin, frontRay.direction, Color.magenta);
        Debug.DrawRay(backRay.origin, backRay.direction, Color.red);
        Debug.DrawRay(leftRay.origin, leftRay.direction, Color.blue);
        Debug.DrawRay(rightRay.origin, rightRay.direction, Color.green);
    }
    public void DestroyWithRadius(ref Ray direction, ref RaycastHit hitInfo, ref bool hitThatSide, Quaternion dirInstance)
    {
        int distanceBetweenBombAndImpact;

        if (Physics.Raycast(direction, out hitInfo, radiusExplode))
        {
            hitThatSide = true;
            distanceBetweenBombAndImpact = (int)Vector3.Distance(transform.position, hitInfo.collider.gameObject.transform.position);
            if (hitInfo.collider.tag != "Unbreakable" && hitInfo.collider.tag != "Player")
            {
                hitInfo.collider.gameObject.SetActive(false);
                Instantiate(prefabExplosion, hitInfo.collider.gameObject.transform.position , dirInstance);

                for (int i = 1; i <= distanceBetweenBombAndImpact; i++)
                {
                    Instantiate(prefabExplosion, hitInfo.collider.gameObject.transform.position - (direction.direction * i), dirInstance);
                }
                if (hitInfo.collider.tag == "Enemy")
                    enemyHasBeenDamaged?.Invoke();
            }
            else if(hitInfo.collider.tag == "Player")
                playerHasBeenDamaged?.Invoke();
        }
        else
        {
            for (int i = 1; i <= radiusExplode; i++)
            {
                Instantiate(prefabExplosion, transform.position + (direction.direction * i), dirInstance);
            }
        }
    }
}
