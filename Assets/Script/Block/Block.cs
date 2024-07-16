using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Block : MonoBehaviour
{
    public GameObject squarePrefab;
    public List<Transform> squares = new List<Transform>();
    public SpriteRenderer sr;
    public Sprite sprite;
    public int col;
    public int row;
    public int spriteIndex;
    public bool canClick = true;
    public bool canChange = false;
    


    public void SetDefault()
    {
        sr.enabled = false;
        squares.Clear();
        canClick = true;
        canChange = false;
    }
    public SquareInBlock SpawnSquare(float x, float y)
    {
        var tempSquare = GameCtr.instance.squareInBlock.Spawn(transform);
        tempSquare.SetActive(true);
        SquareInBlock sib = tempSquare.GetComponent<SquareInBlock>();
        sib.sprite.sprite = sprite;
        sib.SetDefault();
        sib.SetPos(x, y);
        squares.Add(tempSquare.transform);
        return tempSquare.GetComponent<SquareInBlock>();
    }

    public void OnMoveSprite(bool isActive)
    {
        foreach (var s in squares)
        {
            s.GetComponent<SquareInBlock>().moveSprite.gameObject.SetActive(isActive);
        }
    }

    public Vector3 GetCenter()
    {
        Vector3 center = Vector3.zero;
        squares.ForEach(s =>
        {
            center += s.position;
            //Debug.Log(s.position);
        });
        center = new Vector3(center.x/squares.Count, center.y/squares.Count,0);
        //Debug.Log(center);

        return center;
    }
    public int GetNumberCol()
    {
        int max = -10;
        int min = 10;
        foreach (var s in squares)
        {
            var ctr = s.GetComponent<SquareInBlock>();
            if (ctr.localPos.x < min) min = (int)ctr.localPos.x;
            if (ctr.localPos.x > max) max = (int)ctr.localPos.x;
        }
        return max-min+1;
    }
    
    public void Desawn()
    {
        squares.ForEach(s => { s.Recycle(); });
        squares.Clear();
        gameObject.Recycle();
    }
}
