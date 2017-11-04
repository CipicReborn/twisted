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
        m_currentSegment = m_tubeBuilder.GenerateInitialSegments();
        Debug.Log(m_currentSegment);
    }

    public void GoToNextSegment () {

    }

    #endregion


    #region PRIVATE

    System.Action m_DoAction;

    TubeBuilder m_tubeBuilder;
    TubeCleaner m_tubeCleaner;
    
    Segment m_currentSegment;

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

    #endregion
}
