using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor;

public class ChangeTextLevel : MonoBehaviour
{
    public TMP_Text mainLv;
    public TMP_Text loseLevel;
    public TMP_Text winLevel;
    public TMP_Text buttonnextBuild;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeLevel(int lv){
        mainLv.text = $"Level {lv}";
        loseLevel.text = $"LEVEL {lv}";
        winLevel.text = $"LEVEL {lv}";
        buttonnextBuild.text = $"Level {lv}";
    }
}
