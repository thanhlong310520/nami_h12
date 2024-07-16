using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class ChangeAsset : MonoBehaviour
{
    [SerializeField] Image replayPopup;
    [SerializeField] Image x;
    [SerializeField] Image ok;
    [SerializeField] Image losePopup;
    [SerializeField] Image replayLose;
    [SerializeField] Image homeLose;
    [SerializeField] SpriteRenderer bg;
    [ShowInInspector]public List<Sprite> sp0 = new List<Sprite>();
    [ShowInInspector]public List<Sprite> sp1 = new List<Sprite>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Change()
    {
        Debug.Log("ChangeAsset");
        int index = Random.Range(0, 2);
        if(index == 0)
        {
            Changing(sp0);
        }
        else
        {
            Changing(sp1);
        }

    }
    void Changing(List<Sprite> s)
    {
        bg.sprite = s[0];
        replayPopup.sprite = s[1];
        losePopup.sprite = s[2];
        x.sprite = s[3];
        ok.sprite = s[4];
        replayLose.sprite = s[5];
        homeLose.sprite = s[6];
    } 
}
