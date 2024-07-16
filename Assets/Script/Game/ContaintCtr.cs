using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContaintCtr : MonoBehaviour
{
    [SerializeField] Transform left;
    [SerializeField] Transform right;
    [SerializeField] Transform bot;
    [SerializeField] List<BoxCollider2D> colliders;
    public void ChangeSpriteContaint(int row, int col)
    {
        string name = $"{row}x{col}";
        left.localPosition = new Vector3(-(float)col/2,0,0);
        right.localPosition = new Vector3((float)col/2,0,0);
        bot.localPosition = new Vector3(0,-row,0);
        ChangetColli(col,row);
        
    }
    void ChangetColli(int col,int row)
    {
        float ground = 17 - col;
        foreach (var c in colliders)
        {
            c.size = new Vector2(ground / 2, 0.2f);
        }
        colliders[0].offset = new Vector2(ground / 4, -0.1f);
        colliders[1].offset = new Vector2(-ground / 4, -0.1f);
        colliders[2].size = new Vector2(1, row);
        Transform piece1 = GameCtr.instance.gridManager.GetPieceInGrid(col - 1, row - 1).transform;
        Transform piece2 = GameCtr.instance.gridManager.GetPieceInGrid(col - 1, 0).transform;
        Vector3 pos = new Vector3((piece1.position.x + piece2.position.x) / 2, (piece1.position.y + piece2.position.y) / 2);
        colliders[2].transform.position = pos;
    }
}
