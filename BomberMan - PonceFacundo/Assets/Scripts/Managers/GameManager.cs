using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public bool endGame;
    public enum PlayerFinalState
    {
        Win,
        InMenu,
        InGame,
        Defeat
    }
    public PlayerFinalState playerState;

    private static GameManager instance;
    public static GameManager Get()
    {
        return instance;
    }

    public delegate void OpenDoor();
    public OpenDoor theDoorIsOpen;

    [SerializeField] private int amountEnemies;
    [SerializeField] private int maxEnemies;
    [SerializeField] public float timeGame;
    [SerializeField] public int amountBombsPlaced;
    [SerializeField][Range(1,5)] 
    public float enemySpeed;
    [Header("PLAYER SCORE")]
    [Space(30)]
    [SerializeField] public int scorePlayer;
    [SerializeField] public int enemiesKilled;
    private int auxScore;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void IncreaseAmountBombsPlacedInGame()
    {
        amountBombsPlaced++;
    }
    public int GetAmountBombsPlacedInGame()
    {
        return amountBombsPlaced;
    }
    public void SetPlayState(PlayerFinalState state)
    {
        playerState = state;
    }
    public void SetAmountEnemiesKilled()
    {
        enemiesKilled = maxEnemies - amountEnemies;
    }
    public void ResetData()
    {
        scorePlayer = 0;
        timeGame = 0;
    }
    public void SetPlayerScore(int score)
    {
        scorePlayer += score;
        auxScore = scorePlayer;
    }
    public int GetFinalScore()
    {
        return auxScore;
    }
    public float GetEnemySpeed()
    {
        return enemySpeed;
    }
    public float GetPlayerScore()
    {
        return scorePlayer;
    }
    public int GetAmountEnemiesKilled()
    {
        return enemiesKilled;
    }
    public void EndGame(ref bool playerAlive)
    {
        endGame = true;

        if (playerAlive)
            playerState = PlayerFinalState.Win;
        else
            playerState = PlayerFinalState.Defeat;

        ResetData();

        SetAmountEnemiesKilled();

        StartCoroutine(LoadEndScene());
    }
    IEnumerator LoadEndScene()
    {
        yield return new WaitForSeconds(1);
        if (SceneLoader.Get() != null)
            SceneLoader.Get().LoadScene("EndScene");
    }
    public void Update()
    {
        CalTimeGame();
    }
    void CalTimeGame()
    {
        if(playerState == PlayerFinalState.InGame)
            timeGame += Time.deltaTime;
    }
    public float GetTimeGame()
    {
        return timeGame;
    }
    public void DecreaseAmountEnemies()
    {
        amountEnemies--;
            
        if (amountEnemies <= 0)
        {
            amountEnemies = 0;
            theDoorIsOpen?.Invoke();
        }
    }
    public void IncreaseAmountEnemies()
    {
        amountEnemies += 1;
    }
    public int GetActualAmountEnemies()
    {
        return amountEnemies;
    }
    public int GetMaxAmountEnemies()
    {
        return maxEnemies;
    }
}
