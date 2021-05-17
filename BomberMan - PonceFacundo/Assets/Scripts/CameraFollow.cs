using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] public Transform lookAtThat;

    [SerializeField][Range(2,8)] public float zoomUp;
    [SerializeField][Range(3,10)] public float zoomDistance;
    [SerializeField][Range(0.5f,9)] public float smothSpeed;
    private Vector3 zoom;

    private Vector3 posToMoveTowards;

    void LateUpdate()
    {
        FocusToTargetAndMove();
    }
    public void FocusToTargetAndMove()
    {
        Vector3 myPos = transform.position;

        zoom = new Vector3(zoomDistance, zoomUp, 0);

        if(lookAtThat != null)
        {
            posToMoveTowards = lookAtThat.position + zoom;

            //transform.LookAt(lookAtThat, lookAtThat.up);

            transform.position = Vector3.Lerp(myPos, posToMoveTowards, Vector3.Distance(myPos, posToMoveTowards) * Time.deltaTime * smothSpeed);
        }
    }
}
