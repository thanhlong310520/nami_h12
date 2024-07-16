using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCtr : MonoBehaviour
{

    public GameObject blockPrefab;
    public Transform holderBlock;
    public GridManger gridManager;
    public ObjectCtr objectCtr;
    public GameObject squareInBlock;
    public List<GameObject> fxPrefab = new List<GameObject>();
    public PointWin pointWin;
    public ContaintCtr contain;
    public HandleBlock handleBlock;
    public ParticleSystem winFx;
    public List<Sprite> sprites = new List<Sprite>();
    public List<Block> listBlock = new List<Block>();
    public List<Block> listBlockDone = new List<Block>();
    public Transform containt;

    

    [SerializeField] LevelData levelData;
    DataLevel dataLevelMap;
    public bool isTutarial = false;
    bool canTouch = false;


    public static GameCtr instance;
    private void Awake()
    {
        instance = this;
        Input.multiTouchEnabled = false;
    }
   

    // Update is called once per frame
    void Update()
    {
    }

    public void SetUpLevel(int lv)
    {
        //change.Change();
        var text = levelData.listLevel[lv - 1];
        var dataLv = JsonUtility.FromJson<DataLevel>(text.text);
        dataLevelMap = dataLv;
        gridManager.SetPos(dataLv.colNumber, dataLv.rowNumber);
        // initialization Grid
        gridManager.SetUpLevel(dataLv.colNumber, dataLv.rowNumber);
        contain.ChangeSpriteContaint(dataLv.rowNumber, dataLv.colNumber);
        gridManager.SetDonePieceInGrid(dataLv.listPigDone);
        CreateBlockLevel(dataLv);
        TutorialCtr.instance.SratTutorial(text);
    }

    void CreateBlockLevel(DataLevel dataLv)
    {

        foreach (var b in dataLv.blocks)
        {
            //GameObject tempBlock = Instantiate(blockPrefab);

            GameObject tempBlock = blockPrefab.Spawn();
            tempBlock.transform.SetParent(holderBlock);
            var blockCtr = tempBlock.GetComponent<Block>();
            tempBlock.gameObject.SetActive(true);
            blockCtr.SetDefault();
            int tRow = b.row + dataLv.rowNumber + 3;
            int tCol = b.col;
            var piece = gridManager.GetPieceInGrid(tCol, tRow);
            if (piece != null)
            {
                blockCtr.transform.position = piece.transform.position;
                blockCtr.row = tRow;
                blockCtr.col = tCol;
                blockCtr.sprite = sprites[b.sprite];
                foreach (var s in b.listPosSquare)
                {
                    var sib = blockCtr.SpawnSquare(s.x, s.y);
                    int x = (int)(tCol + s.x);
                    int y = (int)(tRow + s.y);
                    sib.SetPiece(gridManager.GetPieceInGrid(x, y));

                }
            }
            blockCtr.canChange = b.canChange;
            blockCtr.sr.enabled = b.canChange;   
            listBlock.Add(blockCtr);
        }
    }
    public void MoveDownBlock(Block b)
    {
        FindPosBlock(b);
    }

    void FindPosBlock(Block block)
    {
        GameManager.instance.audioManager.PlaySound(6);
        SetCanTouch(false);
        for (int row = 0; row < gridManager.listPieceInGrid.Count; row++)
        {
            bool canStay = true;
            foreach (var transfS in block.squares)
            {
                SquareInBlock s = transfS.GetComponent<SquareInBlock>();
                int rcheck = (int)(row + s.localPos.y);
                int ccheck = (int)(block.col + s.localPos.x);
                if (CheckHasSquare(ccheck, rcheck)) canStay = false;
            }
            if (canStay)
            {
                MoveBlock(block, block.col, row);
                break;
            }
        }
    }
    bool CheckHasSquare(int c, int r)
    {
        if (c < 0 || r < 0) return true;
        var tempPiece = gridManager.GetPieceInGrid(c, r);

        if (tempPiece.hasSquare)
        {
            return true;
        }
        return false;

    }

    void MoveBlock(Block b, int c, int r)
    {
        b.canClick = false;
        b.OnMoveSprite(true);
        b.sr.enabled = false;
        listBlockDone.Add(b);
        b.transform.DOMove(gridManager.GetPieceInGrid(c, r).transform.position, 0.15f).OnComplete(() =>
        {
            GameManager.instance.Vibrate();
            foreach (var transfS in b.squares)
            {
                SquareInBlock s = transfS.GetComponent<SquareInBlock>();
                int rcheck = (int)(r + s.localPos.y);
                int ccheck = (int)(c + s.localPos.x);
                s.SetDone();
                var pig = gridManager.GetPieceInGrid(ccheck, rcheck);
                s.SetPiece(pig);
                pig.SetOnOffSprite(true);
                pig.SetSIB(s);

                SetPieceInGrid(ccheck, rcheck);
                b.OnMoveSprite(false);
            }
            StartCoroutine(SpawnEffect(b));
            CheckCoincidePiece(b);
        });
    }

    void SetPieceInGrid(int c, int r)
    {
        for (int i = r; i >= 0; i--)
        {
            gridManager.GetPieceInGrid(c, i).hasSquare = true;
        }
    }
    void CheckCoincidePiece(Block b)
    {
        bool over = false;
        List<Block> listB = new List<Block>();
        foreach (var s in b.squares)
        {
            listBlock.ForEach(bcheck =>
            {
                if (bcheck.canClick)
                {
                    bool canAdd = false;
                    bcheck.squares.ForEach(scheck =>
                    {
                        if (s.GetComponent<SquareInBlock>().pieceInGrid == scheck.GetComponent<SquareInBlock>().pieceInGrid)
                        {
                            canAdd = true;
                            over = true;
                            // game over
                            Debug.Log("gameover");
                        }
                    });
                    if (canAdd) { listB.Add(bcheck); }
                }
            });
        }
        if (!over) CheckDone();
        else
        {
            StartCoroutine(HandleCoincidePiece(listB));
        }
    }
    IEnumerator HandleCoincidePiece(List<Block> listBlock)
    {
        foreach (var b in listBlock)
        {
            foreach (var s in b.squares)
            {
                s.GetComponent<SquareInBlock>().anim.Play("SquareInBlock");
            }

        }
        yield return new WaitForSeconds(1f);
        foreach (var b in listBlock)
        {
            foreach (var s in b.squares)
            {
                s.GetComponent<SquareInBlock>().anim.Play("Idle");
                s.GetComponent<SquareInBlock>().sprite.color = Color.white;
            }

        }
        GameOver();
    }
    void CheckDone()
    {
        SetCanTouch(true);
        bool done = true;
        foreach (var b in listBlock)
        {
            if (b.canClick) done = false;
        }
        if (done)
        {
            CheckLose();
            listBlockDone.Clear();
            //StartCoroutine(MoveObject());
        }
    }
    void CheckLose()
    {
        bool isHasSib = false;
        for(int i = 0; i < gridManager.listPieceInGrid[dataLevelMap.rowNumber].Count; i++)
        {
            PieceInGrid temppig = gridManager.listPieceInGrid[dataLevelMap.rowNumber][i];
            if (temppig.hasSquare) isHasSib = true;
        }
        if (isHasSib) GameOver();
        else StartCoroutine(Win());
    }
    IEnumerator MoveObject()
    {
        UICtr.instance.mask.parent.gameObject.SetActive(true);
        if(objectCtr == null)
        {
            objectCtr = GameObject.FindGameObjectWithTag("Car").GetComponent<ObjectCtr>();
        }
        yield return new WaitForSeconds(0.25f);
        objectCtr?.particleSmoke?.Play();
        GameManager.instance.audioManager.PlaySound(2);
        yield return new WaitForSeconds(0.6f);
        objectCtr?.Move();
    }
    public void GameOver()
    {
        StartCoroutine(ShowLose());
    }

    IEnumerator ShowLose()
    {
        pointWin.Lose();
        SetCanTouch(false);
        GameManager.instance.audioManager.PlaySound(5);
        yield return new WaitForSeconds(0.1f);
        UICtr.instance.ShowPopup(UICtr.instance.losePanel,null,null);
        yield return new WaitForSeconds(1f);
        objectCtr.firePS?.Stop();


    }
    IEnumerator SpawnEffect(Block b)
    {
        var tempPrefab = GetGameFx(b.GetNumberCol());
        var tempFx = Instantiate(tempPrefab);
        tempFx.transform.position = b.GetCenter();
        tempFx.SetActive(true);
        yield return new WaitForSeconds(3f);
        Destroy(tempFx);
    }

    public IEnumerator Win()
    {
        GameManager.instance.audioManager.PlaySound(4);
        SetCanTouch(false);
        yield return new WaitForSeconds(0.25f);
        winFx.Play();
        pointWin.Win();
        objectCtr.sprite.sprite = objectCtr.spriteCar[1];

        yield return new WaitForSeconds(1.5f);

        UICtr.instance.ShowPopup(UICtr.instance.winPanel, () =>
        {
            UICtr.instance.nextbt.localScale = Vector3.zero;
            UICtr.instance.coinctr.SetDefaultCoin();
        }, () => { StartCoroutine(UICtr.instance.ShowNextButton()); });
        //UICtr.instance.ShowPopup(UICtr.instance.winPanel, () => {
        //    UICtr.instance.nextbt.localScale = Vector3.zero;
        //    UICtr.instance.coinctr.SetDefaultCoin();
        //},null);
    }

    public void SetDefaultValue()
    {

        pointWin.StartLevel();
        listBlock.ForEach(b =>
        {
            b.Desawn();
        });
        listBlock.Clear();
        listBlockDone.Clear();
        objectCtr.SetDefault();
        if (GameManager.instance.currentLevel > levelData.listLevel.Count)
        {
            int tempLv = UnityEngine.Random.Range(10, levelData.listLevel.Count);
            SetUpLevel(tempLv);
        }
        else
        {
            SetUpLevel(GameManager.instance.currentLevel);
        }

        //InterAds
        GameManager.instance.ShowAdsInter();
    }
    GameObject GetGameFx(int colNumber)
    {
        if (colNumber < 3) return fxPrefab[0];
        if (colNumber < 5) return fxPrefab[1];
        return fxPrefab[2];
    }
    public void NextLevel()
    {
        SetDefaultValue();
    }

    public void SetCanTouch(bool ct)
    {
        canTouch = ct;
    }
    public bool GetCanTouch()
    {
        return canTouch;
    }
    
}
