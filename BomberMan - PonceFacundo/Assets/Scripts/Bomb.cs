using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] public float timeToExplode;
    [SerializeField] public float timer;
    [SerializeField] public bool isActive;
    [SerializeField] public float radiusExplode;
    [SerializeField] public GameObject prefabExplosion;

    public delegate void PlayerReciveDamage();
    public static PlayerReciveDamage playerHasBeenDamaged;

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

                    InstantiateExplosions();

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

    public void InstantiateExplosions()
    {
        Quaternion frontSide = new Quaternion(0, 5, 1, 1);
        for (int i = 1; i <= radiusExplode; i++)
        {
            if(!hitFront)
                Instantiate(prefabExplosion, transform.position + (transform.forward * i), frontSide);
        }

        Quaternion rightSide = new Quaternion(1, 5, 0, 1);
        for (int i = 1; i <= radiusExplode; i++)
        {
            if(!hitRight)
                Instantiate(prefabExplosion, transform.position + (transform.right * i), rightSide);
        }

        Quaternion backSide = new Quaternion(0, -5, -1, 1);
        for (int i = 1; i <= radiusExplode; i++)
        {
            if(!hitBack)
                Instantiate(prefabExplosion, transform.position + (-transform.forward * i), backSide);
        }
        
        Quaternion leftSide = new Quaternion(-1, 5, 0, 1);
        for (int i = 1; i <= radiusExplode; i++)
        {
            if(!hitLeft)
                Instantiate(prefabExplosion, transform.position + (-transform.right * i), leftSide);
        }

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
        if (Physics.Raycast(direction, out hitInfo, radiusExplode))
        {
            hitThatSide = true;
            if (hitInfo.collider.tag != "Unbreakable" && hitInfo.collider.tag != "Player")
            {
                hitInfo.collider.gameObject.SetActive(false);
                Instantiate(prefabExplosion, hitInfo.collider.gameObject.transform.position , dirInstance);
                Instantiate(prefabExplosion, hitInfo.collider.gameObject.transform.position - direction.direction, dirInstance);
            }
            else if(hitInfo.collider.tag == "Player")
            {
                playerHasBeenDamaged?.Invoke();
            }
        }
    }
}
