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

    public float Speed = 0;
    public float rotationDuration = 0.5f;
    public Color[] Colors;

    public void SetModeGame () {
        m_tubeBuilder.GenerateAdditionnalSegments();
        m_DoAction = DoActionRun;
    }

    public void SetModeVoid () {
        m_tubeBuilder.StopGeneratingSegments();
        m_DoAction = DoActionVoid;
    }

    public void SetModeMenu () {
        m_tubeBuilder.StopGeneratingSegments();
        m_tubeCleaner.ClearAllSegments();
        Init();
    }

    public void GoToNextSegment () {
        int nextSegmentIndex = m_currentSegment.transform.GetSiblingIndex() + 1;
        Segment nextSegment = transform.GetChild(nextSegmentIndex).GetComponent<Segment>();
        Rotate(nextSegment.GetOffset());
        m_currentSegment = nextSegment;
    }

    public float[] GetCurrentSegmentBoundaries () {
        SetCurrentSegmentBoundaries();
        return m_currentSegmentBoundaries;
    }

    public void ShowCurrentSegment() {
        m_currentSegment.transform.GetChild(0).GetComponent<MeshRenderer>().material = Resources.Load("RedFloor") as Material;
    }

    public void ChangeSegmentsColor () {
        m_colorIndex++;
        Utils.LoopValue(ref m_colorIndex, 0, Colors.Length - 1);
        for (int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).GetComponent<Segment>().SetColor(Colors[m_colorIndex]);
        }
    }

    public Color GetColor () {
        return Colors[m_colorIndex];
    }
    #endregion


    #region PRIVATE

    System.Action m_DoAction;

    TubeBuilder m_tubeBuilder;
    TubeCleaner m_tubeCleaner;
    
    Segment m_currentSegment;
    int m_currentQuaternionIndex = 0;
    float[] m_currentSegmentBoundaries = { 0, 0 };

    int m_colorIndex = 0;

    readonly Quaternion[] ROTATIONS = {
        Quaternion.identity,
        Quaternion.AngleAxis(45.0f, Vector3.forward),
        Quaternion.AngleAxis(90.0f, Vector3.forward),
        Quaternion.AngleAxis(135.0f, Vector3.forward),
        Quaternion.AngleAxis(180.0f, Vector3.forward),
        Quaternion.AngleAxis(225.0f, Vector3.forward),
        Quaternion.AngleAxis(270.0f, Vector3.forward),
        Quaternion.AngleAxis(315.0f, Vector3.forward)
    };

    

    private void Awake () {
        instance = this;
        m_tubeBuilder = GetComponent<TubeBuilder>();
        m_tubeCleaner = GetComponent<TubeCleaner>();

        m_DoAction = DoActionVoid;
    }

    private void Init () {
        transform.position = Vector3.zero;
        InitialiseRotation();
        m_currentSegment = m_tubeBuilder.GenerateInitialSegments();
    }

    private void Update () {
        m_DoAction();
	}

    private void DoActionVoid () {}

    private void DoActionRun () {
        Move();
        m_tubeCleaner.ClearPassedSegments();
    }

    private void Move () {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - Speed * Time.deltaTime);
    }

    private void InitialiseRotation () {
        m_currentQuaternionIndex = 0;
        transform.rotation = ROTATIONS[0];
    }

    private void Rotate (int offset) {
        Quaternion from = transform.rotation;

        int newIndex = m_currentQuaternionIndex - offset;
        Utils.LoopValue(ref newIndex, 0, ROTATIONS.Length - 1);
        Quaternion to = ROTATIONS[newIndex];

        StartCoroutine(DoRotate(from, to));
        m_currentQuaternionIndex = newIndex;
    }


    IEnumerator DoRotate (Quaternion from, Quaternion to) {
        float arrivalTime = Time.time + rotationDuration;
        float elapsedTime = 0;
        yield return null;
        while (Time.time < arrivalTime) {
            elapsedTime += Time.deltaTime;
            float normalisedProgression = elapsedTime / rotationDuration;  
            transform.rotation = Quaternion.Slerp(from, to, normalisedProgression);
            yield return null;
        }
        transform.rotation = to;
    }

    private void SetCurrentSegmentBoundaries () {
        m_currentSegmentBoundaries[0] = m_currentSegment.transform.position.z;
        m_currentSegmentBoundaries[1] = m_currentSegment.transform.position.z + m_currentSegment.GetSize();
    }

    #endregion
}
