using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ModelSO",menuName = "ModelSO")]
public class ModelSO : ScriptableObject
{
    public List<DataModel> listModel = new List<DataModel>();
}

[Serializable]
public class DataModel{
    public string name;
    public GameObject modelPrefab;
}
