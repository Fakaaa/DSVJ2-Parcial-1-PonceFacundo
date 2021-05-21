using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public bool isAlive;
    [SerializeField] public int lifes;
    [SerializeField] public int maxAmountBombs;
    [SerializeField] public int actualAmountBombs;
    [SerializeField] public int radiusMyBombs;

    [SerializeField] public GameObject prefabBomb;

    public delegate void ResetAfterDie();
    public static ResetAfterDie playerHasDie;

    public delegate void PassInfoUI();
    public PassInfoUI passMyDataToTheUI;

    public void Start()
    {
        prefabBomb.GetComponent<Bomb>().radiusExplode = radiusMyBombs;
        Bomb.playerHasBeenDamaged += ReciveDamage;
        Bomb.bombExplode += BombHasExplode;
        Enemy.playerDamaged += ReciveDamage;
        passMyDataToTheUI?.Invoke();
    }
    private void OnDisable()
    {
        Bomb.playerHasBeenDamaged -= ReciveDamage;
        Bomb.bombExplode -= BombHasExplode;
        Enemy.playerDamaged -= ReciveDamage;
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
            if (GameManager.Get() != null)
                GameManager.Get().IncreaseAmountBombsPlacedInGame();
            prefabBomb.GetComponent<Bomb>().radiusExplode = radiusMyBombs;
            Instantiate(prefabBomb, transform.position, Quaternion.identity);
            passMyDataToTheUI?.Invoke();
        }
    }

    public void BombHasExplode()
    {
        actualAmountBombs--;
        passMyDataToTheUI?.Invoke();
    }

    public void ReciveDamage()
    {
        lifes--;

        playerHasDie?.Invoke();
        transform.position = new Vector3(CreateMap.scaleFloorX - (CreateMap.scaleFloorX - 2), 0.2f, CreateMap.scaleFloorY - (CreateMap.scaleFloorY - 2));
        passMyDataToTheUI?.Invoke();

        if (lifes <= 0)
        {
            lifes = 0;
            isAlive = false;
            if (GameManager.Get() != null)
                GameManager.Get().EndGame(ref isAlive);
            Destroy(gameObject);
        }
    }
}
