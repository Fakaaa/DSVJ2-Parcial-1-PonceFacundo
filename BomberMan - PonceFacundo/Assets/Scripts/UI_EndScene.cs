using UnityEngine;

public class UI_EndScene : MonoBehaviour
{
    [SerializeField] public GameObject winText;
    [SerializeField] public GameObject defeatText;

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
        }
    }
}
