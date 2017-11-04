using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	void Start () {
        TheTubeManager.Instance.SetModeMenu();
    }

    public void StartGame () {
        ScreenManager.Instance.OpenScreen(ScreenManager.Instance.GameScreen);
        TheTubeManager.Instance.SetModeGame();
    }

    public void GameOver () {
        TheTubeManager.Instance.SetModeVoid();
        ScreenManager.Instance.OpenScreen(ScreenManager.Instance.GameOverScreen);
    }

    public void GoToMainMenu () {
        TheTubeManager.Instance.SetModeMenu();
        ScreenManager.Instance.OpenScreen(ScreenManager.Instance.TitleCard);
    }
}
