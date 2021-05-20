using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] public string ActualSceneLoaded;
    private static SceneLoader instance;
    public static SceneLoader Get()
    {
        return instance;
    }
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void LoadScene(string name)
    {
        ActualSceneLoaded = name;
        SceneManager.LoadScene(name);

        if (name == "InGame" && GameManager.Get() != null)
            GameManager.Get().SetPlayState(GameManager.PlayerFinalState.InGame);
        else if(name == "MainMenu" && GameManager.Get() != null)
            GameManager.Get().SetPlayState(GameManager.PlayerFinalState.InMenu);
    }
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
