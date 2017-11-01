using UnityEngine;
using System.Collections;

public class Segment : MonoBehaviour, IPoolable {

    #region PUBLIC

    public void Init () {
        m_floor.localPosition = new Vector3(m_floor.localPosition.x, m_yPos + 10, m_floor.localPosition.z);
        m_floor.gameObject.SetActive(false);
    }

    public void Enable () {
        m_floor.gameObject.SetActive(true);
        StartCoroutine(TweenToPosition());
    }

    public void SetColor () {

    }

    public void SetSize (float size) {
        m_floor.localScale = new Vector3(m_floor.localScale.x, m_floor.localScale.y, size);
        m_floor.localPosition = new Vector3(m_floor.localPosition.x, m_floor.localPosition.y, size / 2.0f);
    }

    public float GetSize () {
        return m_floor.localScale.z;
    }

    #endregion



    #region PRIVATE

    private Transform m_floor;
    private float m_yPos;

    private void Awake () {
        m_floor = transform.GetChild(0);
        m_yPos = m_floor.localPosition.y;
    }

    private IEnumerator TweenToPosition () {
        while (m_yPos - m_floor.localPosition.y > 0.2f) {
            m_floor.localPosition = new Vector3(m_floor.localPosition.x, Mathf.Lerp(m_floor.localPosition.y, m_yPos, 0.2f), m_floor.localPosition.z);
            yield return null;
        }
        m_floor.localPosition = new Vector3(m_floor.localPosition.x, m_yPos, m_floor.localPosition.z);
    }

    #endregion
}
