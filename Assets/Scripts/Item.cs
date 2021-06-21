using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject
{
    public string itemName;
    [TextArea]
    public string itemDesc;

    public ItemType itemType; // item 유형
    public Sprite itemImage;
    public GameObject itemPrefab;

    public string weaponType; // 무기 유형
    
    public enum ItemType
    {
        Equipment,
        Used,
        Ingredient,
        ETC
    }

}
