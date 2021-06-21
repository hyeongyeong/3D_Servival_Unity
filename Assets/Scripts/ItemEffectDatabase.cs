using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemEffect
{
    public string itemName;
    [Tooltip("HP, SP, DP, HUNGRY, THIRSTY, SATISFY만 가능합니다")]
    public string[] part; // 효과
    public int[] num; // 수치
}

public class ItemEffectDatabase : MonoBehaviour
{
    [SerializeField]
    private SlotTooltip theSlotToolTip;
    [SerializeField]
    private ItemEffect[] itemEffects;
    [SerializeField]
    private WeaponManager theWeaponManager;
    [SerializeField]
    private StatusController thePlayerController;

    private const string HP = "HP", SP = "SP", DP = "DP", HUNGRY = "HUNGRY", THIRSTY = "THIRSTY", SATISFY = "SATISFY";

    public void UseItem(Item _item)
    {
        if (_item.itemType == Item.ItemType.Equipment)
        {
            // 장비 아이템 -> 장착
            StartCoroutine(theWeaponManager.ChangeWeaponCoroutine(_item.weaponType, _item.itemName));
        }

        else if (_item.itemType == Item.ItemType.Used)
        {
            for (int x = 0; x < itemEffects.Length; x++)
            {
                if(itemEffects[x].itemName == _item.itemName)
                {
                    for (int y = 0; y < itemEffects[x].part.Length; y++)
                    {
                        switch (itemEffects[x].part[y])
                        {
                            case HP:
                                thePlayerController.IncreaseHP(itemEffects[x].num[y]);
                                break;
                            case SP:
                                thePlayerController.IncreaseSP(itemEffects[x].num[y]);
                                break;
                            case DP:
                                thePlayerController.IncreaseDP(itemEffects[x].num[y]);
                                break;
                            case THIRSTY:
                                thePlayerController.IncreaseThirsty(itemEffects[x].num[y]);
                                break;
                            case HUNGRY:
                                thePlayerController.IncreaseHungry(itemEffects[x].num[y]);
                                break;
                            case SATISFY:
                                thePlayerController.IncreaseSatisfy(itemEffects[x].num[y]);
                                break;
                            default:
                                Debug.Log("잘못된 Status 부위");
                                break;
                        }
                        Debug.Log(_item.itemName + "을 사용하였습니다");
                    }
                    return;
                }
            }
            Debug.Log("itemEffect Item name과 일치하는 itemname이 없습니다.");
        }
    }

    public void ShowToolTip(Item _item, Vector3 _pos)
    {
        theSlotToolTip.ShowToolTip(_item, _pos);
    }
    public void HideToolTip()
    {
        theSlotToolTip.HideToolTip();
    }
}
