using UnityEngine;

public class HandleBlock : MonoBehaviour
{
    public Block currentBlock;
    // Start is called before the first frame update
    void Start()
    {
        //RenderSib();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    RotateBlock();
        //}
        //if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        //{
        //    FlipBlock();
        //}
    }
    public void MovedownBlock()
    {
        if (currentBlock == null) return;
        GameCtr.instance.MoveDownBlock(currentBlock);
        ResetBlock();
    }

    public void FlipBlock()
    {
        //float xmin = 100, xmax = -100;
        //foreach (var s in b.squares)
        //{
        //    if(xmin > s.localPosition.x) xmin = s.localPosition.x;
        //    if(xmax < s.localPosition.x) xmax = s.localPosition.x;
        //}
        //int sup = (int)(xmax + xmin);
        if (currentBlock == null) return;
        if (!currentBlock.canChange) return;
        foreach (var s in currentBlock.squares)
        {
            var sib = s.GetComponent<SquareInBlock>();
            var localpos = new Vector2(sib.localPos.x * (-1), sib.localPos.y);
            //var localpos = new Vector2(sib.localPos.x * (-1) + sup, sib.localPos.y);
            sib.SetPos(Mathf.RoundToInt(localpos.x), Mathf.RoundToInt(localpos.y));
        }
        ResetBlock();


    }
    public void RotateBlock()
    {
        if (currentBlock == null) return;
        if(!currentBlock.canChange) return;
        foreach (var s in currentBlock.squares)
        {
            var sib = s.GetComponent<SquareInBlock>();
            Vector2 localPos = GetPosAfterRotate(sib.localPos);
            sib.SetPos(Mathf.RoundToInt(localPos.x), Mathf.RoundToInt(localPos.y));
        }
        ResetBlock();
    }

    Vector2 GetPosAfterRotate(Vector2 pos)
    {
        // tinh tien ve tam
        //pos = new Vector2(pos.x - translationVector.x, pos.y - translationVector.y);
        Vector2 newPos = Vector2.zero;
        //quay
        float angle = -90;
        newPos.x = pos.x * Mathf.Cos(Mathf.Deg2Rad * angle) - pos.y * Mathf.Sin(Mathf.Deg2Rad * angle);
        newPos.y = pos.x * Mathf.Sin(Mathf.Deg2Rad * angle) + pos.y * Mathf.Cos(Mathf.Deg2Rad * angle);
        // tra lai ve toa do cu
        //newPos = new Vector2(newPos.x + translationVector.x, newPos.y + translationVector.y);
        return newPos;
    }

    void ResetBlock()
    {
        currentBlock = null;
    }
}
