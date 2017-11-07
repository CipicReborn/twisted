using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {

    public void Disappear () {
        m_audioSource.Play();
        StartCoroutine(TweenDisappear());
    }

    private const float COLLECTIBLE_TWEEN_DURATION = 0.1f;
    private AudioSource m_audioSource;

    private void Awake () {
        m_audioSource = GetComponent<AudioSource>();
    }

    IEnumerator TweenDisappear () {
        float startScale = 0.4f;
        float endScale = 0.0f;
        float elapsedTime = 0;
        
        transform.localScale = new Vector3(startScale, startScale, startScale);
        yield return null;
        while (elapsedTime < COLLECTIBLE_TWEEN_DURATION) {
            elapsedTime += Time.deltaTime;
            float scale = Mathf.Lerp(startScale, endScale, elapsedTime / COLLECTIBLE_TWEEN_DURATION);
            transform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }
        transform.localScale = new Vector3(endScale, endScale, endScale);
    }
}
