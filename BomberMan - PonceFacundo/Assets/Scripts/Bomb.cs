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

                    Quaternion leftSide = new Quaternion(-1,0,0,1);
                    Quaternion rightSide = new Quaternion(1, 0, 0, 1);
                    Quaternion frontSide = new Quaternion(0, 0, 1, 1);
                    Quaternion backSide = new Quaternion(0, 0,-1, 1);
                    Instantiate(prefabExplosion, transform.position + (transform.forward / 3.5f), frontSide);
                    Instantiate(prefabExplosion, transform.position + (-transform.forward / 3.5f), backSide);
                    Instantiate(prefabExplosion, transform.position + (transform.right / 3.5f), rightSide);
                    Instantiate(prefabExplosion, transform.position + (-transform.right / 3.5f), leftSide);

                    DestroyWithRadius(ref frontRay, ref frontHit);
                    DestroyWithRadius(ref backRay, ref backHit);
                    DestroyWithRadius(ref leftRay, ref leftHit);
                    DestroyWithRadius(ref rightRay, ref rightHit);

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
    public void DrawRaysOnDebug()
    {
        Debug.DrawRay(frontRay.origin, frontRay.direction, Color.magenta);
        Debug.DrawRay(backRay.origin, backRay.direction, Color.red);
        Debug.DrawRay(leftRay.origin, leftRay.direction, Color.blue);
        Debug.DrawRay(rightRay.origin, rightRay.direction, Color.green);
    }
    public void DestroyWithRadius(ref Ray direction, ref RaycastHit hitInfo)
    {
        if (Physics.Raycast(direction, out hitInfo, radiusExplode))
        {
            if (hitInfo.collider.tag != "Unbreakable" && hitInfo.collider.tag != "Player")
            {
                hitInfo.collider.gameObject.SetActive(false);
                if (hitInfo.collider.tag != "Breakable")
                {

                }
            }
            else if(hitInfo.collider.tag == "Player")
            {
                playerHasBeenDamaged?.Invoke();
            }
        }
    }
}
