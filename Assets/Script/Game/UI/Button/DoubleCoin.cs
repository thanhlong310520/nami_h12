using Nami.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoubleCoin : BaseButton
{
    protected override void Start()
    {
        base.Start();
    }
    protected override void Handle()
    {
        base.Handle();
        UICtr.instance.loadingAds.StartLoad();
        print("redo ads");
        GameAds.Get.LoadAndShowRewardAd((onComplete) =>
        {
            if (onComplete)
            {

                UICtr.instance.coinctr.SetCoinAdd(40);
                UICtr.instance.coinctr.CountCoins(() => { UICtr.instance.ChangToModelState(); });
            }
            else
            {
                Debug.Log("redo ads khong hien ads");
            }
            UICtr.instance.loadingAds.Done();
        });


    }
}
