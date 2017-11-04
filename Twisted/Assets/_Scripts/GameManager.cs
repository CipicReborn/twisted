using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    #region STATIC

    public static GameManager Instance {
        get {
            return instance;
        }
    }

    private static GameManager instance;

    #endregion

    #region PUBLIC METHODS

    public void StartGame () {
        ScreenManager.Instance.OpenScreen(ScreenManager.Instance.GameScreen);
        TheTubeManager.Instance.SetModeGame();
        m_player.SetModeGame();
    }

    public void GameOver () {
        TheTubeManager.Instance.SetModeVoid();
        m_player.SetModeMenu();
        ScreenManager.Instance.OpenScreen(ScreenManager.Instance.GameOverScreen);
    }

    public void GoToMainMenu () {
        TheTubeManager.Instance.SetModeMenu();
        m_player.SetModeMenu();
        ScreenManager.Instance.OpenScreen(ScreenManager.Instance.TitleCard);
    }

    public bool IsGameOver () {
        return false;
    }

    #endregion

    PlayerController m_player;

    void Awake () {
        instance = this;
        m_player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void Start () {
        TheTubeManager.Instance.SetModeMenu();
    }
}
