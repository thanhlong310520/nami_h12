using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ModelCtr : MonoBehaviour
{
    public List<PieceModel> pieceModels = new List<PieceModel>();
    public Canvas canvas;
    public string name = "";
    public int indexModel;
    Tween moveTween;
    private void Start()
    {
    }
    public void AddPieceToData(PieceModel piece)
    {
        int index = -1;
        index = pieceModels.IndexOf(piece);
        if(index >= 0) { 
            GameManager.instance.playerData.listPiectModelActive.Add(index);
            GameManager.instance.AddCoin(-piece.Cost);
            print("cost " + piece.Cost);
            GameManager.instance.SaveData();
        }
    }

    public void LoadModel(Camera cam)
    {
        if(canvas != null)
        {
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = cam;
        }
        for(int i = 0; i < pieceModels.Count; i++) 
        {
            pieceModels[i].modelCtr = this;
            pieceModels[i].SetStart(CheckActive(i));
        }
    }

    bool CheckActive(int i)
    {
        if (indexModel > GameManager.instance.playerData.currentModelIndex) return false;
        else if (indexModel < GameManager.instance.playerData.currentModelIndex) return true;
        else
        {
            bool isActive = false;
            foreach (var index in GameManager.instance.playerData.listPiectModelActive)
            {
                if(index == i) isActive = true;
            }
            return isActive;
        }
    }

    public void ShowTween(float posX,UnityAction callback)
    {
        if(moveTween != null)
        {
            moveTween.Kill();
        }
        moveTween = transform.DOMoveX(posX, 0.5f).OnComplete(() => { callback?.Invoke(); });
    }
}
