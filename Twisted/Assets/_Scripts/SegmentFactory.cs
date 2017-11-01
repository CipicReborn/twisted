using UnityEngine;

public class SegmentFactory  {

    #region PUBLIC STATIC

    public static Segment GenerateSegment () {

        GameObject segment = GameObject.Instantiate(segmentPrefab);
        return segment.GetComponent<Segment>();
    }


    #endregion



    #region PRIVATE STATIC

    private static GameObject segmentPrefab = Resources.Load("Prefabs/TubeSegmentPrefab") as GameObject;


    #endregion
}
