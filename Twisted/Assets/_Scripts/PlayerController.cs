using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


    public void Jump () {
        //m_animator.SetBool(m_jumpParameter, true);
        TheTubeManager.Instance.GoToNextSegment();
    }


    Animator m_animator;

    private void Awake () {
        m_animator = GetComponent<Animator>();
    }
}
