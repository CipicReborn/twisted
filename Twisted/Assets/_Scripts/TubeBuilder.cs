using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeBuilder: MonoBehaviour {

    #region PUBLIC

    public int TubeLength;

    public float initialSegmentsApparitionInterval = 0;
    public float normalSegmentsApparitionInterval = 0;

    public Segment GenerateInitialSegments () {
        StartCoroutine(AddFirstSegments());
        return m_firstSegment;
    }

    public void GenerateAdditionnalSegments () {
        m_addNewSegments = StartCoroutine(EvaluateNewSegmentsNeed());
    }

    public void StopGeneratingSegments () {
        if (m_addNewSegments != null) {
            StopCoroutine(m_addNewSegments);
        }
    }

    #endregion


    #region PRIVATE

    private readonly float[] ANGLES = { 0, 45.0f, 90.0f, 135.0f, 180.0f, 225.0f, 270.0f, 315.0f };

    Segment m_firstSegment;
    Segment m_lastSegmentAdded;
    int m_lastSegmentAngleIndex = 0;

    Coroutine m_addNewSegments;

    private IEnumerator AddFirstSegments () {
        m_firstSegment = AddFirstSegment();
        for (int i = 1; i <= TubeLength; i++) {
            AddNewSegment();
            yield return new WaitForSeconds(initialSegmentsApparitionInterval);
        }
    }

    private IEnumerator EvaluateNewSegmentsNeed () {
        while (true) {
            if (transform.childCount < TubeLength) {
                AddNewSegment();
            }
            yield return new WaitForSeconds(normalSegmentsApparitionInterval);
        }
    }

    private Segment AddFirstSegment () {
        Debug.Log(SegmentsPool.Instance);
        var firstSegment = SegmentsPool.Instance.GetSegment();
        firstSegment.transform.SetParent(transform);
        SetPosition(firstSegment, 0);
        SetRotation(firstSegment, 0);
        SetScale(firstSegment, 6);
        firstSegment.Enable();
        m_lastSegmentAdded = firstSegment;
        return firstSegment;
    }

    private void AddNewSegment () {
        var newSegment = SegmentsPool.Instance.GetSegment();
        SetupTransform(newSegment);
        newSegment.Enable();
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
        int offset = 0;
        if (index == -1) {
            int prevIndex = m_lastSegmentAngleIndex;
            int random = Random.Range(0, 2);
            offset = 1;
            if (random == 0) {
                offset = -1;
            }
            index = prevIndex + offset;
            LoopValue(ref index, 0, ANGLES.Length - 1);
        }
        float angle = ANGLES[index];
        m_lastSegmentAngleIndex = index;
        segment.SetRotation(angle, index, offset);
    }

    private void LoopValue (ref int value, int min, int max) {
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
