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

    public float CollectibleThreshold {
        get {
            return m_collectibleThreshold;
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
        Hud.Instance.InitGame();
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

    public void Restart () {
        ReinitLevel();
        GoToMainMenu();
    }

    public void Continue () {
        m_speedUpThreshold = 10 + m_currentLevel * m_thresholdIncreaseFactor;
        m_collectibleThreshold += 0.15f;
        m_collectibleThreshold = Mathf.Clamp(m_collectibleThreshold, 0.45f, 0.8f);
        GoToMainMenu();
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
            Hud.Instance.DisplayNewHiScore();
        }
        if (m_score >= m_speedUpThreshold) {
            SpeedUp();
        }
    }

    public void AddCollectible () {
        IncrementCurrencyValue();
    }

    #endregion


    #region PRIVATE

    PlayerController m_player;
    AudioSource m_audioSource;
    bool m_isGameOver = false;
    const string BEST_SCORE_KEY = "BestScores";
    const string CURRENCY_VALUE_KEY = "Currency";
    const string ROUNDS_PLAYED_KEY = "RoundsPlayed";

    int m_score = 0;
    int m_bestScore = 0;
    int m_currencyValue = 0;
    int m_roundsPlayed = 0;

    int m_speedUpThreshold = 10;
    int m_currentLevel = 0;
    int m_thresholdIncreaseFactor = 3;

    float m_collectibleThreshold = 0.85f;

    void Awake () {
        instance = this;
        m_player = GameObject.Find("Player").GetComponent<PlayerController>();
        m_bestScore = PlayerPrefs.GetInt(BEST_SCORE_KEY, 0);
        m_currencyValue = PlayerPrefs.GetInt(CURRENCY_VALUE_KEY, 0);
        m_roundsPlayed = PlayerPrefs.GetInt(ROUNDS_PLAYED_KEY, 0);
        m_audioSource = GetComponent<AudioSource>();
    }

    void Start () {
        m_score = 0;
        RefreshHud();
        TheTubeManager.Instance.SetModeMenu();
    }

    private void GoToMainMenu () {
        TheTubeManager.Instance.SetModeMenu();
        m_player.SetModeMenu();
        ScreenManager.Instance.OpenScreen(ScreenManager.Instance.TitleCard);
    }

    private void ReinitLevel () {
        m_currentLevel = 0;
        m_speedUpThreshold = 10;
        m_collectibleThreshold = 0.8f;
        TheTubeManager.Instance.Speed = 3.0f;
        TheTubeManager.Instance.rotationDuration = 0.4f;
        m_player.SetAnimatorSpeed(1);
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

    void IncrementCurrencyValue () {
        m_currencyValue++;
        Hud.Instance.UpdateCurrencyValue(m_currencyValue);
        PlayerPrefs.SetInt(CURRENCY_VALUE_KEY, m_currencyValue);
    }

    void RefreshHud () {
        Hud.Instance.UpdateScore(m_score);
        Hud.Instance.UpdateBestScore(m_bestScore);
        Hud.Instance.UpdateRoundsPlayed(m_roundsPlayed);
        Hud.Instance.UpdateCurrencyValue(m_currencyValue);
    }

    public void SpeedUp () {

        m_collectibleThreshold -= 0.04f;
        m_collectibleThreshold = Mathf.Clamp(m_collectibleThreshold, 0.45f, 0.8f);
        m_speedUpThreshold += 10 + m_currentLevel * m_thresholdIncreaseFactor;

        if (TheTubeManager.Instance.Speed < 8) {
            Debug.Log("Speed Up !");
            m_audioSource.Play();
            m_currentLevel++;
            TheTubeManager.Instance.ChangeSegmentsColor();
            TheTubeManager.Instance.Speed += 0.5f;
            TheTubeManager.Instance.rotationDuration = 3.0f / TheTubeManager.Instance.Speed * 0.4f;
            m_player.SetAnimatorSpeed(TheTubeManager.Instance.Speed / 3.0f);
            Hud.Instance.DisplaySpeedUp(TheTubeManager.Instance.Speed == 8.0f);
        }
    }

    #endregion
}
