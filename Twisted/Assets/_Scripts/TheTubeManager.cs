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
        InitialiseRotation();
        m_currentSegment = m_tubeBuilder.GenerateInitialSegments();
        Debug.Log(m_currentSegment);
    }

    public void GoToNextSegment () {
        int nextSegmentIndex = m_currentSegment.transform.GetSiblingIndex() + 1;
        Segment nextSegment = transform.GetChild(nextSegmentIndex).GetComponent<Segment>();
        Rotate(nextSegment.GetOffset());
        m_currentSegment = nextSegment;
    }

    #endregion


    #region PRIVATE

    System.Action m_DoAction;

    TubeBuilder m_tubeBuilder;
    TubeCleaner m_tubeCleaner;
    
    Segment m_currentSegment;
    int m_currentQuaternionIndex = 0;

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

    public float rotationDuration = 0.5f;

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

    #endregion
}
