using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public bool isAlive;
    [SerializeField] public int lifes;
    [SerializeField] public int maxAmountBombs;
    [SerializeField] public int actualAmountBombs;

    [SerializeField] public GameObject prefabBomb;


    public void Start()
    {
        Bomb.playerHasBeenDamaged += ReciveDamage;
        Bomb.bombExplode += BombHasExplode;
    }
    private void OnDisable()
    {
        Bomb.playerHasBeenDamaged -= ReciveDamage;
        Bomb.bombExplode -= BombHasExplode;
    }
    public void Update()
    {
        PlaceBomb();
    }
    public void PlaceBomb()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0)) && actualAmountBombs < maxAmountBombs)
        {
            actualAmountBombs++;
            Instantiate(prefabBomb, transform.position, Quaternion.identity);
        }
    }

    public void BombHasExplode()
    {
        actualAmountBombs--;
    }

    public void ReciveDamage()
    {
        lifes--;


        if (lifes <= 0)
        {
            lifes = 0;
            isAlive = false;
            if (GameManager.Get() != null)
                GameManager.Get().EndGame(GameManager.PlayerFinalState.Defeat);
            Destroy(gameObject);
        }
    }
}
