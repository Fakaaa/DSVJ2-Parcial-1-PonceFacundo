using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public bool endGame;
    public enum PlayerFinalState
    {
        Win,
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

    public void EndGame(ref bool playerAlive)
    {
        endGame = true;

        if (playerAlive)
            playerState = PlayerFinalState.Win;
        else
            playerState = PlayerFinalState.Defeat;

        StartCoroutine(LoadEndScene());
    }
    IEnumerator LoadEndScene()
    {
        yield return new WaitForSeconds(1);
        if (SceneLoader.Get() != null)
            SceneLoader.Get().LoadScene("EndScene");
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
