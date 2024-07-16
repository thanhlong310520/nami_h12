using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ViewMap : MonoBehaviour
{
    public Button priveButton ;
    public Button nextButton ;

    public GameObject shownObject;
    public TMP_Text nameModel;
    public List<GameObject> objectOff = new List<GameObject>();
    public int indexModel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowHideModel(bool isShow){
        shownObject.gameObject.SetActive(isShow);
        objectOff.ForEach(of=> of.gameObject.SetActive(!isShow));
        if(isShow) {
            indexModel = GameManager.instance.playerData.currentModelIndex;
            CheckOnOffButtonMap();
        }
        else{
            StateShowMap s;
            var currentIdx = GameManager.instance.playerData.currentModelIndex;
            if (indexModel > currentIdx) s = StateShowMap.previous; 
            else if(indexModel < currentIdx) s = StateShowMap.next;
            else s = StateShowMap.idle;
            GameManager.instance.ShowMap(currentIdx,s);
        }
    }

    void CheckOnOffButtonMap(){
        if(indexModel <=0) priveButton.gameObject.SetActive(false);
        else priveButton.gameObject.SetActive(true);
        if(indexModel >= (GameManager.instance.listModelCtrs.Count-1)) nextButton.gameObject.SetActive(false);
        else nextButton.gameObject.SetActive(true);
    }
    public void SetNameModel(string name)
    {
        nameModel.text = name;
    }

    public void NextModel(){
        indexModel++;
        if(indexModel > GameManager.instance.listModelCtrs.Count - 1) indexModel = GameManager.instance.listModelCtrs.Count-1;
        GameManager.instance.ShowMap(indexModel,StateShowMap.next);
        CheckOnOffButtonMap();
    }
    public void PreviousModel(){
        indexModel--;
        if(indexModel < 0) indexModel =0;
        GameManager.instance.ShowMap(indexModel,StateShowMap.previous);
        CheckOnOffButtonMap();
    }

}
