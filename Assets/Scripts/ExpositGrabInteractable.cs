using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using BNG;

public class ExpositGrabInteractable : MonoBehaviour
{
    private Transform originalParent;
    public Collider RighthandColider;
    public Collider LefthandColider;
    public Transform RightAttachTransform;
    public Transform LeftAttachTransform;
    public GameObject DropPoint;
    private bool isGrabbed = false;
    public bool UseAttachTransform = false;
    public bool DropWithAnimation = true;
    public float GhostEnableTime = 3f;
    private Collider DropColider;
    public UnityEvent OnGrab;
    public UnityEvent OnRelese;
    public UnityEvent FastRelese;

    //Hand Swith Manager
   /* public GameObject XRRightHandModel;
    public GameObject XRLeftHandModel;
    public GameObject ObjectRightHandPose;
    public GameObject ObjectLeftHandPose;*/

    bool isRightHandGrab = false;
    bool isLeftHandGrab = false;

    private float lerpDuration = 1f;

    // Haptic feedback properties
    public float hapticIntensity = 0.5f;
    public float hapticDuration = 0.1f;

    void Start()
    {
        originalParent = DropPoint.transform.parent; // Store the original parent of the object
        DropPoint.SetActive(false);
        DropColider = DropPoint.GetComponent<Collider>();
       // ObjectLeftHandPose.SetActive(false);
       // ObjectRightHandPose.SetActive(false);

    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the other object is tagged as "Hand"
        if (other == RighthandColider)
        {
            if (!isGrabbed)
            {
                StartCoroutine(Grab(other.transform));
                StartCoroutine(OnDropObjectLate());
                isRightHandGrab = true;

                

                // Trigger haptic feedback
                TriggerHapticFeedback(true);
            }
        }
        if (other == LefthandColider)
        {
            if (!isGrabbed)
            {
                StartCoroutine(Grab(other.transform));
                StartCoroutine(OnDropObjectLate());
                isLeftHandGrab = true;


                // Trigger haptic feedback
                TriggerHapticFeedback(false);
            }
        }
        if (other == DropColider)
        {
            FaaaastRelese();

            if (isGrabbed)
            {
                StartCoroutine(Release());
                DropPoint.SetActive(false);
                Debug.Log("function called relese");
            }
        }
    }

    IEnumerator Grab(Transform hand)
    {
        isGrabbed = true;
        transform.SetParent(hand); // Make the object a child of the hand
        GetComponent<Rigidbody>().isKinematic = true; // Make it kinematic so it moves with the hand
        OnGrab.Invoke();
   
        float elapsedTime = 0f;
        if (UseAttachTransform)
        {
            while (elapsedTime < lerpDuration)
            {
                if (isRightHandGrab)
                {
                    transform.localPosition = Vector3.Lerp(gameObject.transform.localPosition, RightAttachTransform.transform.localPosition, elapsedTime / lerpDuration);
                    transform.localRotation = Quaternion.Lerp(gameObject.transform.localRotation, RightAttachTransform.transform.localRotation, elapsedTime / lerpDuration);
                    //XRRightHandModel.SetActive(false);
                    //ObjectRightHandPose.SetActive(true);
                    //ObjectLeftHandPose.SetActive(false);
                }
                if (isLeftHandGrab)
                {
                    transform.localPosition = Vector3.Lerp(gameObject.transform.localPosition, LeftAttachTransform.transform.localPosition, elapsedTime / lerpDuration);
                    transform.localRotation = Quaternion.Lerp(gameObject.transform.localRotation, LeftAttachTransform.transform.localRotation, elapsedTime / lerpDuration);
                 /*   XRLeftHandModel.SetActive(false);
                    ObjectLeftHandPose.SetActive(true);
                    ObjectRightHandPose.SetActive(false);*/
                }
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            transform.localPosition = Vector3.zero; // Optionally, reset the position relative to the hand
            transform.localRotation = Quaternion.identity; // Optionally, reset the rotation relative to the hand
        }
    }

    IEnumerator Release()
    {
        float elapsedTime = 0f;

        StartCoroutine(OnGrabBoolAgine());
        transform.SetParent(originalParent); // Restore the original parent
        GetComponent<Rigidbody>().isKinematic = true; // Make it non-kinematic again

        if (DropWithAnimation)
        {
            while (elapsedTime < lerpDuration)
            {
                transform.position = Vector3.Lerp(gameObject.transform.position, DropPoint.transform.position, elapsedTime / lerpDuration);
                transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, DropPoint.transform.rotation, elapsedTime / lerpDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
                isRightHandGrab = false;
                isLeftHandGrab = false;
            }
        }
        else
        {
            gameObject.transform.localRotation = DropPoint.transform.localRotation;
            gameObject.transform.localPosition = DropPoint.transform.localPosition;
        }
        OnRelese.Invoke();
    }

    IEnumerator OnGrabBoolAgine()
    {
        yield return new WaitForSeconds(3);
        isGrabbed = false;
    }

    IEnumerator OnDropObjectLate()
    {
        yield return new WaitForSeconds(GhostEnableTime);
        DropPoint.SetActive(true);
    }

    public void FaaaastRelese()
    {
        FastRelese.Invoke();
     /*   XRRightHandModel.SetActive(true);
        XRLeftHandModel.SetActive(true);
        ObjectLeftHandPose.SetActive(false);
        ObjectRightHandPose.SetActive(false);*/
    }

    private void TriggerHapticFeedback(bool isRightHand)
    {
        if (isRightHand)
        {
            InputBridge.Instance.VibrateController(0, hapticIntensity, hapticDuration, ControllerHand.Right);
        }
        else
        {
            InputBridge.Instance.VibrateController(0, hapticIntensity, hapticDuration, ControllerHand.Left);
        }
    }
}
