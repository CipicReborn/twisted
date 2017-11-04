using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    #region PUBLIC METHODS

    public void SetModeGame () {
        Init();
        m_doAction = DoActionRoll;
    }

    public void SetModeMenu () {
        m_doAction = DoActionVoid;
    }

    public void Jump () {
        if (!m_isJumping) {
            if (m_jumpsToDo > 0) {
                m_jumpsToDo--;
            }
            StartCoroutine(DoJump());
            StartCoroutine(WaitForState(IDLE_STATE, SetLanded));
        }
        else {
            m_jumpsToDo++;
        }
    }

    #endregion


    #region PRIVATE

    const string JUMP_PARAMETER_NAME = "startJump";
    const string IDLE_STATE = "Idle";
    const string JUMP_STATE = "Jump";

    System.Action m_doAction;

    Animator m_animator;
    Transform m_graphicAsset;
    int m_jumpParameterId = 0;
    float m_perimeter = 0;
    bool m_isJumping = false;
    int m_jumpsToDo = 0;

    private void Awake () {
        m_animator = GetComponent<Animator>();
        m_jumpParameterId = Animator.StringToHash(JUMP_PARAMETER_NAME);
        m_graphicAsset = transform.GetChild(0);
        m_perimeter = 2.0f * Mathf.PI * (m_graphicAsset.localScale.x / 2.0f);

        Init();
    }

    private void Init () {
        m_doAction = DoActionVoid;
        m_isJumping = false;
        m_animator.SetBool(m_jumpParameterId, false);
        m_jumpsToDo = 0;
    }

    private void Update () {
        m_doAction();
    }

    private void DoActionVoid () { }

    private void DoActionRoll () {
        float angularSpeed = TheTubeManager.Instance.Speed * 360.0f / m_perimeter;
        m_graphicAsset.Rotate(transform.rotation.eulerAngles + new Vector3(angularSpeed * Time.deltaTime, 0, 0));
    }

    private IEnumerator DoJump () {
        m_isJumping = true;
        m_animator.SetBool(m_jumpParameterId, true);
        TheTubeManager.Instance.GoToNextSegment();
        yield return null;
        m_animator.SetBool(m_jumpParameterId, false);
    }

    private IEnumerator WaitForState (string targetStateName, System.Action callback) {
        yield return null;
        bool stateReached = false;
        while (!stateReached) {
            if (!m_animator.IsInTransition(0)) {
                stateReached = m_animator.GetCurrentAnimatorStateInfo(0).IsName(targetStateName);
            }
            yield return new WaitForEndOfFrame();
        }
        yield return null;
        callback();
    }

    private void SetLanded () {
        m_isJumping = false;
        if (GameManager.Instance.IsGameOver()) {
            return;
        }
        if (m_jumpsToDo > 0) {
            Jump();
        }
    }

    #endregion

}
