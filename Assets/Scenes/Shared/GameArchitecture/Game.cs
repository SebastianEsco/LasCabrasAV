using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{

    #region Singleton
    private static Game instance;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void CreateGame()
    {
        GameObject gameGo = new GameObject("[GAME]");
        instance = gameGo.AddComponent<Game>();
        DontDestroyOnLoad(gameGo);


    }

    public static Game Instance
    {
        get
        {
            if (instance == null)
            {
                CreateGame();
            }
            return instance;
        }
    }
    #endregion

    private CharacterState playerOne;
    private void CreatePlayer()
    {
        GameObject playerGo = new GameObject("[PLAYER 1]");
        playerOne = playerGo.AddComponent<CharacterState>();
        DontDestroyOnLoad (playerGo);
    }


    private void Awake()
    {
        CreatePlayer();
    }
    public CharacterState PlayerOne => playerOne;
}
