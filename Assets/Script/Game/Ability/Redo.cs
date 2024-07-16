using DG.Tweening;
using Nami.Controller;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Redo : BaseButton
{
    [SerializeField] int numbercointtouse = 20;
    public Image image;
    public TMP_Text textNumber;
    public List<Sprite> sprites = new List<Sprite>();
    [SerializeField] int numberAdd = 3;
    [SerializeField] int cost = 100;
    [SerializeField] GameObject panelRedo;
    protected override void Handle()
    {
        base.Handle();
        Handling();
    }

    public void Handling()
    {
        if (GameCtr.instance.listBlockDone.Count <= 0) return;

        if (GameManager.instance.playerData.numberRedo > 0)
        {
            AddNumberRedo(-1);
            Redoing();
        }
        else
        {
            OnOffPopupRedo(true);
        }
    }

    public void AddByCoin()
    {
        GameManager.instance.audioManager.PlaySound(0);
        if (GameManager.instance.playerData.coinNumber < cost) return;
        GameManager.instance.AddCoin(-cost);
        AddNumberRedo(numberAdd);
        
    }

    public void AddByWatchAds()
    {
        GameManager.instance.audioManager.PlaySound(0);
        UICtr.instance.loadingAds.StartLoad();
        print("redo ads");
        GameAds.Get.LoadAndShowRewardAd((onComplete) =>
        {
            if (onComplete)
            {
                AddNumberRedo(numberAdd);
            }
            else
            {
                Debug.Log("redo ads khong hien ads");
            }
            UICtr.instance.loadingAds.Done();
        });

    }

    public void OffPopup()
    {
        GameManager.instance.audioManager.PlaySound(0);
        OnOffPopupRedo(false);
    }
    public void OnOffPopupRedo(bool isON)
    {
        panelRedo.SetActive(isON);
    }

    public void SetTextNumberRedoing()
    {
        if (GameManager.instance.playerData.numberRedo > 0)
            textNumber.text = GameManager.instance.playerData.numberRedo.ToString();
        else
            textNumber.text = "+";

        OnOffPopupRedo(false);
    }

    public void AddNumberRedo(int add)
    {
        GameManager.instance.playerData.numberRedo += add;
        GameManager.instance.SaveData();
        SetTextNumberRedoing();
    }
    public void Redoing()
    {
        Block b = GameCtr.instance.listBlockDone[GameCtr.instance.listBlockDone.Count - 1];
        GameCtr.instance.listBlockDone.Remove(b);
        b.canClick = true;
        b.OnMoveSprite(true);
        b.transform.DOMove(GameCtr.instance.gridManager.GetPieceInGrid(b.col, b.row).transform.position, 0.15f).OnComplete(() => {
            if (b.canChange)
            {
                b.sr.enabled = true;
            }
        });
        ResetSquareInBlock(b);
        SetTextNumberRedoing();
    }

    void ResetSquareInBlock(Block b)
    {
        foreach (var transfS in b.squares)
        {
            SquareInBlock s = transfS.GetComponent<SquareInBlock>();
            int rcheck = (int)(b.row + s.localPos.y);
            int ccheck = (int)(b.col + s.localPos.x);
            s.boxColli.isTrigger = true;
            s.sprite.enabled = true;
            s.pieceInGrid.SetOnOffSprite(false);
            s.pieceInGrid.sib = null;
            ResetPieceInGrid(s.pieceInGrid.col, s.pieceInGrid.row);
            var pig = GameCtr.instance.gridManager.GetPieceInGrid(Mathf.RoundToInt(ccheck), Mathf.RoundToInt(rcheck));
            s.SetPiece(pig);
            b.OnMoveSprite(false);
        }
    }

    void ResetPieceInGrid(int c, int r)
    {
        for (int i = r; i >= 0; i--)
        {
            if (GameCtr.instance.gridManager.GetPieceInGrid(c, i).sib != null || GameCtr.instance.gridManager.GetPieceInGrid(c, i).ismap) break;
            GameCtr.instance.gridManager.GetPieceInGrid(c, i).hasSquare = false;
        }
    }
}
