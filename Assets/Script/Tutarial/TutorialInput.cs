using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class TutorialInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
{
    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered
    public float persentScreen = 5;
    

    public bool canclick = false;
    public bool canHandle = false;
    void Start()
    {
        dragDistance = Screen.height * persentScreen / 100; //dragDistance is 15% height of the screen
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!canclick) return;
        fp = eventData.position;
        lp = eventData.position;
        SelectBlock(eventData.position);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        lp = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        lp = eventData.position;  //last touch position. Ommitted if you use list

        CheckState();
    }

    void CheckState()
    {
        if (!canHandle) return;
        canHandle = false;
        StateTut st = new StateTut();
        if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
        {
            if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
            {
                //Flip
                st = StateTut.flip;
            }
            else
            {
                if (lp.y < fp.y)  //If the movement was up
                {
                    // move 
                    st = StateTut.drop;

                }
            }
        }
        else
        {
            st = StateTut.rotate;
        }
        print("state tutorial " + st);
        StartCoroutine(TutorialCtr.instance.Handle(st));
    }
    void SelectBlock(Vector3 pos)
    {
        
        canHandle = false;

        if (TutorialCtr.instance.data.listlevel.Count <= TutorialCtr.instance.indexlevel) 
        {
            TutorialCtr.instance.EndTutorial();
            return;
        }
        if (TutorialCtr.instance.data.listlevel[TutorialCtr.instance.indexlevel].data.Count <= TutorialCtr.instance.step)
        {
            TutorialCtr.instance.EndTutorial();
            return;
        }
        if (TutorialCtr.instance.data.listlevel[TutorialCtr.instance.indexlevel].data[TutorialCtr.instance.step].State == StateTut.clickbutton)
        {
            var newPos = Camera.main.WorldToScreenPoint(pos);
            pos.z = Camera.main.nearClipPlane;
            Ray ray = Camera.main.ScreenPointToRay(newPos);
            RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction, int.MaxValue);
            print("click " + hits.Length);
            foreach (var h in hits)
            {
                print(h.collider.name);
                if(h.transform.GetComponent<Redo>() != null)
                {
                    canHandle = true;
                }
            }
        }
        else
        {
            if (TutorialCtr.instance.data.listlevel[TutorialCtr.instance.indexlevel].data[TutorialCtr.instance.step].blockindex >= GameCtr.instance.listBlock.Count) canHandle = false;
            else
            {
                pos.z = Camera.main.nearClipPlane;
                Ray ray = Camera.main.ScreenPointToRay(pos);
                RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction, int.MaxValue);
                foreach (var h in hits)
                {
                    if (h.transform.parent.gameObject == GameCtr.instance.listBlock[TutorialCtr.instance.data.listlevel[TutorialCtr.instance.indexlevel].data[TutorialCtr.instance.step].blockindex].gameObject)
                    {
                        canHandle = true;
                    }
                }

            }
        }
    }
}
