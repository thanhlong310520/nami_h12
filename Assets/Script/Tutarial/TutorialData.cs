using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "TutariaData", menuName = "TutoData")]
public class TutorialData : ScriptableObject
{
    public List<LevelTutorial> listlevel;
}

[Serializable]
public class LevelTutorial
{
    public TextAsset level;
    public List<DataTut> data;

}
[Serializable]
public class DataTut
{
    [EnumPaging]
    public StateTut State;
    public int blockindex;
    public GameObject target;

}
[Serializable]
public enum StateTut{
    none,rotate, flip, drop, clickbutton
}
