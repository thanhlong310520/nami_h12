using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using System;

public class GridManger : MonoBehaviour
{
    public GameObject piecePrefab;
    public int colNumber;
    public int rowNumber;
    [ShowInInspector] public List<List<PieceInGrid>> listPieceInGrid = new List<List<PieceInGrid>>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetUpLevel(int col, int row)
    {
        colNumber = col;
        rowNumber = row;
        RemovePiece();
        listPieceInGrid.Clear();
        listPieceInGrid = new List<List<PieceInGrid>>();
        for (int i = 0; i < (rowNumber * 4+3); i++)
        {
            var tempList = new List<PieceInGrid>();
            for (int j = 0; j < colNumber; j++)
            {
                var tempPiece = SpawnPiece(j, i);

                tempList.Add(tempPiece.GetComponent<PieceInGrid>());
            }
            listPieceInGrid.Add(tempList);
        }
    }

    public void SetDonePieceInGrid(List<PosPieceInGridDone> listDone)
    {
        if (listDone == null) return;
        foreach (var d in listDone)
        {
            var p = GetPieceInGrid(d.col,d.row);
            p.hasSquare = true;
            p.ismap = true;
            p.spriteFront.enabled = true;
        }
    }
    GameObject SpawnPiece(int col, int row)
    {
        int halfCol = colNumber / 2;
        var tempPiece = piecePrefab.Spawn(transform);
        tempPiece.transform.localPosition = new Vector3(col - halfCol, row, 0);
        var pig = tempPiece.GetComponent<PieceInGrid>();
        pig.SetDefault();
        pig.row = row;
        pig.col = col;
        pig.ismap = false;
        pig.SetOnOffSprite(false);
        if(row < rowNumber) pig.spriteBack.enabled = true;
        else pig.spriteBack.enabled = false;
        tempPiece.SetActive(true);
        return tempPiece;
    }
    void RemovePiece()
    {
        foreach (var list in listPieceInGrid)
        {
            foreach (var p in list)
            {
                p.gameObject.Recycle();
            }
        }
    }

    public PieceInGrid GetPieceInGrid(int col, int row)
    {
        if (row > listPieceInGrid.Count) return null;
        if (col > listPieceInGrid[0].Count) return null;
        return listPieceInGrid[row][col];
    }

    public void SetPos(int c, int r)
    {
        float x = 0;
        if((c%2) == 0)
        {
            x = 0.5f;
        }
        else
        {
            x = 0;
        }
        float y = GameCtr.instance.containt.position.y + 0.5f - r;
        transform.position = new Vector3(x,y,transform.position.z);
    }
}
