using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuyInItemButton : BaseButton
{
    public UnityAction callback;
    protected override void Handle()
    {
        base.Handle();
        callback?.Invoke();
    }
}
