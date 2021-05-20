using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    [SerializeField] public PowerUpWall prefabObject;
    [Header("PORCENT TO SPAWN POWER UP")]
    [SerializeField][Range(0, 100)] public float value;

    private float randomActivatePowerUp;
    private void Start()
    {
        randomActivatePowerUp = Random.Range(0, 100);

        if (randomActivatePowerUp <= value)
            Instantiate(prefabObject.gameObject, transform.position, Quaternion.identity);
        else
            return;
    }
}
