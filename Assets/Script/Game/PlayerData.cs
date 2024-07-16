using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public int levelCurrent = 1;
    public int coinNumber = 0;
    public int currentModelIndex = 0;
    public int numberRedo = 3;
    public List<int> listModelActived;
    public List<int> listPiectModelActive;
    public PlayerData()
    {
        levelCurrent = 1;
        coinNumber = 0;
        currentModelIndex = 0;
        numberRedo = 3;
        listPiectModelActive = new List<int>();
        listModelActived = new List<int>();


}
}
