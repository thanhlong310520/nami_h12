
using DG.Tweening;
using Nami.Controller;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    private void Awake()
    {
        instance = this;
        Input.multiTouchEnabled = false;
    }


    public PlayerData playerData;
    public int currentLevel = 1;
    public AudioManager audioManager;
    
    public ModelSO modelSO;
    public GameObject holderGamePlay;
    public Transform holderModel;
    public ModelCtr currentMode;
    public FitWorkgroundToCamera fitCamera;

    public List<ModelCtr> listModelCtrs = new List<ModelCtr>();    
    private void Start()
    {
        LoadData();
        Render_Model();
        SetCurrentMap();
        //banner
        ShowAdsBanner();
    }
    void LoadData()
    {
        playerData = (JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString("playerdata")) as PlayerData);
        if (playerData == null)
        {
            print("DAAAAAAAAAAAAAAAAAAAAAAA");
            playerData = new PlayerData();
        }
        if(playerData.listPiectModelActive == null)
        {
            playerData.listPiectModelActive = new List<int>();
        }
        if (playerData.listModelActived == null)
        {
            playerData.listModelActived = new List<int>();
            playerData.currentModelIndex = 0;

        }
        currentLevel = playerData.levelCurrent;
    }

    void Render_Model()
    {
        for(int i = 0; i < modelSO.listModel.Count; i++)
        {
            var tempModel = Instantiate(modelSO.listModel[i].modelPrefab,holderModel);
            tempModel.transform.position = Vector3.zero;
            var temctr = tempModel.GetComponent<ModelCtr>();
            temctr.name = modelSO.listModel[i].name;
            temctr.indexModel = i;
            temctr.LoadModel(fitCamera.modelCamera);
            listModelCtrs.Add(temctr);
        }
    }

    public void SetCurrentMap()
    {
        foreach (var model in listModelCtrs)
        {
            model.gameObject.SetActive(false);
        }
        if (playerData.currentModelIndex < 0) playerData.currentModelIndex = 0;
        if (playerData.currentModelIndex >= modelSO.listModel.Count) playerData.currentModelIndex = modelSO.listModel.Count - 1;

        currentMode = listModelCtrs[playerData.currentModelIndex];
        currentMode.gameObject.SetActive(true);
        UICtr.instance.buildingUI.CreatedTask(currentMode);
        UICtr.instance.buildingUI.OnOffTaskPanel(false);
        UICtr.instance.redo.SetTextNumberRedoing(); 
    }
    public void ShowMap(int index,StateShowMap state)
    {
        var pos = GetPosX(state);
        for(int i = 0; i < listModelCtrs.Count; i++)
        {
            if (i != index) listModelCtrs[i].ShowTween(pos, null);
        }
        listModelCtrs[index].transform.position = new Vector3(-pos, listModelCtrs[index].transform.position.y, listModelCtrs[index].transform.position.z);
        listModelCtrs[index].ShowTween(0, () => { OffModel(index); });
        listModelCtrs[index].gameObject.SetActive(true);
        UICtr.instance.viewmap.SetNameModel(listModelCtrs[index].name);
    }
    void OffModel(int index)
    {
        for (int i = 0; i < listModelCtrs.Count; i++)
        {
            if (i != index) listModelCtrs[i].gameObject.SetActive(false);
        }
    }

    float GetPosX(StateShowMap state)
    {
        if(state == StateShowMap.idle) return 0f;
        if (state == StateShowMap.next) return -30f;
        return 30f;
    }

    public void StateModel(){
        holderGamePlay.SetActive(false);
        holderModel.gameObject.SetActive(true);
    }
    public void StateGamePlay()
    {
        holderGamePlay.SetActive(true);
        //holderModel.gameObject.SetActive(false);

    }

    public void AddCoin(int add)
    {
        playerData.coinNumber += add;
        UICtr.instance.coinctr.textcoin.text = GameManager.instance.playerData.coinNumber.ToString();
    }


    // --------------------------------ADS-----------------------------------
    public void ShowAdsInter()
    {
        Debug.Log("ads");
        GameAds.Get.ShowInterstitialAd();
    }
    public void ShowAdsBanner()
    {
        Debug.Log("gamectr - show banner");
        try
        {
            GameAds.Get.ShowBanner();
        }
        catch (Exception e)
        {
            print("error ads banner");
        }


    }
    public void Vibrate()
    {
        if (!UICtr.instance.setting.onVibrator) return;
        Vibrator.Vibrate(75);
    }
    // ----------------------------------------------------------------------

    public void SaveData()
    {
        PlayerPrefs.SetString("playerdata", JsonUtility.ToJson(playerData));
    }
}

public enum StateShowMap
{
    idle, previous,next
}
[Serializable]
public class DataLevel
{
    public int colNumber;
    public int rowNumber;
    public List<DataBlock> blocks = new List<DataBlock>();
    public List<PosPieceInGridDone> listPigDone = new List<PosPieceInGridDone>();

    public void Add(Block block)
    {
        DataBlock temp = new DataBlock();
        temp.Add(block);
        blocks.Add(temp);
    }
}
[Serializable]
public class DataBlock
{
    public int row;
    public int col;
    public int sprite;
    public bool canChange;
    public List<Vector2> listPosSquare = new List<Vector2>();

    public void Add(Block block)
    {
        row = block.row; col = block.col;
        sprite = block.spriteIndex;
        canChange = block.canChange;
        foreach (var s in block.squares)
        {
            Vector2 posS = s.localPosition;
            listPosSquare.Add(new Vector2(Mathf.RoundToInt(posS.x), Mathf.RoundToInt(posS.y)));
        }
    }
}
[Serializable]
public class PosPieceInGridDone
{
    public int col;
    public int row;
}
