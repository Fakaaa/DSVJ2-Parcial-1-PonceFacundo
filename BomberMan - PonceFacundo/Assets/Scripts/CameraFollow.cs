using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] public Transform lookAtThat;
    [SerializeField] public Transform moveToThisPos;

    [SerializeField][Range(4,8)] public float zoomUp;
    [SerializeField][Range(5,10)] public float zoomDistance;
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

        posToMoveTowards = moveToThisPos.position + zoom;

        transform.LookAt(lookAtThat, lookAtThat.up);

        transform.position = Vector3.Lerp(myPos, posToMoveTowards, Vector3.Distance(myPos, posToMoveTowards));
    }
}
