using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioButton : BaseButton
{
    public bool onAudio = true;

    [SerializeField] Image audioImage;
    [SerializeField] List<Sprite> spriteOnOffAudio = new List<Sprite>();
    protected override void Start()
    {
        base.Start();
        onAudio = true;
        audioImage.sprite = spriteOnOffAudio[0];
        GameManager.instance.audioManager.audioSource.volume = 1;
    }
    protected override void Handle()
    {
        base.Handle();
        onAudio = !onAudio;
        if (onAudio)
        {
            audioImage.sprite = spriteOnOffAudio[0];
            GameManager.instance.audioManager.audioSource.volume = 1;
        }
        else
        {
            audioImage.sprite = spriteOnOffAudio[1];
            GameManager.instance.audioManager.audioSource.volume = 0;
        }

    }
}
