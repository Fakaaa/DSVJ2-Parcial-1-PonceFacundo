using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] public bool isOpen;
    void Start()
    {
        isOpen = false;
    }
}
