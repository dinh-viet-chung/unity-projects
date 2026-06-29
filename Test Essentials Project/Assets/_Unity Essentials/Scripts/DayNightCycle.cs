using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Day/Night Settings")]
    [Tooltip("How many real-time seconds a full day takes")]
    public float dayDurationInSeconds = 120f;

    [Tooltip("Axis of rotation (usually X for sun movement)")]
    public Vector3 rotationAxis = Vector3.right;

    private float rotationSpeed;

    void Start()
    {
        // 360 degrees divided by total seconds = degrees per second
        rotationSpeed = 360f / dayDurationInSeconds;
    }

    void Update()
    {
        // Rotate the light continuously
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
    }

    // Optional: Update speed if value changes in Inspector at runtime
    void OnValidate()
    {
        if (dayDurationInSeconds > 0)
        {
            rotationSpeed = 360f / dayDurationInSeconds;
        }
    }
}