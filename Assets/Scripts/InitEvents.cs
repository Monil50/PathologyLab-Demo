using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InitEvents : MonoBehaviour
{
    public UnityEvent initEvents;

    void Start()
    {
        initEvents.Invoke();
    }

}
