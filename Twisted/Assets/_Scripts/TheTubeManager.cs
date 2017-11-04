using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheTubeManager : MonoBehaviour {

    #region STATIC

    public static TheTubeManager Instance {
        get {
            return instance;
        }
    }

    private static TheTubeManager instance;

    #endregion


    #region PUBLIC

    public int TubeLength;
    public float Speed = 0;
    public float initialSegmentsApparitionInterval = 0;
    public float normalSegmentsApparitionInterval = 0;

    public void SetModeStartGame () {
        StartCoroutine(AddFirstSegments());
        m_DoAction = DoActionRun;
    }

    public void SetModeRunGame () {
        m_addNewSegments = StartCoroutine(EvaluateNewSegmentsNeed());
    }

    public void SetModeMenu () {
        StopCoroutine(m_addNewSegments);
    }

    #endregion


    #region PRIVATE

    System.Action m_DoAction;
    Coroutine m_addNewSegments;

    Queue<Segment> m_segments;
    Segment m_lastSegmentAdded;
    int m_lastSegmentAngleIndex = 0;
    float m_segmentsClearanceZBoundary = 0;
    
    private readonly float[] ANGLES = { 0, 45.0f, 90.0f, 135.0f, 180.0f, 225.0f, 270.0f, 315.0f };

    private void Awake () {
        instance = this;
        m_DoAction = DoActionVoid;
        m_segments = new Queue<Segment>();
        m_segmentsClearanceZBoundary = Camera.main.transform.position.z;
    }

    private void Update () {
        m_DoAction();
	}

    private void DoActionVoid () {}

    private void DoActionRun () {
        Move();
        ClearPassedSegments();
    }

    private void Move () {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - Speed * Time.deltaTime);
    }

    private void ClearPassedSegments () {
        while (m_segments.Peek().transform.position.z < m_segmentsClearanceZBoundary) {
            SegmentsPool.PutBackToPool(m_segments.Dequeue());
        }
    }

    private IEnumerator AddFirstSegments () {
        AddFirstSegment();
        for (int i = 1; i <= TubeLength; i++) {
            AddNewSegment();
            yield return new WaitForSeconds(initialSegmentsApparitionInterval);
        }
        SetModeRunGame();
    }

    private IEnumerator EvaluateNewSegmentsNeed () {
        while (true) {
            if (m_segments.Count < TubeLength) {
                AddNewSegment();
            }
            yield return new WaitForSeconds(normalSegmentsApparitionInterval);
        }
    }

    private void AddFirstSegment () {
        var firstSegment = SegmentsPool.GetSegment();
        firstSegment.transform.SetParent(transform);
        SetPosition(firstSegment, 0);
        SetRotation(firstSegment, 0);
        SetScale(firstSegment, 6);
        firstSegment.Enable();
        m_segments.Enqueue(firstSegment);
        m_lastSegmentAdded = firstSegment;
    }

    private void AddNewSegment () {
        var newSegment = SegmentsPool.GetSegment();
        SetupTransform(newSegment);
        newSegment.Enable();
        m_segments.Enqueue(newSegment);
        m_lastSegmentAdded = newSegment;
    }

    private void SetupTransform (Segment segment) {
        segment.transform.SetParent(transform);
        SetPosition(segment);
        SetRotation(segment);
        SetScale(segment);
    }

    private void SetPosition (Segment segment, float pos = -1) {
        if (pos == -1) {
            pos = m_lastSegmentAdded.transform.localPosition.z + m_lastSegmentAdded.GetSize() + 0.5f;
        }
        segment.transform.localPosition = new Vector3(0, 0, pos);
    }

    private void SetRotation (Segment segment, int index = -1) {
        if (index == -1) {
            int prevIndex = m_lastSegmentAngleIndex;
            int random = Random.Range(0, 2);
            int offset = 1;
            if (random == 0) {
                offset = -1;
            }
            index = prevIndex + offset;
            LoopValue(ref index, 0, ANGLES.Length - 1);
        }
        float angle = ANGLES[index];
        m_lastSegmentAngleIndex = index;
        segment.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Debug.Log("Set Rotation to <" + angle.ToString() + ">");
    }

    void LoopValue (ref int value, int min, int max) {
        if (value == max + 1) {
            value = min;
        }
        else if (value == min - 1) {
            value = max;
        }
    }

    private void SetScale (Segment segment, int size = 0) {
        if (size == 0) {
            size = Random.Range(2, 4);
        }
        segment.SetSize(size);
    }

    #endregion
}
