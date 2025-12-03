using UnityEngine;
using UnityEngine.Splines;

public class SCR_SplineRenderer : MonoBehaviour
{
    public SplineContainer playerPath;
    public int splinePointsToRender;
    public Color pathColor = Color.cyan; 
    public LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = splinePointsToRender;
        lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
        lineRenderer.material.color = pathColor;

        for (int i = 0; i < splinePointsToRender; i++)
        {
            float t = (float)i / (splinePointsToRender -1);
            lineRenderer.SetPosition(i, playerPath.EvaluatePosition(t));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
