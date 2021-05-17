using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public int lifes;
    [SerializeField] public float playerSpeed;
    [SerializeField] public bool isAlive;

    private Ray mySideRay;
    private Ray myFrontRay;

    private Rigidbody myBody;
    void Start()
    {
        transform.position = new Vector3(CreateMap.scaleFloorX * 0.5f, 1 , CreateMap.scaleFloorY * 0.5f);
        myBody = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        MovePlayer();
    }

    public void MovePlayer()
    {
        float xAxis = Input.GetAxisRaw("Vertical");
        float zAxis = Input.GetAxisRaw("Horizontal");
        Vector3 moveVec = new Vector3(-xAxis, 0, zAxis);
        mySideRay = new Ray(transform.position, transform.right);
        myFrontRay = new Ray(transform.position, transform.forward);

        Debug.DrawRay(mySideRay.origin, mySideRay.direction, Color.red);
        Debug.DrawRay(myFrontRay.origin, myFrontRay.direction, Color.blue);
        Debug.DrawRay(mySideRay.origin, -mySideRay.direction, Color.red);
        Debug.DrawRay(myFrontRay.origin, -myFrontRay.direction, Color.blue);

        RaycastHit hitSides;
        RaycastHit hitFront;
        if(Physics.Raycast(myFrontRay, 1,))

        transform.Translate(moveVec * playerSpeed * Time.deltaTime);
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
