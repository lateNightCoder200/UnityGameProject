using UnityEngine;
using UnityEngine.UI;

public class CheckpointInteraction2D : MonoBehaviour
{
    public int checkpointIndex; // Checkpoint index to trigger
    public ParrotSplineController controller; // Make sure this matches your controller's class name

    void OnMouseDown()
    {
        if (controller != null)
        {
            controller.MoveToCheckpoint(checkpointIndex);
        }
    }
}
