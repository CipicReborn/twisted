using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentsPool {

    #region PUBLIC STATIC

    public static Segment GetSegment () {
        EnsurePoolContainsOneElement();
        var segment = segmentsPool.Pop();
        segment.Init();
        return segment;
    }

    public static void PutBackToPool (Segment segment) {
        segmentsPool.Push(segment);
    }

    #endregion



    #region PRIVATE STATIC

    private static Stack<Segment> segmentsPool = new Stack<Segment>();

    private static void EnsurePoolContainsOneElement () {
        if (segmentsPool.Count == 0) {
            var segment = SegmentFactory.GenerateSegment();
            segmentsPool.Push(segment);
        }
    }

    #endregion
}
