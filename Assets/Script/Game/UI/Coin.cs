using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;

public class Coin : MonoBehaviour
{
    [SerializeField] private Transform pileOfCoins;
    [SerializeField] private Transform coinHolder;
    public Transform target;
    [SerializeField] private List<Vector2> initialPos = new List<Vector2>();
    [SerializeField] private List<Quaternion> initialRotation = new List<Quaternion>();


    [SerializeField] float timeScale1 = 0.1f;
    [SerializeField] float timeMove = 0.5f;
    [SerializeField] float timeRotate = 0.2f;
    [SerializeField] float timeDelayMoveandRotate = 0.3f;
    [SerializeField] float timeScale0 = 0.3f;
    [SerializeField] float timeDelayScale = 1f;
    [SerializeField] float timeCountDelay = 0.1f;
    public TMP_Text textcoin;
    public TMP_Text textnumbercoin;
    public int numberAdd = 20;
    public int add = 2;
    int numbercoinAdded = 0;
    public void Init()
    {
        for (int i = 0; i < pileOfCoins.childCount; i++)
        {
            initialPos.Add(pileOfCoins.GetChild(i).GetComponent<RectTransform>().anchoredPosition);
            initialRotation.Add(pileOfCoins.GetChild(i).rotation);
        }
        textcoin.text = GameManager.instance.playerData.coinNumber.ToString();
        SetDefaultCoin();
    }
    public void SetDefaultCoin()
    {
        SetCoinAdd(20);
        pileOfCoins.gameObject.SetActive(true);
        for (int i = 0; i < pileOfCoins.childCount; i++)
        {
            var coin1 = coinHolder.GetChild(i);
            coin1.gameObject.SetActive(true);
            
            var coin = pileOfCoins.GetChild(i).GetComponent<RectTransform>();
            coin.anchoredPosition =  initialPos[i];
            coin.rotation =  initialRotation[i];
            coin.localScale = Vector3.zero;
        }
    }
    public void SetCoinAdd(int num)
    {
        numberAdd = num;
        textnumbercoin.text = numberAdd.ToString();
        add = numberAdd / pileOfCoins.childCount;
        print("setttttttttttt" + numberAdd + "   " + add);
    }

    public void CountCoins(UnityAction callback)
    {
        UICtr.instance.mask.parent.gameObject.SetActive(true);
        pileOfCoins.gameObject.SetActive(true);
        numbercoinAdded = 0;
        var delay = 0f;

        for (int i = 0; i < pileOfCoins.childCount; i++)
        {
            StartCoroutine(HideCoinIncoinHolder((pileOfCoins.childCount - 1 - i), delay));

            pileOfCoins.GetChild(i).DOScale(1f, timeScale1).SetDelay(delay).SetEase(Ease.OutBack);

            pileOfCoins.GetChild(i).DOMove(target.position, timeMove)
                .SetDelay(delay + timeDelayMoveandRotate).SetEase(Ease.InBack).OnComplete(() =>
                {
                    CountDollars(callback);
                });


            pileOfCoins.GetChild(i).DORotate(Vector3.zero, timeRotate).SetDelay(delay + timeDelayMoveandRotate)
                .SetEase(Ease.Flash);


            pileOfCoins.transform.GetChild(i).DOScale(0f, timeScale0).SetDelay(delay + timeDelayScale).SetEase(Ease.OutBack);

            delay += timeCountDelay;

        }

        
    }

    public void Settext()
    {
        textcoin.text = GameManager.instance.playerData.coinNumber.ToString();
    }
    IEnumerator HideCoinIncoinHolder(int i,float time)
    {
        yield return new WaitForSeconds(time);
        coinHolder.GetChild(i).gameObject.SetActive(false);
    }
    void CountDollars(UnityAction callback)
    {
        numbercoinAdded++;
        GameManager.instance.AddCoin(add);
        if(numbercoinAdded >= pileOfCoins.childCount)
        {
            pileOfCoins.gameObject.SetActive(false);
            callback.Invoke();
        }
    }
}
