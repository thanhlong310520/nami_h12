using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceInGrid : MonoBehaviour
{
    public int col;
    public int row;
    public SpriteRenderer spriteFront;
    public SpriteRenderer spriteBack;
    public bool hasSquare = false;
    public SquareInBlock sib;
    public bool ismap = false;

    public void SetDefault()
    {
        sib = null;
        hasSquare = false;
        ismap = false;
        spriteFront.enabled = false;
    }

    public void SetSprite(Sprite s)
    {
        spriteFront.sprite = s;
    }
    public void SetSIB(SquareInBlock s)
    {
        sib = s;
        spriteFront.sprite = s.sprite.sprite;
    }
    public void SetOnOffSprite(bool isOn)
    {
        spriteFront.enabled = isOn;
    }
}
