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

    public void SetModeGame () {
        m_DoAction = DoActionRun;
        StartCoroutine(AddFirstSegments());
    }

    public void SetModeMenu () {
        StopCoroutine(m_addNewSegments);
    }

    #endregion


    #region PRIVATE

    System.Action m_DoAction;
    Queue<Segment> m_segments;
    float m_cameraPositionZ = 0;
    Segment lastSegmentAdded;
    Coroutine m_addNewSegments;
    private const float ANGLE_SPREAD = 45;

    private void Awake () {
        instance = this;
        m_DoAction = DoActionVoid;
        m_segments = new Queue<Segment>();
        m_cameraPositionZ = Camera.main.transform.position.z;
    }

    private void Start () {
        
	}

    private void Update () {
        m_DoAction();
	}

    private void DoActionVoid () {}

    private void DoActionRun () {

        Move();
        ClearPassedSegments();
        EvaluateNewSegmentsNeed();

    }

    private void Move () {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - Speed * Time.deltaTime);
    }

    private void ClearPassedSegments () {
        
        while (m_segments.Peek().transform.position.z < m_cameraPositionZ) {
            SegmentsPool.PutBackToPool(m_segments.Dequeue());
        }
    }

    private IEnumerator AddFirstSegments () {
        AddFirstSegment();
        for (int i = 1; i <= TubeLength; i++) {
            AddNewSegment();
            yield return new WaitForSeconds(0.2f);
        }
        m_addNewSegments = StartCoroutine(EvaluateNewSegmentsNeed());
    }

    private IEnumerator EvaluateNewSegmentsNeed () {
        while (true) {
            if (m_segments.Count < TubeLength) {
                AddNewSegment();
            }
            yield return new WaitForSeconds(1);
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
        lastSegmentAdded = firstSegment;
    }

    private void AddNewSegment () {
        var newSegment = SegmentsPool.GetSegment();
        SetupTransform(newSegment);
        newSegment.Enable();
        m_segments.Enqueue(newSegment);
        lastSegmentAdded = newSegment;
    }

    private void SetupTransform (Segment segment) {
        segment.transform.SetParent(transform);
        SetPosition(segment);
        SetRotation(segment);
        SetScale(segment);
    }

    private void SetPosition (Segment segment, float pos = -1) {
        if (pos == -1) {
            pos = lastSegmentAdded.transform.localPosition.z + lastSegmentAdded.GetSize() + 0.5f;
        }
        segment.transform.localPosition = new Vector3(0, 0, pos);
    }

    private void SetRotation (Segment segment, float angle = -1) {
        if (angle == -1) {
            angle = 0;
            Vector3 forward = Vector3.forward;
            lastSegmentAdded.transform.rotation.ToAngleAxis(out angle, out forward);
            int random = Random.Range(0, 2);
            float factor = 1;
            if (random == 0) {
                random *= -1;
            }
            angle += factor * ANGLE_SPREAD;
        }
        segment.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void SetScale (Segment segment, int size = 0) {
        if (size == 0) {
            size = Random.Range(2, 4);
        }
        segment.SetSize(size);
    }

    #endregion
}
