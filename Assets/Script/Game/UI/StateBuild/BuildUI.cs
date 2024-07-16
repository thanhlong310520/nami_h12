using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BuildUI : MonoBehaviour
{
    [SerializeField] GameObject itemTaskPrefab;
    [SerializeField] Transform content;
    [SerializeField] Transform holderCoin;
    [SerializeField] GameObject taskPanel;
    [SerializeField] ButtonOnOffTask closeTask;
    [SerializeField] ChapterButton chapterButton;
    [SerializeField] List<Image> listfillSprite = new List<Image>();
    [SerializeField] TMP_Text textprogess;
    [SerializeField] int numberTask;
    [SerializeField] int numberTaskDone;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] GameObject blockingBlock;

    public void CreatedTask(ModelCtr modelCtr)
    {
        numberTask = modelCtr.pieceModels.Count;
        numberTaskDone = 0;
        bool isCreate = false;
        foreach (var piece in modelCtr.pieceModels)
        {
            print(piece.name + piece.isActive);
            if (piece.isActive)
            {
                numberTaskDone++;
                continue;
            }
            if(!isCreate)
            {
                isCreate = true;
                var tempItem = itemTaskPrefab.Spawn(content);
                var itemtask = tempItem.GetComponent<ItemTask>();
                SetSizeItem(itemtask);
                itemtask.SetItem(piece);
                itemtask.buildUi = this;
            }
        }
        SetFill();
        CheckDoneChapter();
        CheckHaveTask();
        closeTask.callback = OnOffTaskPanel;
        chapterButton.callback = OnOffTaskPanel;
    }

    void SetSizeItem(ItemTask item)
    {
        var rectItem = item.GetComponent<RectTransform>();
        float widthContent = content.GetComponent<RectTransform>().rect.width;
        float left = content.GetComponent<VerticalLayoutGroup>().padding.left;
        float right = content.GetComponent<VerticalLayoutGroup>().padding.right;
        rectItem.sizeDelta = new Vector2(widthContent-left-right,rectItem.sizeDelta.y);
        print("sizeeeeee" + widthContent);
    }

    public void AddTaskDone()
    {
        numberTaskDone++;
        SetFill();

    }

    void CheckDoneChapter()
    {
        if(numberTaskDone >= numberTask)
        {
            chapterButton.SetDoneChapter(true);
        }
        else
        {
            chapterButton.SetDoneChapter(false);
        }
    }

    void SetFill()
    {
        textprogess.text = $"{numberTaskDone}/{numberTask}";
        //fillSprite.fillAmount = (float)numberTaskDone/numberTask;
        listfillSprite.ForEach((fillSprite) =>
        {
            fillSprite.DOFillAmount((float)numberTaskDone / numberTask, 0.1f);
        });
    }

    public void OnOffTaskPanel(bool isOn)
    {
        SetFill();
        taskPanel.SetActive(isOn);
        if(isOn)
        {
            blockingBlock.SetActive(false);
            CheckCanClickItem();
        }
    }
    void CheckCanClickItem()
    {
        for (int i = 0; i < content.childCount; i++)
        {
            var itemTask = content.GetChild(i).GetComponent<ItemTask>();
            bool canclick = false;
            if (GameManager.instance.playerData.coinNumber < itemTask.currentPiece.Cost)
            {
                canclick = false;
            }
            else canclick = true;
            itemTask.bt.SetOnClickButton(canclick);
        }
    }
    public void CheckHaveTask()
    {
        bool haveTask = false;
        if (content.childCount <= 0) haveTask = false;
        for (int i = 0; i < content.childCount; i++)
        {
            var itemTask = content.GetChild(i).GetComponent<ItemTask>();
            if (GameManager.instance.playerData.coinNumber >= itemTask.currentPiece.Cost)
            {
                haveTask = true;
            };
        }
        chapterButton.HaveTask(haveTask);
    }

    //for fillcoin;
    public int numberfill = 10;
    int currentfill = 0;
    public void FillCoin(Vector3 endPos,UnityAction callback)
    {
        blockingBlock.SetActive(true);
        currentfill = 0;    
        var delay = 0f;
        for(int i = 0; i < numberfill; i++)
        {

            var tempcoit = coinPrefab.Spawn(holderCoin);
            tempcoit.transform.position = UICtr.instance.coinctr.target.position;//Camera.main.WorldToScreenPoint(UICtr.instance.coinctr.target.position);
            tempcoit.transform.localScale = Vector3.one;
            tempcoit.transform.DOMove(endPos, 0.25f).SetDelay(delay).OnComplete(() =>
            {
                tempcoit.transform.DOScale(0, 0.1f).OnComplete(() =>
                {
                    CheckDoneFillCoin(callback);
                    tempcoit.Recycle();
                });
            });
            
            delay += 0.1f;
        }
    }
    void CheckDoneFillCoin(UnityAction callback)
    {
        currentfill++;
        if(currentfill >= numberfill) 
        {
            CreatedTask(GameManager.instance.currentMode.GetComponent<ModelCtr>());
            CheckCanClickItem();
            callback?.Invoke(); 
            CheckHaveTask();
        }
    }
}
