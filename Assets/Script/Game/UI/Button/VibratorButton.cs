using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VibratorButton : BaseButton
{

    [SerializeField] Image vibratorImage;
    [SerializeField] List<Sprite> spriteOnOffVibrator = new List<Sprite>();
    protected override void Start()
    {
        base.Start();
        vibratorImage.sprite = spriteOnOffVibrator[0];
    }
    protected override void Handle()
    {
        base.Handle();
        UICtr.instance.setting.onVibrator = !UICtr.instance.setting.onVibrator;
        if (UICtr.instance.setting.onVibrator)
        {
            vibratorImage.sprite = spriteOnOffVibrator[0];
        }
        else
        {
            vibratorImage.sprite = spriteOnOffVibrator[1];
        }

    }
}
