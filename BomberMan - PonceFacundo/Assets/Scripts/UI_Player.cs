using UnityEngine;
using UnityEngine.UI;

public class UI_Player : MonoBehaviour
{
    [SerializeField] public Text lifesPlayer; 
    [SerializeField] public Text amountBombsCanPlace; 
    [SerializeField] public Text rangeBombsPlaced; 
    [SerializeField] public Text enemiesKilled;
    [SerializeField] public Text timerGame;

    [SerializeField] public Text scoreGame;

    [SerializeField] private Player playerRef;
    private int amountEnemiesKilled;

    private void Start()
    {
        if(playerRef != null)
            playerRef.passMyDataToTheUI += UpdateDataUI;

        if (GameManager.Get() != null)
            amountEnemiesKilled = GameManager.Get().GetMaxAmountEnemies() - GameManager.Get().GetActualAmountEnemies();
    }
    private void OnDisable()
    {
        if(playerRef != null)
            playerRef.passMyDataToTheUI -= UpdateDataUI;
    }
    private void Update()
    {
        if(GameManager.Get() != null)
            timerGame.text = GameManager.Get().GetTimeGame().ToString("F2");
    }
    void UpdateDataUI()
    {
        if(playerRef != null)
        {
            if (GameManager.Get() != null)
            {
                amountEnemiesKilled = GameManager.Get().GetMaxAmountEnemies() - GameManager.Get().GetActualAmountEnemies();
                scoreGame.text = GameManager.Get().GetPlayerScore().ToString();
            }
            enemiesKilled.text = amountEnemiesKilled.ToString();
            lifesPlayer.text = playerRef.lifes.ToString();
            amountBombsCanPlace.text = "Placed\n" + playerRef.actualAmountBombs.ToString();
            rangeBombsPlaced.text = "Range\n" + playerRef.radiusMyBombs.ToString();
        }
    }
}
