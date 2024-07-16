using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonOnOffTask : BaseButton
{
    public UnityAction<bool> callback;
    public bool isOn;
    protected override void Handle()
    {
        base.Handle();
        callback?.Invoke(isOn);
    }
}
