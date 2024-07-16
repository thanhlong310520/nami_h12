using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngineInternal;

public class ToolMap : MonoBehaviour
{
    public static ToolMap instance;
    private void Awake()
    {
        ToolMap.instance = this;
    }
    public GridManger gridManger;
    public TMP_InputField colInput;
    public TMP_InputField rowInput;
    public TMP_InputField output;
    public Image image;
    public DataLevel level = new DataLevel();
    public Block blockPrefab;
    public GameObject prefab;
    public HandleBlock handleBlock;

    public Block currentBlock;


    public bool isCreatBlock = false;
    public bool isDestroy = false;
    public bool isSetPos = false;
    public bool isSetPig = false;
    public int spriteIndex = 0;
    public List<SquareInBlock> squareInBlocks;
    public List<GameObject> blocks;
    public List<PieceInGrid> listPigDone;

    // Start is called before the first frame update
    //void Start()
    //{
    //    listPigDone = new List<PieceInGrid>();
    //    ChangeImage("1");
    //    level = new DataLevel();
    //    var text = Resources.Load("test") as TextAsset;
    //    Debug.Log(text);
    //    blockPrefab.TestJson(text);
    //}

    ////Update is called once per frame
    //void Update()
    //{
    //    MoveBlock();
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //        if (isDestroy)
    //        {
    //            RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction, int.MaxValue, LayerMask.GetMask("Block"));
    //            if (hits.Length > 0)
    //            {
    //                blocks.Remove(hits[0].collider.transform.parent.gameObject);
    //                Destroy(hits[0].collider.transform.parent.gameObject);
    //            }

    //        }
    //        if (isSetPos)
    //        {
    //            RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction, int.MaxValue, LayerMask.GetMask("Grid"));
    //            foreach (var item in hits)
    //            {
    //                SetPos(item.collider.GetComponent<PieceInGrid>());
    //                Debug.Log(item.collider.name);
    //            }

    //        }
    //        if (isSetPig)
    //        {
    //            RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction, int.MaxValue, LayerMask.GetMask("Grid"));
    //            if (hits.Length > 0)
    //            {
    //                SetDonePiece(hits[0].collider.GetComponent<PieceInGrid>());
    //            }
    //        }
    //    }
    //    if (Input.GetKeyDown(KeyCode.D))
    //    {
    //        DestroyBlock();
    //    }
    //}

    void SetDonePiece(PieceInGrid pig)
    {
        if (pig.hasSquare)
        {
            listPigDone.Remove(pig);
            pig.hasSquare = false;
            pig.spriteFront.color = Color.white;
        }
        else
        {
            listPigDone.Add(pig);
            pig.hasSquare = true;
            pig.spriteFront.color = Color.black;
        }
    }
    void MoveBlock()
    {
        if (currentBlock == null) return;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        currentBlock.transform.position = mousePos;

    }

    public void RenderGrid()
    {
        int c = int.Parse(colInput.text);
        int r = int.Parse(rowInput.text);
        level.rowNumber = r;
        level.colNumber = c;
        gridManger.SetUpLevel(c, r);
    }
    public void SetDonePig()
    {
        listPigDone.Clear();
        isSetPig = true;
        isCreatBlock = false;
        isSetPos = false;

        isDestroy = false;
    }
    public void DonePiece()
    {
        isSetPig = false;
        foreach (var pd in listPigDone)
        {
            PosPieceInGridDone p = new PosPieceInGridDone();
            p.col = pd.col;
            p.row = pd.row;
            pd.spriteFront.color = Color.white;
            level.listPigDone.Add(p);
        }
        listPigDone.Clear();
    }
    public void CreateBlock()
    {
        squareInBlocks.Clear();
        blockPrefab.squares.ForEach(s =>
        {
            s.GetComponent<SquareInBlock>().ResetSelect();
        });
        isCreatBlock = true;
        isDestroy = false;
        isSetPos = false;
        isSetPig = false;
        blockPrefab.gameObject.SetActive(true);
    }
    public void Done()
    {
        if (squareInBlocks.Count <= 0) return;
        if (currentBlock != null)
        {
            Destroy(currentBlock.gameObject);
            currentBlock = null;
        }
        isCreatBlock = false;
        isSetPos = true;
        GameObject tempBlock = Instantiate(prefab);
        tempBlock.transform.SetParent(transform);
        var blockCtr = tempBlock.GetComponent<Block>();
        blockCtr.sprite = image.sprite;
        blockCtr.spriteIndex = spriteIndex;
        for (int i = 0; i < squareInBlocks.Count; i++)
        {
            var tempPiece = Instantiate(squareInBlocks[i].gameObject, tempBlock.transform);
            SquareInBlock s = tempPiece.GetComponent<SquareInBlock>();
            s.sprite.sprite = GameCtr.instance.sprites[spriteIndex];
            tempPiece.SetActive(true);
            blockCtr.squares.Add(tempPiece.transform);
        }
        currentBlock = blockCtr;


        blockPrefab.gameObject.SetActive(false);
        handleBlock.currentBlock = currentBlock;
    }
    void DestroyBlock()
    {
        isDestroy = true;
        if (currentBlock != null)
        {
            Destroy(currentBlock.gameObject);
            currentBlock = null;
        }
    }

    void SetPos(PieceInGrid pig)
    {
        isSetPos = false;
        if (currentBlock == null) return;
        currentBlock.transform.position = pig.transform.position;
        currentBlock.col = pig.col;
        currentBlock.row = pig.row;
        blocks.Add(currentBlock.gameObject);
        currentBlock = null;

    }
    public void DoneLevel()
    {

        blocks.ForEach(b => {
            level.Add(b.GetComponent<Block>());
            Destroy(b.gameObject);
        });
        string txt = JsonUtility.ToJson(level, true);
        output.text = txt;
        Debug.Log(txt);
        blocks.Clear();
        level = new DataLevel();
        int c = int.Parse(colInput.text);
        int r = int.Parse(rowInput.text);
        level.rowNumber = r;
        level.colNumber = c;

    }

    public void ChangeImage(string data)
    {
        spriteIndex = int.Parse(data) - 1;
        image.sprite = GameCtr.instance.sprites[spriteIndex];
    }

}
