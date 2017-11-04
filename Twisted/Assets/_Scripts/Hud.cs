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
    public UnityEngine.UI.Text GameOverScreenCurrencyValue;
    public UnityEngine.UI.Text GameOverScreenScore;
    public UnityEngine.UI.Text GameOverScreenBestScore;
    


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

    #endregion



    #region PRIVATE

    private void Awake () {
        instance = this;
    }

    #endregion
}
