using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeButton : BaseButton
{
    protected override void Handle()
    {
        base.Handle();
        UICtr.instance.LoadModelState();
    }
}
