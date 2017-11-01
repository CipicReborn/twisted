using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartGame () {
        ScreenManager.Instance.OpenScreen(ScreenManager.Instance.GameScreen);
    }

    public void GameOver () {
        ScreenManager.Instance.OpenScreen(ScreenManager.Instance.GameOverScreen);
    }

    public void GoToMainMenu () {
        ScreenManager.Instance.OpenScreen(ScreenManager.Instance.TitleCard);
    }
}
