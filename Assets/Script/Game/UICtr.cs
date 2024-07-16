using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Runtime.CompilerServices;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;
using Nami.Controller;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using Unity.VisualScripting;

public class UICtr : MonoBehaviour
{
    [SerializeField] GameObject startPanel;
    [SerializeField] LoadingBar loadding;
    public Image blur;
    public Redo redo;
    public GameObject textLevelMainPopup;
    public Coin coinctr;
    public ChangeTextLevel changeTextLevel;
    public LoadingAdsReward loadingAds;
    public SettingButton setting;
    public RectTransform mask;
    public GameObject losePanel;
    public GameObject winPanel;
    public BuildUI buildingUI;
    public Transform nextbt;
    public ViewMap viewmap;
    float sizeMask = 3000;

    public static UICtr instance;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        StartGame();
    }
    public void StartGame()
    {
        sizeMask = Screen.height > Screen.width ? Screen.height : Screen.width;
        mask.sizeDelta = new Vector2(sizeMask, sizeMask);
        mask.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(sizeMask, sizeMask);
        startPanel.SetActive(true);
        mask.parent.gameObject.SetActive(false);
        SetDefault();
        loadding.Loadding(ClickButtonStart);
        loadingAds.Done();  
    }
    public void SetDefault()
    {
        //mask.gameObject.SetActive(false);
        ShowGamePlayState();
        blur.gameObject.SetActive(false);
        losePanel.SetActive(false); 
        winPanel.SetActive(false);
    }

    public void ClickButtonStart()
    {
        changeTextLevel.ChangeLevel(GameManager.instance.currentLevel);
        coinctr.Init();
        SetDefault();
        //PlayAudioClickButton();
        ShowLogFireBase.Instance.ShowStartLevel();
        Debug.Log("start");
        LoadModelState();
        //mask.parent.gameObject.SetActive(true);
        //mask.DOSizeDelta(Vector2.one, 0.75f).OnComplete(() =>
        //{
        //    GameCtr.instance.SetDefaultValue();
        //    startPanel.SetActive(false);
        //    mask.DOSizeDelta(new Vector2(sizeMask, sizeMask), 0.75f).OnComplete(() =>
        //    {
        //        mask.parent.gameObject.SetActive(false);
        //        GameCtr.instance.SetCanTouch(true);
        //    });
        //});
    }
    public void ClickButtonNextLevel()
    {
        PlayAudioClickButton();
        coinctr.CountCoins(() => 
        {
            ChangToModelState(); 
        });
    }
    public void ChangToModelState()
    {
        GameManager.instance.currentLevel++;
        GameManager.instance.playerData.levelCurrent = GameManager.instance.currentLevel;
        GameManager.instance.SaveData();
        LoadModelState();
    }
    public void LoadModelState()
    {

        mask.parent.gameObject.SetActive(true);
        mask.DOSizeDelta(Vector2.one, 0.75f).OnComplete(() =>
        {

            changeTextLevel.ChangeLevel(GameManager.instance.currentLevel);
            ResetUIForPlay();
            GameManager.instance.StateModel();
            ShowBuildState();
            ShowLogFireBase.Instance.ShowCompleteLevel();
            mask.DOSizeDelta(new Vector2(sizeMask, sizeMask), 0.75f).OnComplete(() => {
                mask.parent.gameObject.SetActive(false);
            });

        });
    }
    public void Next()
    {
        mask.parent.gameObject.SetActive(true);
        mask.DOSizeDelta(Vector2.one, 0.75f).OnComplete(() =>
        {
            ResetUIForPlay();
            GameManager.instance.StateGamePlay();
            ShowGamePlayState();
            GameCtr.instance.NextLevel();
            //changeTextLevel.ChangeLevel(GameManager.instance.currentLevel);
            ShowLogFireBase.Instance.ShowCompleteLevel();
            mask.DOSizeDelta(new Vector2(sizeMask, sizeMask), 0.75f).OnComplete(() => {
                mask.parent.gameObject.SetActive(false);
                GameCtr.instance.SetCanTouch(true);
            });

        });
    }
    public void Replay()
    {
        GameCtr.instance.objectCtr.firePS.Stop();
        PlayAudioClickButton();
        mask.parent.gameObject.SetActive(true);
        mask.DOSizeDelta(Vector2.one, 0.75f).OnComplete(() =>
        {
            ResetUIForPlay();
            GameCtr.instance.SetDefaultValue();
            ShowLogFireBase.Instance.AddNumberTriesLevel();

            mask.DOSizeDelta(new Vector2(sizeMask, sizeMask), 0.75f).OnComplete(() => {
                mask.parent.gameObject.SetActive(false);
                GameCtr.instance.SetCanTouch(true);
            });
        });
    }
    
    void ShowBuildState()
    {
        viewmap.ShowHideModel(false);
        buildingUI.gameObject.SetActive(true); 
        textLevelMainPopup.SetActive(false);
        redo.gameObject.SetActive(false);
        buildingUI.CheckHaveTask();
    }
    void ShowGamePlayState()
    {
        buildingUI.gameObject.SetActive(false);
        redo.gameObject.SetActive(true);
        textLevelMainPopup.SetActive(true);
    }
    public void ShowPopup(GameObject panel,UnityAction bgAction,UnityAction cplAction)
    {
        mask.parent.gameObject.SetActive(false);
        panel.SetActive(true) ;
        bgAction?.Invoke();
        panel.transform.GetChild(0).GetChild(0).localScale = Vector3.zero;
        panel.transform.GetChild(0).GetChild(0).DOScale(Vector3.one, 1.5f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            cplAction?.Invoke();
        });
        blur.gameObject.SetActive(true);
        blur.color = new Color(0, 0, 0, 0);
        blur.DOColor(new Color(0, 0, 0, (float)150 / 255), 0.1f);
    }

    public IEnumerator ShowNextButton()
    {
        yield return new WaitForSeconds(1f);
        nextbt.DOScale(1, 0.25f);
    }

    void ResetUIForPlay()
    {
        startPanel.SetActive(false);
        losePanel.SetActive(false);
        blur.gameObject.SetActive(false);
        winPanel.SetActive(false);

    }


    void PlayAudioClickButton(){
        GameManager.instance.audioManager.PlaySound(0);
    }

}
