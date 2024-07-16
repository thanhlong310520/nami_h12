using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SquareInBlock : MonoBehaviour
{
    public Vector2 localPos;
    public SpriteRenderer sprite;
    public SpriteRenderer moveSprite;
    public Animator anim;

    public BoxCollider2D boxColli;

    public bool isSelect = false;
    public PieceInGrid pieceInGrid;


    public void SetDefault()
    {
        moveSprite.enabled = false;
        sprite.enabled = true;
        boxColli.isTrigger = true;
        isSelect = false;
        anim.StopPlayback();    
        sprite.color = Color.white;
        print(sprite.color);    
        pieceInGrid = null;

    }
    public void SetDone()
    {
        sprite.enabled = false;
        boxColli.isTrigger = false;
    }

    public void SetPiece(PieceInGrid p)
    {
        pieceInGrid = p;
    }
    public void SetPos(float x, float y)
    {
        transform.localPosition = new Vector3(x, y, 0);
        //transform.DOLocalMove(new Vector3(x, y, 0), 0.15f);
        localPos = new Vector2(x, y);
    }


    // for Tool
    public void Click()
    {
        if(ToolMap.instance.isCreatBlock)
        {
            Select();
        }
        
    }
    void Select()
    {
        isSelect = !isSelect;
        if (isSelect)
        {
            sprite.color = Color.yellow;
            ToolMap.instance.squareInBlocks.Add(this);
        }
        else
        {
            sprite.color = Color.white;
            ToolMap.instance.squareInBlocks.Remove(this);
        }
    }
    public void ResetSelect()
    {
        isSelect = false;
        sprite.color = Color.white;
    }

}
