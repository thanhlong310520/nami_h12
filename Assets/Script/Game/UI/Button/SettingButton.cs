using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingButton : BaseButton
{
    [SerializeField] bool isShow = false;
    [SerializeField] RectTransform holderButton;
    [SerializeField] float posHoder = 200f;
    [SerializeField] float timeShowHide = 0.25f;
    Button blur;
    [SerializeField] bool canClick = true;
    public bool onVibrator = true;
    protected override void Start()
    {
        base.Start();
        SetDefaul();
        blur = UICtr.instance.blur.GetComponent<Button>();
    }
    protected override void Handle()
    {
        if (!canClick) return;
        base.Handle();
        canClick = false;
        isShow = !isShow;
        if (isShow)
        {
            GameCtr.instance.SetCanTouch(false);    
            UICtr.instance.blur.gameObject.SetActive(true);
            UICtr.instance.blur.color = new Color(1, 1, 1, 0);
            UICtr.instance.blur.DOColor(new Color(1, 1, 1, (float)150 / 255), 0.1f);
            blur.onClick.AddListener(()=> { Handle(); });
            holderButton.transform.parent.gameObject.SetActive(true);
            holderButton.DOAnchorPos(Vector2.zero, timeShowHide).SetEase(Ease.InOutQuad).OnComplete(() => { canClick = true;  });
        }
        else
        {
            GameCtr.instance.SetCanTouch(true);
            blur.onClick.RemoveListener(() => { Handle(); });
            UICtr.instance.blur.DOColor(new Color(1, 1, 1, 0), 0.1f).OnComplete(() => {
                blur.gameObject.SetActive(false);
            });
            holderButton.DOAnchorPos(new Vector2(0,posHoder), timeShowHide).SetEase(Ease.InOutQuad).OnComplete(() =>
            {
                holderButton.transform.parent.gameObject.SetActive(false);
                canClick = true;
            });
        }
    }

    void SetDefaul()
    {
        isShow = false;
        canClick = true;
        onVibrator = true;
        holderButton.anchoredPosition = new Vector2(0, posHoder);
        holderButton.transform.parent.gameObject.SetActive(false);
    }
}
