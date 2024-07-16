using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ConfirmTaks : BaseButton
{
    public bool canClick = false;
    public Image image;
    public UnityEvent callback;
    public List<Sprite> sprites = new List<Sprite>();
    protected override void Handle()
    {
        if (!canClick) return;
        base.Handle();
        callback?.Invoke();
    }

    public void SetOnClickButton(bool isclick)
    {
        Sprite spriteOn;
        if(isclick)
        {
            spriteOn = sprites[0];
        }
        else
        {
            spriteOn = sprites[1];
            print("chaaaaaaaaaaaaaaaaaaaaaaaannnnnnne");
        }
        print(isclick);
        bt.interactable = isclick;
        image.sprite = spriteOn;

    }
}
