using UnityEngine;

[System.Serializable]
public struct BonusInfo
{
    public Vector3 position;
    public string type;

    public BonusInfo(string type, Vector3 position)
    {
        this.type = type;
        this.position = position;
    }
}