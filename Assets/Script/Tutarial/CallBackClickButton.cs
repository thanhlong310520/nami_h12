using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CallBackClickButton : MonoBehaviour
{
    public UnityEvent eventClick;
    public void Handle()
    {
        eventClick?.Invoke();
    }
}
