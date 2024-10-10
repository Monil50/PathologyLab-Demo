using UnityEngine;
using BNG;

public class DropPartical : MonoBehaviour
{
    // Reference to the VR controller (or bottle) transform
    public Transform controllerTransform;

    // The angle threshold at which to trigger the water fall (adjustable in the inspector)
    public float triggerAngle = 0f;

    // Flag to check if the water has already fallen
    private bool hasWaterFallen = false;

    void Update()
    {
        // Get the current "up" vector of the bottle (Y-axis in local space)
        Vector3 bottleUp = controllerTransform.up;

        // Calculate the angle between the bottle's "up" vector and the global Y-axis (Vector3.up)
        float tiltAngle = Vector3.Angle(bottleUp, Vector3.up);
        Debug.Log("tilt Angle "+tiltAngle);
        // Check if the tilt angle exceeds the trigger angle and if the water hasn't fallen yet
        if (tiltAngle >= triggerAngle )
        {
            WaterFall();
        }
    }

    // Function to handle water falling
    void WaterFall()
    {
       // hasWaterFallen = true;
        Debug.Log("Water is falling!");
        // Add your logic for water falling here
    }

    // Optional: Reset the water fall for testing purposes
    public void ResetWaterFall()
    {
        hasWaterFallen = false;
    }   
}
