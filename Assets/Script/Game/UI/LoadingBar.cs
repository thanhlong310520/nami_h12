using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{
    public float timeLoading = 3f;
    public Image loadingFill;

    public AsyncOperation asyncLoad;
    [Button]
    public void Loadding(UnityAction callback)
    {
        loadingFill.fillAmount = 0;
        loadingFill.DOFillAmount(1, timeLoading).SetEase(Ease.OutQuint).OnComplete(() =>
        {
            print("addd");
            callback?.Invoke();
        });
    }
    public void LoadSceneAsync(string sceneName,UnityAction callback)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName,callback));
    }

    IEnumerator LoadSceneCoroutine(string sceneName,UnityAction callback)
    {
        asyncLoad = SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = false;

        // Chờ đợi cho việc tải scene hoàn tất
        while (!asyncLoad.isDone)
        {
            print("fill " + asyncLoad.progress);
            yield return null;
        }
        print("done loading");
        callback?.Invoke(); 
    }
}
