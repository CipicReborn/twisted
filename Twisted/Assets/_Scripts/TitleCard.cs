using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCard : MonoBehaviour {

    private const float PULSE_DURATION = 1.0f;
    public UnityEngine.UI.Text m_callToAction;
    float m_grey = 50.0f / 255.0f;

    //Coroutine m_coroutine;

    //private void OnEnable () {
    //    m_coroutine = StartCoroutine(LerpAlphaCallToAction());
    //}

    //private void OnDisable () {
    //    StopCoroutine(m_coroutine);
    //}

    //IEnumerator LerpAlphaCallToAction () {
    //    while (true) {
    //        
    //        m_callToAction.color = new Color(m_grey, m_grey, m_grey, a);
    //        Debug.Log("alpha set to " + a);
    //        yield return null;    
    //    }
    //}


    private void Update () {
        float a = Mathf.PingPong(Time.time, 1);
        m_callToAction.color = new Color(m_grey, m_grey, m_grey, a);
    }
}
