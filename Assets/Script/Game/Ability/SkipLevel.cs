using DG.Tweening;
using Nami.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkipLevel : BaseButton
{
    protected override void Handle()
    {
        base.Handle();
        UICtr.instance.loadingAds.StartLoad();
        print("redo ads");
        GameAds.Get.LoadAndShowRewardAd((onComplete) =>
        {
            if (onComplete)
            {
                UICtr.instance.ChangToModelState();
            }
            else
            {
                Debug.Log("redo ads khong hien ads");
            }
            UICtr.instance.loadingAds.Done();
        });
    }
}
