using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    public int currency;

    public SerializableDictionary<string, bool> skillTree;
    public SerializableDictionary<string, int> inventory;
    public List<string> equipmentId;

    public SerializableDictionary<string, bool> checkpoints;
    public string closestCheckpointId;
    public string currentSceneName;

    public float lostSoulX;
    public float lostSoulY;
    public int lostSoulAmount;

    public SerializableDictionary<string, float> volumeSettings;

    public GameData()
    {
        this.lostSoulX = 0;
        this.lostSoulY = 0;
        this.lostSoulAmount = 0;

        this.currency = 0;
        skillTree = new SerializableDictionary<string, bool>();
        inventory = new SerializableDictionary<string, int>();
        equipmentId = new List<string>();

        closestCheckpointId = string.Empty;
        checkpoints = new SerializableDictionary<string, bool>();
        this.currentSceneName = null;

        volumeSettings = new SerializableDictionary<string, float>();
    }
}
