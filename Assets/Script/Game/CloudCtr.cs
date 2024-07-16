using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCtr : MonoBehaviour
{
    public List<GameObject> clouds = new List<GameObject>();
    public Transform mask;
    public float minTimeSqawn = 3;
    public float maxTimeSqawn = 7;
    float timeSqawn;
    float currentTime = 0;

    public static CloudCtr instance;
    private void Awake()
    {
        instance = this;        
    }
    void Start()
    {
        timeSqawn = Random.RandomRange(minTimeSqawn, maxTimeSqawn);
        currentTime = 0;

        Sqawn();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if( currentTime > timeSqawn ) 
        {
            Sqawn();
        }
    }
    void Sqawn()
    {
        currentTime = 0;
        timeSqawn = Random.Range(minTimeSqawn, maxTimeSqawn);
        int index = (int)Random.Range(0, (clouds.Count));
        var tempCloud = Instantiate(clouds[index]);
        tempCloud.transform.SetParent(mask);
        tempCloud.transform.localScale = Vector3.one;
        var y = -(RandomY());
        tempCloud.GetComponent<RectTransform>().anchoredPosition = new Vector3((Screen.width / 2 + 100), y, 0);
        var t = tempCloud.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-(Screen.width / 2 + 100), y), 50).OnComplete(() => {
            Destroy(tempCloud); 
        });
    }
    float RandomY()
    {
        return Random.Range(Screen.height / 2 - 400, Screen.height/2);
    }
}
