using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
[System.Serializable]

public class saveData 
{
    public Vector3 playerPosition;
    public string mapBoundry;
    public List<InventorySaveData> inventorySaveData;
    public float musicVolume;
    public bool isMusicOn;
}
