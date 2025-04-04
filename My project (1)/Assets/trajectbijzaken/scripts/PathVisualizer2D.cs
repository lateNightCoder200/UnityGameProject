using UnityEngine;

public class PathVisualizer2D : MonoBehaviour
{
    public Transform[] checkpoints;
    public LineRenderer lineRenderer;

    void Start()
    {
        Vector3[] positions = new Vector3[checkpoints.Length];
        for (int i = 0; i < checkpoints.Length; i++)
        {
            positions[i] = checkpoints[i].position;
        }
        lineRenderer.positionCount = checkpoints.Length;
        lineRenderer.SetPositions(positions);
    }
}
