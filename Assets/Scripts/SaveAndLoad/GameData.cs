using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public float lostCurrencyX;
    public float lostCurrencyY;
    public int lostCurrencyAmount;

    public SerializableDictionary<string, int> inventory;
    public SerializableDictionary<string, bool> skillTree;
    public List<string> equipmentID;
    public int currency;

    public SerializableDictionary<string, bool> checkpoints;
    public string closestCheckpointID;

    public SerializableDictionary<string, float> volumeSettings;

    public GameData()
    {
        this.lostCurrencyX = 0;
        this.lostCurrencyY = 0;
        this.lostCurrencyAmount = 0;

        inventory = new SerializableDictionary<string, int>();
        skillTree = new SerializableDictionary<string, bool>();
        equipmentID = new List<string>();
        this.currency = 0;

        checkpoints = new SerializableDictionary<string, bool>();
        closestCheckpointID = string.Empty;

        volumeSettings = new SerializableDictionary<string, float>();
    }
}
