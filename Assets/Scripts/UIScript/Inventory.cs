using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false;

    // ?ʿ??? ??????Ʈ
    [SerializeField]
    private GameObject go_InventoryBase;
    [SerializeField]
    private GameObject go_QuickSlotParent;
    [SerializeField]
    private QuickSlotController theQuickslot;

    [SerializeField]
    private GameObject go_SlotsParent;
    private ItemEffectDatabase theItemEffectDatabase;
    private Slot[] slots;
    private Slot[] quickSlots;
    private bool isNotPut;

    private int slotNumber;

    // Start is called before the first frame update
    void Start()
    {
        theItemEffectDatabase = FindObjectOfType<ItemEffectDatabase>();
        slots = go_SlotsParent.GetComponentsInChildren<Slot>();
        quickSlots = go_QuickSlotParent.GetComponentsInChildren<Slot>();
    }

    // Update is called once per frame
    void Update()
    {
        TryOpenInventory();
    }

    private void TryOpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryActivated = !inventoryActivated;

            if (inventoryActivated)
                OpenInventory();
            else
                CloseInventory();

        }
    }
    private void OpenInventory()
    {
        go_InventoryBase.SetActive(true);
    }

    private void CloseInventory()
    {
        theItemEffectDatabase.HideToolTip();
        go_InventoryBase.SetActive(false);
    }

    public void AcquireItem(Item _item, int _count = 1)
    {
        PutSlot(quickSlots, _item, _count);
        if (!isNotPut)
        {
            theQuickslot.IsActivatedQuickSlot(slotNumber);
        }
        if (isNotPut)
            PutSlot(slots, _item, _count);
        if (isNotPut)
            Debug.Log("?????԰? ?κ??丮?? ??á???ϴ?");
    }

    private void PutSlot(Slot[] _slots, Item _item, int _count)
    {
        if (Item.ItemType.Equipment != _item.itemType) // ???? ???????? ?ƴ? ????
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i].item != null && _slots[i].item.itemName == _item.itemName)
                {
                    slotNumber = i;
                    _slots[i].SetSlotCount(_count);
                    isNotPut = false;
                    return;
                }
            }
        }

        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].item == null)
            {
                _slots[i].AddItem(_item, _count);
                isNotPut = false;
                return;
            }
        }
        isNotPut = true;

    }

    public int GetItemCount(string _itemName)
    {
        int temp = SearchSlotItem(slots, _itemName);
        return temp != 0 ? temp : SearchSlotItem(quickSlots, _itemName);
    }

    private int SearchSlotItem(Slot[] _slots, string _itemName)
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].item != null)
            {
                if (_itemName == _slots[i].item.itemName)
                    return _slots[i].itemCount;
            }
        }
        return 0;
    }

    public void SetItemCount(string _itemName, int _itemCount)
    {
        if (!ItemCountAdjust(slots, _itemName, _itemCount))
            ItemCountAdjust(quickSlots, _itemName, _itemCount);
    }
    
    private bool ItemCountAdjust(Slot[] _slots, string _itemName, int _itemCount)
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].item != null)
            {
                if (_itemName == _slots[i].item.itemName)
                    _slots[i].SetSlotCount(-_itemCount);
                return true;
            }
        }
        return false;
    }
}
