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
    public static ResetAfterDie resetCameraAfterDie;

    public delegate void PassInfoUI();
    public PassInfoUI passMyDataToTheUI;

    private PlayerMovement myRefMovement;
    [SerializeField] private float timeInputDisaibleAfterSpawn;
    private float timer;

    public void Start()
    {
        myRefMovement = gameObject.GetComponent<PlayerMovement>();
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
        if (timer <= timeInputDisaibleAfterSpawn)
            timer += Time.deltaTime;
        else
            myRefMovement.inputAviable = true;

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
    public void ResetPosAfterDie()
    {
        timer = 0;
        transform.position = new Vector3(CreateMap.scaleFloorX - (CreateMap.scaleFloorX - 2), 0.2f, CreateMap.scaleFloorY - (CreateMap.scaleFloorY - 2));
    }
    public void ReciveDamage()
    {
        lifes--;

        myRefMovement.inputAviable = false;

        resetCameraAfterDie?.Invoke();

        ResetPosAfterDie();

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
