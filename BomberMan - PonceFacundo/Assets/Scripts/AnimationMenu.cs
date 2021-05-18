using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationMenu : MonoBehaviour
{
    [System.Serializable]
    public enum TypeAnim
    {
        BarrelRolllllX,
        BarrelRolllllY,
        BarrelRolllllZ,
    }
    public TypeAnim myAnim;

    [SerializeField] public float speedRotation;

    void Update()
    {
        switch (myAnim)
        {
            case TypeAnim.BarrelRolllllX:
                DoBarrelRollll(Vector3.right);
                break;
            case TypeAnim.BarrelRolllllY:
                DoBarrelRollll(Vector3.up);
                break;
            case TypeAnim.BarrelRolllllZ:
                DoBarrelRollll(Vector3.forward);
                break;
        }
    }
    public void DoBarrelRollll(Vector3 direction)
    {
        gameObject.transform.Rotate(direction, speedRotation * Time.deltaTime, Space.World);
    }
}
