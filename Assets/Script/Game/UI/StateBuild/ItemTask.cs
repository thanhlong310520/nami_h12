using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemTask : MonoBehaviour
{
    public ConfirmTaks bt;
    public Image icon;
    public TMP_Text text;
    public TMP_Text costTxt;
    public BuildUI buildUi;
    public PieceModel currentPiece;

    public void SetItem(PieceModel piece)
    {
        currentPiece = piece; 
        bt.canClick = true;
        bt.callback.RemoveAllListeners();
        bt.callback.AddListener(() =>
        {
            StartCoroutine(Handle(piece));
        });
        icon.sprite = piece.icon;
        text.text = piece.textTask;
        costTxt.text = piece.Cost.ToString();
    }

    IEnumerator Handle(PieceModel piece)
    {
        yield return null;
        var isfill = piece.CheckCanfill();
        if (isfill)
        {
            buildUi.AddTaskDone();
            bt.canClick = false;
            bt.callback.RemoveAllListeners();
            // fill coin
            piece.ActiveModel();

            buildUi.FillCoin(bt.transform.position, () => {
                buildUi.OnOffTaskPanel(false);
                gameObject.Recycle();
                piece.FillColorForMesh();
            });
            
        }
    }
}
