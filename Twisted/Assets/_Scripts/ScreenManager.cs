using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class ScreenManager : MonoBehaviour {
    
    #region STATIC
    public static ScreenManager Instance {
        get {
            return instance;
        }
    }

    private static ScreenManager instance;
    #endregion

    #region PUBLIC
    public Animator StartScreen;
    public Animator TitleCard;
    public Animator GameScreen;
    public Animator GameOverScreen;

    public void OpenScreen (Animator screenToOpen) {
        if (m_currentlyOpenScreen == screenToOpen)
            return;

        m_screenToOpen = screenToOpen;
        TransitionToNewScreen();
    }
    
    #endregion

    #region PRIVATE

    private const string OPEN_TRANSITION_NAME = "isOpen";
    private const string OPEN_STATE_NAME = "Open";
    private const string CLOSED_STATE_NAME = "Closed";

    private Animator m_currentlyOpenScreen;
    private Animator m_screenToOpen;
    private int m_isOpenParameter;

    private void Awake () {
        instance = this;
    }

    void OnEnable () {
        m_isOpenParameter = Animator.StringToHash(OPEN_TRANSITION_NAME);
        if (StartScreen == null)
            return;
        OpenScreen(StartScreen);
    }


    void TransitionToNewScreen () {
        if (m_currentlyOpenScreen != null) {
            AnimateScreenClosingAndTransition();
        }
        else {
            AnimateScreenOpening();
        }
    }

    void AnimateScreenClosingAndTransition () {
        m_currentlyOpenScreen.SetBool(m_isOpenParameter, false);
        StartCoroutine(WaitForScreenState(m_currentlyOpenScreen, CLOSED_STATE_NAME, Transition));
    }

    void Transition () {
        AnimateScreenOpening();
        FinaliseScreenClosing();
    }
    void FinaliseScreenClosing () {
        m_currentlyOpenScreen.gameObject.SetActive(false);
        m_currentlyOpenScreen = null;
    }

    void AnimateScreenOpening () {
        m_screenToOpen.gameObject.SetActive(true);
        m_screenToOpen.transform.SetAsLastSibling();
        m_screenToOpen.SetBool(m_isOpenParameter, true);
        StartCoroutine(WaitForScreenState(m_screenToOpen, OPEN_STATE_NAME, RecordScreenAsOpened));
    }

    void RecordScreenAsOpened () {
        m_currentlyOpenScreen = m_screenToOpen;
        m_screenToOpen = null;
    }

    IEnumerator WaitForScreenState (Animator screenAnimator, string targetStateName, System.Action callback) {
        bool stateReached = false;
        while (!stateReached) {
            if (!screenAnimator.IsInTransition(0)) {
                stateReached = screenAnimator.GetCurrentAnimatorStateInfo(0).IsName(targetStateName);
            }
            yield return new WaitForEndOfFrame();
        }
        callback();
    }


    #endregion
}