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
        IncrementRoundsPlayed();
        PlayerPrefs.Save();
        m_isGameOver = false;
        m_score = 0;
        ScreenManager.Instance.OpenScreen(ScreenManager.Instance.GameScreen);
        TheTubeManager.Instance.SetModeGame();
        m_player.SetModeGame();
    }

    public void GameOver (string cause) {
        Debug.Log("GameOver by " + cause);
        TheTubeManager.Instance.SetModeVoid();
        m_player.SetModeMenu();
        ScreenManager.Instance.OpenScreen(ScreenManager.Instance.GameOverScreen);
        PlayerPrefs.Save();
    }

    public void GoToMainMenu () {
        TheTubeManager.Instance.SetModeMenu();
        m_player.SetModeMenu();
        ScreenManager.Instance.OpenScreen(ScreenManager.Instance.TitleCard);
    }

    public bool IsGameOver () {
        m_isGameOver = false;
        float playerPos = m_player.transform.position.z;
        float[] boundaries = TheTubeManager.Instance.GetCurrentSegmentBoundaries();
        if (playerPos < boundaries[0] || playerPos > boundaries[1]) {
            m_isGameOver = true;
            //TheTubeManager.Instance.ShowCurrentSegment();
            //Debug.Log(playerPos + " not in [" + boundaries[0] + ", " + boundaries[1] + "]");
        }
        return m_isGameOver;
    }

    public void IncrementScore () {
        m_score++;
        Hud.Instance.UpdateScore(m_score);
        if (m_score > m_bestScore) {
            SetNewBestScore(m_score);
        }
    }

    #endregion


    #region PRIVATE

    PlayerController m_player;
    bool m_isGameOver = false;
    const string BEST_SCORE_KEY = "BestScores";
    const string CURRENCY_VALUE_KEY = "Currency";
    const string ROUNDS_PLAYED_KEY = "RoundsPlayed";

    int m_score = 0;
    int m_bestScore = 0;
    int m_currencyValue = 0;
    int m_roundsPlayed = 0;

    void Awake () {
        instance = this;
        m_player = GameObject.Find("Player").GetComponent<PlayerController>();
        m_bestScore = PlayerPrefs.GetInt(BEST_SCORE_KEY, 0);
        m_currencyValue = PlayerPrefs.GetInt(CURRENCY_VALUE_KEY, 0);
        m_roundsPlayed = PlayerPrefs.GetInt(ROUNDS_PLAYED_KEY, 0);
    }

    void Start () {
        RefreshHud();
        TheTubeManager.Instance.SetModeMenu();
    }

    void SetNewBestScore (int value) {
        m_bestScore = value;
        Hud.Instance.UpdateBestScore(m_bestScore);
        PlayerPrefs.SetInt(BEST_SCORE_KEY, m_bestScore);
    }

    void IncrementRoundsPlayed () {
        m_roundsPlayed++;
        Hud.Instance.UpdateRoundsPlayed(m_roundsPlayed);
        PlayerPrefs.SetInt(ROUNDS_PLAYED_KEY, m_roundsPlayed);
    }

    void SetNewCurrencyValue (int value) {
        m_currencyValue = value;
        Hud.Instance.UpdateCurrencyValue(m_currencyValue);
        PlayerPrefs.SetInt(CURRENCY_VALUE_KEY, m_currencyValue);
    }

    void RefreshHud () {
        Hud.Instance.UpdateScore(m_score);
        Hud.Instance.UpdateBestScore(m_bestScore);
        Hud.Instance.UpdateRoundsPlayed(m_roundsPlayed);
        Hud.Instance.UpdateCurrencyValue(m_currencyValue);
    }

    #endregion
}
