using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]public int enemySpeed;

    void Start()
    {
        Bomb.enemyHasBeenDamaged += EnemyDie;
    }
    private void OnDisable()
    {
        Bomb.enemyHasBeenDamaged -= EnemyDie;
    }
    void EnemyDie()
    {
        if (GameManager.Get() != null)
            GameManager.Get().DecreaseAmountEnemies();
    }
}
