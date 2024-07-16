using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCtr : MonoBehaviour
{
    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered
    public float persentScreen = 5;
    void Start()
    {
        dragDistance = Screen.height * persentScreen / 100; //dragDistance is 15% height of the screen
    }

    void Update()
    {
        if (GameCtr.instance.GetCanTouch() && !GameCtr.instance.isTutarial)
        {
            CheckTouch();
        }
    }

    void CheckTouch()
    {
        if (Input.touchCount == 1) // user is touching the screen with a single touch
        {
            Touch touch = Input.GetTouch(0); // get the touch

            if (touch.phase == TouchPhase.Began) //check for the first touch
            {
                fp = touch.position;
                lp = touch.position;
                SelectBlock(touch.position);
            }
            else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
            {
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
            {
                lp = touch.position;  //last touch position. Ommitted if you use list

                CheckState();  
            }
        }
    }

    void CheckState()
    {
        //Check if drag distance is greater than 20% of the screen height
        if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
        {
            if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
            {   
                if ((lp.x > fp.x)) 
                {   //Right swipe
                   // Debug.Log("Right Swipe");
                    GameCtr.instance.handleBlock.FlipBlock();
                }
                else
                {   //Left swipe
                    //Debug.Log("Left Swipe");
                    GameCtr.instance.handleBlock.FlipBlock();
                }
            }
            else
            {   
                if (lp.y > fp.y)  //If the movement was up
                {   //Up swipe
                    //Debug.Log("Up Swipe");
                }
                else
                {
                    GameCtr.instance.handleBlock.MovedownBlock();
                    //Debug.Log("Down Swipe");
                }
            }
        }
        else
        {
            GameCtr.instance.handleBlock.RotateBlock();
            //Debug.Log("Tap");
        }
    }

    void SelectBlock(Vector3 pos)
    {
        pos.z = Camera.main.nearClipPlane;
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction, int.MaxValue, LayerMask.GetMask("Block"));
        if (hits.Length <= 0) return;
        SquareInBlock selectSquare = hits[0].collider.GetComponent<SquareInBlock>();
        if (selectSquare == null) return;
        Block selectBlock = selectSquare.transform.parent.GetComponent<Block>();
        if (selectBlock == null) return;
        if (!selectBlock.canClick) return;
        GameCtr.instance.handleBlock.currentBlock = selectBlock;
        print(selectBlock);
    }
}
