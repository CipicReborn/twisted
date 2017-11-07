using UnityEngine;
using System.Collections;

public class Segment : MonoBehaviour, IPoolable {

    #region PUBLIC

    public void Init () {
        m_floor.localPosition = new Vector3(m_floor.localPosition.x, m_yPos - 10, m_floor.localPosition.z);
        m_floor.gameObject.SetActive(false);
        m_collectible.gameObject.SetActive(false);
        m_floor.GetComponent<MeshRenderer>().material.color = TheTubeManager.Instance.GetColor();
        m_randomCollectible = Random.Range(0, 1.0f);
    }

    public void Enable () {
        m_floor.gameObject.SetActive(true);
        StartCoroutine(TweenToPosition());
    }

    public void SetColor (Color color) {
        StartCoroutine(LerpToColor(color));
    }

    public void SetSize (float size) {
        m_floor.localScale = new Vector3(m_floor.localScale.x, m_floor.localScale.y, size);
        m_floor.localPosition = new Vector3(m_floor.localPosition.x, m_floor.localPosition.y, size / 2.0f);
        m_collectible.localPosition = new Vector3(0, 0, 0.6f * size);
    }

    public float GetSize () {
        return m_floor.localScale.z;
    }

    public void SetRotation (float angle, int offset) {
        transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        m_angleOffset = offset;
    }

    public int GetOffset () {
        return m_angleOffset;
    }

    public float GetZAngle () {
        return transform.rotation.eulerAngles.z;
    }

    public void DropCollectible (float threshold) {
        Debug.Log(m_randomCollectible + " > " + threshold + " ? ");
        if (m_randomCollectible > threshold) {
            m_collectible.gameObject.SetActive(true);
            StartCoroutine(TweenDropCollectible());
        }
    }
    #endregion



    #region PRIVATE

    private Transform m_floor;
    private Transform m_collectible;
    private float m_yPos;
    private int m_angleOffset;
    private const float COLOR_TWEEN_DURATION = 1.5f;
    private const float PLACEMENT_TWEEN_DURATION = 0.6f;
    private const float COLLECTIBLE_TWEEN_DURATION = 0.6f;
    
    float m_randomCollectible = 0;

    private void Awake () {
        m_floor = transform.GetChild(0);
        m_collectible = transform.GetChild(1);
        m_yPos = m_floor.localPosition.y;
    }

    private IEnumerator TweenToPosition () {
        float startPos = m_floor.localPosition.y;
        float elapsedTime = 0;
        yield return null;
        while (elapsedTime < PLACEMENT_TWEEN_DURATION) {
            elapsedTime += Time.deltaTime;
            m_floor.localPosition = new Vector3(m_floor.localPosition.x, Mathf.Lerp(startPos, m_yPos, elapsedTime/PLACEMENT_TWEEN_DURATION), m_floor.localPosition.z);
            yield return null;
        }
        m_floor.localPosition = new Vector3(m_floor.localPosition.x, m_yPos, m_floor.localPosition.z);
    }

    private IEnumerator LerpToColor (Color targetColor) {
        Material mat = m_floor.GetComponent<MeshRenderer>().material;
        Color initialColor = mat.color;
        float elapsedTime = 0;
        yield return null;
        while (elapsedTime < COLOR_TWEEN_DURATION) {
            elapsedTime += Time.deltaTime;
            mat.color = Color.Lerp(initialColor, targetColor, elapsedTime/COLOR_TWEEN_DURATION);
            yield return null;

        }
        mat.color = targetColor;
    }

    private void OnTriggerEnter (Collider other) {
        if (other.CompareTag("Trigger")) {
            DropCollectible(GameManager.Instance.CollectibleThreshold);
        }
    }

    private IEnumerator TweenDropCollectible () {
        float startPos = 0;
        float endPos = -2.5f;
        float startScale = 0;
        float endScale = 0.4f;
        float elapsedTime = 0;
        m_collectible.localPosition = new Vector3(m_collectible.localPosition.x, startPos, m_collectible.localPosition.z);
        m_collectible.localScale = new Vector3(startScale, startScale, startScale);
        yield return null;
        while (elapsedTime < COLLECTIBLE_TWEEN_DURATION) {
            elapsedTime += Time.deltaTime;
            m_collectible.localPosition = new Vector3(m_collectible.localPosition.x, Mathf.Lerp(startPos, endPos, elapsedTime / COLLECTIBLE_TWEEN_DURATION), m_collectible.localPosition.z);
            float scale = Mathf.Lerp(startScale, endScale, elapsedTime / COLLECTIBLE_TWEEN_DURATION);
            m_collectible.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }
        m_collectible.localPosition = new Vector3(m_collectible.localPosition.x, endPos, m_collectible.localPosition.z);
        m_collectible.localScale = new Vector3(endScale, endScale, endScale);
    }

    #endregion
}
