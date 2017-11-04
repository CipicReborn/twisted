using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeCleaner : MonoBehaviour {

    #region PUBLIC METHODS

    public void ClearPassedSegments () {
        for (int i = 0; i < transform.childCount; i++) {
            var lChild = transform.GetChild(0);
            var end = lChild.position.z + lChild.GetChild(0).localScale.z;
            if (end < m_segmentsClearanceZBoundary) {
                ClearSegment(lChild);
            }
            else {
                break;
            }
        }
    }

    public void ClearAllSegments () {
        int startIndex = transform.childCount - 1;
        for (int i = startIndex; i >= 0; i--) {
            ClearSegment(transform.GetChild(i));
        }
    }

    #endregion


    #region PRIVATE

    float m_segmentsClearanceZBoundary;

    private void Awake () {
        m_segmentsClearanceZBoundary = Camera.main.transform.position.z;
    }

    private void ClearSegment (Transform segment) {
        SegmentsPool.Instance.PutBackToPool(segment.GetComponent<Segment>());
    }
    #endregion

}
