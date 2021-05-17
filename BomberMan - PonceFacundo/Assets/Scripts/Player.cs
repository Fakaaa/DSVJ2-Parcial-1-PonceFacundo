using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public bool isAlive;
    [SerializeField] public int lifes;

    public void Start()
    {
        Bomb.playerHasBeenDamaged += ReciveDamage;
    }
    private void OnDisable()
    {
        Bomb.playerHasBeenDamaged -= ReciveDamage;
    }
    public void ReciveDamage()
    {
        lifes--;

        if (lifes <= 0)
        {
            lifes = 0;
            isAlive = false;
            Destroy(gameObject);
        }
    }
}
