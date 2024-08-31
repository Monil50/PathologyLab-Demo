//using UnityEngine;
//using UnityEngine.XR;

//public class ShakeCounter : MonoBehaviour
//{
//    public XRNode rightControllerNode = XRNode.RightHand; // Set to the desired hand controller
//    public XRNode leftControllerNode = XRNode.LeftHand; // Set to the desired hand controller
//    public float shakeThreshold = 2.0f; // Adjust threshold as needed
//    public float timeBetweenShakes = 0.5f; // Minimum time between shakes

//    private int shakeCount = 0;
//    private float lastShakeTime = 0;

//    void Update()
//    {
//        InputDevice rightDevice = InputDevices.GetDeviceAtXRNode(rightControllerNode);
//        InputDevice leftDevice = InputDevices.GetDeviceAtXRNode(leftControllerNode);
//        Vector3 acceleration;
//        if (leftDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out acceleration))
//        {
//            Debug.Log("Acceleration: " + acceleration.magnitude);
//            //if (acceleration.magnitude > shakeThreshold && Time.time > lastShakeTime + timeBetweenShakes)
//            if (acceleration.magnitude > shakeThreshold)
//            {
//                shakeCount++;
//                Debug.LogWarning("Shake Detected! Total Shakes: " + shakeCount);
//                if (shakeCount > 8) 
//                {
//                    AudioManager.instance.PlayAudio("Step0_0");

//                }

//            }
//        }
//        else
//        {
//            Debug.Log("No acceleration data available.");
//        }
//    }
//}


using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

public class ShakeCounter : MonoBehaviour
{
    public XRNode rightControllerNode = XRNode.RightHand; // Set to the desired hand controller
    public XRNode leftControllerNode = XRNode.LeftHand; // Set to the desired hand controller
    public float shakeThreshold = 2.0f; // Adjust threshold as needed
    public float timeBetweenShakes = 0.5f; // Minimum time between shakes
    public Collider capCollider;

    private int shakeCount = 0;
    private float lastShakeTime = 0;
    public UnityEvent endShake;

    void Update()
    {
        InputDevice rightDevice = InputDevices.GetDeviceAtXRNode(rightControllerNode);
        InputDevice leftDevice = InputDevices.GetDeviceAtXRNode(leftControllerNode);

        Vector3 rightAcceleration;
        Vector3 leftAcceleration;

        bool rightShake = false;
        bool leftShake = false;

        // Check if the right controller's acceleration exceeds the threshold
        if (rightDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out rightAcceleration))
        {
            Debug.Log("Right Controller Acceleration: " + rightAcceleration.magnitude);
            if (rightAcceleration.magnitude > shakeThreshold)
            {
                rightShake = true;
            }
        }
        else
        {
            Debug.Log("No acceleration data available for right controller.");
        }

        // Check if the left controller's acceleration exceeds the threshold
        if (leftDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out leftAcceleration))
        {
            Debug.Log("Left Controller Acceleration: " + leftAcceleration.magnitude);
            if (leftAcceleration.magnitude > shakeThreshold)
            {
                leftShake = true;
            }
        }
        else
        {
            Debug.Log("No acceleration data available for left controller.");
        }

        // Increment shake count if either controller has been shaken
        if ((rightShake || leftShake) && Time.time > lastShakeTime + timeBetweenShakes)
        {
            shakeCount++;
            lastShakeTime = Time.time;
            Debug.LogWarning("Shake Detected! Total Shakes: " + shakeCount);

            if (shakeCount > 8)
            {
               // AudioManager.instance.PlayAudio("correct-156911");
                capCollider.GetComponent<Collider>().enabled = true;
                endShake.Invoke();

            }
        }
    }
}
