using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LoadingAdsReward : MonoBehaviour
{
    public Image load;
    public float timeTween = 1f;
    public Tween rotTween;
    
    public void StartLoad()
    {
        load.transform.parent.gameObject.SetActive(true);
        load.transform.eulerAngles = Vector3.zero;
        rotTween = load.transform.DORotate(new Vector3(0, 0, -360), timeTween, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
    }

    public void Done()
    {
        rotTween?.Kill();
        load.transform.eulerAngles = Vector3.zero;
        load.transform.parent.gameObject.SetActive(false);
    }
}
