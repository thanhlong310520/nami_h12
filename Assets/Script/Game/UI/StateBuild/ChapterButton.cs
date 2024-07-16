using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChapterButton : ButtonOnOffTask
{
    public bool isNext = false;
    public TMP_Text text;
    public GameObject notificationHaveTask;
    public Image bgImage;
    public GameObject objectProgress;
    public List<Sprite> sprites = new List<Sprite>();
    protected override void Handle()
    {
        if (isNext)
        {
            isOn = false;
            HandleNext();
        }
        else
        {
            isOn = true;
        }
        base.Handle();
    }

    void HandleNext()
    {
        isNext = false;
        GameManager.instance.playerData.currentModelIndex++;
        GameManager.instance.playerData.listPiectModelActive.Clear();
        GameManager.instance.SaveData();
        GameManager.instance.SetCurrentMap();
        GameManager.instance.ShowMap(GameManager.instance.playerData.currentModelIndex,StateShowMap.next);
    }

    public void SetDoneChapter(bool isDone)
    {
        isNext =isDone;
        if(isDone)
        {
            text.text = "Next";
            bgImage.sprite = sprites[1];
            objectProgress.SetActive(false);
        }
        else
        {
            objectProgress.SetActive(true);
            bgImage.sprite = sprites[0];
            text.text = $"Chapter {GameManager.instance.playerData.currentModelIndex+1}";
        }
    }

    public void HaveTask(bool ishas)
    {
        notificationHaveTask.SetActive(ishas);
    }
}
