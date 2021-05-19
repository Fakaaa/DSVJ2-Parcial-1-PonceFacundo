using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] public bool isOpen;

    void Start()
    {
        GameManager.Get().theDoorIsOpen += SetIsOpen;
        isOpen = false;
    }
    private void OnDisable()
    {
        GameManager.Get().theDoorIsOpen -= SetIsOpen;
    }
    public void SetIsOpen()
    {
        isOpen = true;
    }
}
