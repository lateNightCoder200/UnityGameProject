using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UI;
using System.Collections;

public class ParrotSplineController : MonoBehaviour
{
    [Header("Spline Animate Settings")]
    public SplineAnimate splineAnimate;        // Spline Animate component on the parrot
    public SplineContainer splineContainer;    // Reference to your SplineContainer

    [Header("Checkpoint Setup")]
    [Range(0f, 1f)]
    public float[] checkpointTs;               // Normalized t-values for checkpoints (0 to 1)
    public Button[] checkpointButtons;         // UI buttons that trigger movement

    public float checkpointTolerance = 0.02f;  // Tolerance for reaching checkpoints
    public float moveSpeed = 3f;               // Set a constant speed

    private bool isMoving = false;
    private int activeCheckpoint = 0;

    void Start()
    {
        int checkpointProgress = PlayerPrefs.GetInt("checkpointProgress", 0);

        // Pause the SplineAnimate at the start.
        if (splineAnimate != null)
            splineAnimate.Pause();

        // Setup button listeners.
        if (checkpointButtons != null && checkpointButtons.Length == checkpointTs.Length)
        {
            for (int i = 0; i < checkpointButtons.Length; i++)
            {
                int index = i;
                checkpointButtons[i].onClick.AddListener(() => MoveToCheckpoint(index));
            }
        }

        checkpointButtons[0].gameObject.SetActive(true);
    }

    void Update()
    {
        if (isMoving && splineAnimate != null)
        {
            float currentT = splineAnimate.NormalizedTime % 1f;
            float targetT = checkpointTs[activeCheckpoint];

            if (Mathf.Abs(currentT - targetT) < checkpointTolerance)
            {
                isMoving = false;
                splineAnimate.Pause();
                OnCheckpointReached(activeCheckpoint);
            }
        }
    }

 
    public void MoveToCheckpoint(int checkpointIndex)
    {
        if (checkpointIndex < 0 || checkpointIndex >= checkpointTs.Length)
        {
            Debug.LogWarning("Invalid checkpoint index");
            return;
        }

        foreach (var button in checkpointButtons)
        {
            button.gameObject.SetActive(false);
        }

        activeCheckpoint = checkpointIndex;
        float targetT = checkpointTs[checkpointIndex];

        if (splineAnimate != null)
        {
            splineAnimate.Pause(); // Stop any movement first
            StartCoroutine(AnimateToCheckpoint(targetT));
        }
    }

    private IEnumerator AnimateToCheckpoint(float targetT)
    {
        isMoving = true;

        // Reset scale to the correct value
        transform.localScale = new Vector3(0.2f, 0.2f, 0f); // Set scale to the correct value

        // Ensure the renderer is enabled
        GetComponent<Renderer>().enabled = true;

        float startT = splineAnimate.NormalizedTime;
        Vector3 startPos = splineContainer.Spline.EvaluatePosition(startT);
        Vector3 targetPos = splineContainer.Spline.EvaluatePosition(targetT);

        float distance = Vector3.Distance(startPos, targetPos);  // Calculate distance
        float duration = distance / moveSpeed;  // Time = Distance / Speed
        float elapsedTime = 0f;

        Debug.Log($"Moving from T: {startT} to T: {targetT}, distance: {distance}, duration: {duration}s");

        splineAnimate.Play(); // Start animation

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            float newT = Mathf.Lerp(startT, targetT, t);
            splineAnimate.NormalizedTime = newT;

            Vector3 newPosition = splineContainer.Spline.EvaluatePosition(newT);
            transform.position = splineContainer.transform.TransformPoint(newPosition);

            // Keep the rotation's Y value at 0 (prevent 90 rotation) and maintain X/Z rotation as is.
            transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);

            yield return null;
        }

        splineAnimate.NormalizedTime = targetT;
        transform.position = splineContainer.transform.TransformPoint(splineContainer.Spline.EvaluatePosition(targetT));

        // Ensure Y rotation is still 0 after completion
        transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);

        isMoving = false;
        splineAnimate.Pause();
        OnCheckpointReached(activeCheckpoint);
    }




  
    private void OnCheckpointReached(int checkpointIndex)
    {
        Debug.Log("Checkpoint " + checkpointIndex + " reached.");

        if (checkpointButtons != null && checkpointIndex < checkpointButtons.Length)
        {
            checkpointButtons[checkpointIndex].gameObject.SetActive(true);
        }
    }
}
