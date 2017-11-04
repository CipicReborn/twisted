using System.Collections.Generic;
using UnityEngine;

public class SegmentsPool: MonoBehaviour {

    #region PUBLIC STATIC

    public static SegmentsPool Instance {
        get {
            return instance;
        }
    }

    private static SegmentsPool instance;

    #endregion


    #region PUBLIC

    public int poolSize = 0;

    public Segment GetSegment () {
        EnsurePoolContainsOneElement();
        var segment = transform.GetChild(0).GetComponent<Segment>();
        segment.gameObject.SetActive(true);
        segment.transform.SetParent(null);
        segment.Init();
        return segment;
    }

    public void PutBackToPool (Segment segment) {
        segment.transform.SetParent(transform);
        segment.gameObject.SetActive(false);
    }

    #endregion


    #region PRIVATE

    private void Awake () {
        instance = this;
        for (int i = 0; i < poolSize; i++) {
            AddNewSegmentToPool();
        }
    }

    private void EnsurePoolContainsOneElement () {
        if (transform.childCount == 0) {
            AddNewSegmentToPool();
        }
    }

    private void AddNewSegmentToPool () {
        PutBackToPool(SegmentFactory.GenerateSegment());
    }

    #endregion
}
