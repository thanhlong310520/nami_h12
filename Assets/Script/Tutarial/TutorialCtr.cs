using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialCtr : MonoBehaviour
{
    static string rotate = "Tap to rotate the block!";
    static string drop = "Swipe down to drop the block!";
    static string flip = "Swipe to flip block!";
    static string clickbutton = "";

    public TutorialData data;
    public TutorialInput tInput;
    [SerializeField] Transform hand;
    [SerializeField] SkeletonGraphic skeletonAnimation;
    [SerializeField] Redo redo;
    [SerializeField] TMP_Text text;

    public GameObject target;
    public static TutorialCtr instance;
    string levelTut = "";
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        levelTut = PlayerPrefs.GetString("tutoDone");
        EndTutorial();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int step = 0;
    public int indexlevel = 0;
    public bool SratTutorial(TextAsset lv)
    {
        GameCtr.instance.isTutarial = false;
        indexlevel = GetLevelTutarialData(lv);
        if (indexlevel < 0) 
        {
            EndTutorial();
            return false;
        } 
        GameCtr.instance.isTutarial = true;
        step = 0;
        tInput.gameObject.SetActive(true);
        hand.gameObject.SetActive(true);
        text.gameObject.SetActive(true) ;
        Debug.Log("start tutorial");
        MoveHand();

        return true;
    }
    public void EndTutorial()
    {
        tInput.gameObject.SetActive(false);
        hand.gameObject.SetActive(false);
        GameCtr.instance.isTutarial = false;
        text.gameObject.SetActive(false);
        PlayerPrefs.SetString("tutoDone", levelTut);

    }
    int GetLevelTutarialData(TextAsset lvget)
    {
        int index = -1;

        if (lvget.name == levelTut) return index;
        for(int i =0; i< data.listlevel.Count; i++)
        {
            if(data.listlevel[i].level == lvget)
            {
                index = i;
                levelTut = lvget.name;
                break;
            }
        }
        return index;
    }
    public void MoveHand()
    {
        if (step >= data.listlevel[indexlevel].data.Count) return;
        if (data.listlevel[indexlevel].data[step].State != StateTut.clickbutton)
        {
            Vector2 screenPos = Camera.main.WorldToScreenPoint(GameCtr.instance.listBlock[data.listlevel[indexlevel].data[step].blockindex].transform.position);
            hand.DOMove(screenPos, 0.1f).OnComplete(() =>
            {
                var tut = data.listlevel[indexlevel].data[step].State;
                switch (tut)
                {
                    case StateTut.rotate:
                        {
                            skeletonAnimation.AnimationState.SetAnimation(0,"Click",true);
                            ChangeText(rotate);
                            break;
                        }
                    case StateTut.flip:
                        {
                            skeletonAnimation.AnimationState.SetAnimation(0,"Rightandleft",true);
                            ChangeText(flip);

                            break;
                        }
                    case StateTut.drop:
                        {
                            skeletonAnimation.AnimationState.SetAnimation(0,"Rightandleft2",true);
                            ChangeText(drop);
                            break;
                        }
                }
            });
        }
        else
        {
            hand.DOMove(data.listlevel[indexlevel].data[step].target.transform.position, 0.1f).OnComplete(() =>
            {
                skeletonAnimation.AnimationState.SetAnimation(0,"Click",true);
                ChangeText(clickbutton);
            });
        }
        tInput.canclick = true;
        tInput.canHandle = false;
    }

    public IEnumerator Handle(StateTut tut)
    {
        if(data.listlevel[indexlevel].data[step].State == StateTut.clickbutton)
        {
            if(tut == StateTut.rotate)
            {
                ClickButtonRedo();
                step++;
                tInput.canclick = false;
                yield return new WaitForSeconds(0.25f);
                CheckEndTurorial(); 
                MoveHand();
                skeletonAnimation.AnimationState.SetAnimation(0,"Idle",true);
            }
        }
        else if(data.listlevel[indexlevel].data[step].State == tut)
        {
            skeletonAnimation.AnimationState.SetAnimation(0,"Idle",true);
            HandleBlock(tut);
            step++;
            tInput.canclick = false;
            yield return new WaitForSeconds(0.25f);
            CheckEndTurorial();
            MoveHand();
        }
        



    }

    void CheckEndTurorial()
    {
        if(step >= data.listlevel[indexlevel].data.Count)
        {
            print("end tutorial");
            EndTutorial();
        }
    }

    void ClickButtonRedo()
    {
        //redo.Redoing();
        data.listlevel[indexlevel].data[step].target.GetComponent<CallBackClickButton>().Handle();

    }
    void ChangeText(string t)
    {
        text.transform.localScale = Vector3.zero;
        text.text = t;
        text.transform.DOScale(1, 0.25f).SetEase(Ease.OutQuad);
    }

    //void SetTarget()
    //{
    //    if(data.listlevel[indexlevel].data[step].State == StateTut.clickbutton)
    //    {
    //        target = redo.gameObject;
    //    }
    //    else
    //    {
    //        target = GameCtr.instance.listBlock[data.listlevel[indexlevel].data[step].blockindex].gameObject;
    //    }
    //}
    
    void HandleBlock(StateTut tut)
    {
        Block b = GameCtr.instance.listBlock[data.listlevel[indexlevel].data[step].blockindex];
        GameCtr.instance.handleBlock.currentBlock = b;
        switch (tut)
        {
            case StateTut.rotate:
                {
                    GameCtr.instance.handleBlock.RotateBlock();
                    break;
                }
            case StateTut.flip:
                {
                    GameCtr.instance.handleBlock.FlipBlock();
                    break;
                }
            case StateTut.drop:
                {
                    GameCtr.instance.handleBlock.MovedownBlock();
                    break;
                }
        }
    }

}
