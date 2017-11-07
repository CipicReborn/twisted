using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hud : MonoBehaviour {

    #region STATIC

    public static Hud Instance {
        get {
            return instance;
        }
    }

    private static Hud instance;

    #endregion


    #region PUBLIC

    public UnityEngine.UI.Text TitleCardRoundsPlayed;
    public UnityEngine.UI.Text TitleCardBestScore;
    public UnityEngine.UI.Text TitleCardCurrencyValue;
    public UnityEngine.UI.Text GameScreenScore;
    public UnityEngine.UI.Text GameScreenNewHiScore;
    public UnityEngine.UI.Text GameScreenSpeedUp;
    public UnityEngine.UI.Text GameOverScreenCurrencyValue;
    public UnityEngine.UI.Text GameOverScreenScore;
    public UnityEngine.UI.Text GameOverScreenBestScore;

    public Animator GameScreen;

    public void InitGame () {
        GameScreenScore.text = "0";
        GameScreenNewHiScore.text = "";
        GameScreenSpeedUp.text = "SPEED UP !";
        GameScreenSpeedUp.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "SPEED UP !";
    }

    public void UpdateScore (int value) {
        GameScreenScore.text = value.ToString();
        GameOverScreenScore.text = value.ToString();
    }

    public void UpdateCurrencyValue (int value) {
        TitleCardCurrencyValue.text = "x" + value.ToString();
        GameOverScreenCurrencyValue.text = "x" + value.ToString();
    }

    public void UpdateBestScore (int value) {
        TitleCardBestScore.text = "BEST SCORE: " + value.ToString();
        GameOverScreenBestScore.text = value.ToString();
    }

    public void UpdateRoundsPlayed (int value) {
        TitleCardRoundsPlayed.text = "ROUNDS PLAYED: " + value.ToString();
    }

    public void DisplaySpeedUp (bool maxSpeed) {
        if (maxSpeed) {
            GameScreenSpeedUp.text = "MAX SPEED !";
            GameScreenSpeedUp.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "MAX SPEED !";
        }
        GameScreen.SetTrigger(m_speedUpTrigger);
    }

    public void DisplayNewHiScore () {
        if (GameScreenNewHiScore.text == "") {
            GameScreenNewHiScore.text = NEW_HI_SCORE;
            StartCoroutine(ScaleHiScore());
        }
    }

    #endregion



    #region PRIVATE

    private const string NEW_HI_SCORE = "NEW HI SCORE";
    private const string SPEED_UP_TRIGGER_NAME = "SpeedUp";
    private int m_speedUpTrigger;

    private const float HISCORE_TWEEN_DURATION = 0.5f;

    private void Awake () {
        instance = this;
        m_speedUpTrigger = Animator.StringToHash(SPEED_UP_TRIGGER_NAME);
    }

    private IEnumerator ScaleHiScore () {
        Transform hiScore = GameScreenNewHiScore.transform;
        float elapsedTime = 0;
        float scale = 5;
        yield return null;
        while (elapsedTime < HISCORE_TWEEN_DURATION) {
            elapsedTime += Time.deltaTime;
            scale = Mathf.Lerp(5, 1, elapsedTime / HISCORE_TWEEN_DURATION);
            hiScore.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }
        hiScore.localScale = Vector3.one;
    }

    #endregion
}
