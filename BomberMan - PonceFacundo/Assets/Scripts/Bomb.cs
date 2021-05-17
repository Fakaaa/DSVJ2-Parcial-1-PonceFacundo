using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] public float timeToExplode;
    [SerializeField] public float timer;
    [SerializeField] public bool isActive;
    [SerializeField] public float radiusExplode;

    private float timeTodestroyTrashObj;
    private float timerTrashObj;

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
        timeTodestroyTrashObj = 1;
        isActive = true;
    }
    void Update()
    {
        CheckNearbyFoes();
    }
    public void CheckNearbyFoes()
    {
        if(isActive)
        {
            if(timer <= timeToExplode)
                timer += Time.deltaTime;
            else
            {
                frontRay = new Ray(transform.position, transform.forward);
                backRay = new Ray(transform.position, -transform.forward);
                leftRay = new Ray(transform.position, -transform.right);
                rightRay = new Ray(transform.position, transform.right);
                
                DrawRaysOnDebug();

                DestroyWithRadius(ref frontRay,ref frontHit);

                DestroyWithRadius(ref backRay,ref backHit);
                
                DestroyWithRadius(ref leftRay,ref leftHit);
                
                DestroyWithRadius(ref rightRay,ref rightHit);

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
                Destroy(gameObject);
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
            if (hitInfo.collider.tag != "Unbreakable")
            {
                Destroy(hitInfo.collider.gameObject);
            }
        }
    }
}
