using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public bool endGame;

    public enum PlayerFinalState
    {
        Win,
        InGame,
        Defeat
    }
    public PlayerFinalState playerState;

    private static GameManager instance;
    public static GameManager Get()
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

    public void EndGame(PlayerFinalState state)
    {
        endGame = true;
        switch (state)
        {
            case PlayerFinalState.Win:

                break;
            case PlayerFinalState.Defeat:

                break;
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
