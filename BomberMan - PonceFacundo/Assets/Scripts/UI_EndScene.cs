using UnityEngine;
using UnityEngine.UI;

public class UI_EndScene : MonoBehaviour
{
    [SerializeField] public GameObject winText;
    [SerializeField] public GameObject defeatText;
    [SerializeField] public Text amountBombsInGame;
    [SerializeField] public Text amountGhostsSlayed;
    [SerializeField] public Text finalScorePlayer;

    public bool infoUpdate;
    public void Start()
    {
        infoUpdate = false;
    }
    void Update()
    {
        if(GameManager.Get() != null && !infoUpdate)
        {
            switch (GameManager.Get().playerState)
            {
                case GameManager.PlayerFinalState.Win:
                    winText.SetActive(true);
                    defeatText.SetActive(false);
                    break;
                case GameManager.PlayerFinalState.Defeat:
                    defeatText.SetActive(true);
                    winText.SetActive(false);
                    break;
            }
            infoUpdate = true;

            amountBombsInGame.text = "Amount Bombs placed\n" + GameManager.Get().GetAmountBombsPlacedInGame();
            amountGhostsSlayed.text = "Amount Ghosts Killed\n" + GameManager.Get().GetAmountEnemiesKilled();
            finalScorePlayer.text = "FINAL SCORE\n" + GameManager.Get().GetFinalScore();
        }
    }
}
