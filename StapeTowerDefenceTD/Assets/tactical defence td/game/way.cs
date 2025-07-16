using UnityEngine;

public class way : MonoBehaviour
{
    public Vector3[] points;
    private LineRenderer line;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {

        line = gameObject.GetComponent<LineRenderer>();
        System.Array.Resize(ref points, line.positionCount);

        line.GetPositions(points);

        print(points);

        line.enabled = false;
        //gameObject.SetActive(false);
    }
}
