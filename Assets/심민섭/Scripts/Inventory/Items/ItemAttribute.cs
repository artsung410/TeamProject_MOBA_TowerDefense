using UnityEngine;
using System.Collections;

[System.Serializable]
public class ItemAttribute
{

    public string attributeName;
    public float attributeValue; // int -> float 타입 바꿈 (hsw 11-01)
    public ItemAttribute(string attributeName, int attributeValue)
    {
        this.attributeName = attributeName;
        this.attributeValue = attributeValue;
    }

    public ItemAttribute() { }

}

