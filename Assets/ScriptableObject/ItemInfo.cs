using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used to define information for an item.
[CreateAssetMenu(fileName = "new power up", menuName = "Power Up")]
public class ItemInfo : ScriptableObject
{
    public ItemManager.itemType type;
    public new string name;
    public float duration;

    public Sprite icon;
}
