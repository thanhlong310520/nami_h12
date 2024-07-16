using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleInput : MonoBehaviour
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
        CheckTouch();
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
                SelectButton(fp);
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
        if (Mathf.Abs(lp.x - fp.x) < dragDistance && Mathf.Abs(lp.y - fp.y) < dragDistance)
        {
            // tapp;
        }
        
    }
    void SelectButton(Vector3 pos)
    {
        pos.z = Camera.main.nearClipPlane;
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction, int.MaxValue, LayerMask.GetMask("ButtonModel"));
        if (hits.Length <= 0) return;
        PieceModel piece = hits[0].collider.GetComponent<PieceModel>();
        if (piece == null) return;
        piece.FillColorForMesh();
    }
}
