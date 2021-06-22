using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotController : MonoBehaviour
{
    [SerializeField] private Slot[] quickSlots;
    [SerializeField] private Transform tf_parent;
    [SerializeField] private Transform tf_ItemPos;
    public static GameObject go_HandItem;

    private int selectedSlot;

    [SerializeField]
    private GameObject go_SelectedImage; // 선택된 퀵슬롯의 이미지
    [SerializeField]
    private WeaponManager theWeaponManager;

    // Start is called before the first frame update
    void Start()
    {
        quickSlots = tf_parent.GetComponentsInChildren<Slot>();
        selectedSlot = 0;
    }

    // Update is called once per frame
    void Update()
    {
        TryInputNumber();
    }

    private void TryInputNumber()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            ChangeSlot(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            ChangeSlot(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            ChangeSlot(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            ChangeSlot(3);
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            ChangeSlot(4);
        else if (Input.GetKeyDown(KeyCode.Alpha6))
            ChangeSlot(5);
        else if (Input.GetKeyDown(KeyCode.Alpha7))
            ChangeSlot(6);
        else if (Input.GetKeyDown(KeyCode.Alpha8))
            ChangeSlot(7);
    }

    public void IsActivatedQuickSlot(int _num)
    {
        if (selectedSlot == _num)
        {
            Execute();
        }
        else if(DragSlot.instance != null)
        {
            if(DragSlot.instance.dragSlot != null)
            {
                if(DragSlot.instance.dragSlot.GetQuickSlotNumber() == selectedSlot)
                {
                    Execute();
                    return;
                }
            }
        }

    }

    private void ChangeSlot(int _num)
    {
        SelectedSlot(_num);

        Execute();
    }
    private void Execute()
    {
        if(quickSlots[selectedSlot].item != null)
        {
            if (quickSlots[selectedSlot].item.itemType == Item.ItemType.Equipment)
                StartCoroutine(theWeaponManager.ChangeWeaponCoroutine(quickSlots[selectedSlot].item.weaponType, quickSlots[selectedSlot].item.itemName));
            else if (quickSlots[selectedSlot].item.itemType == Item.ItemType.Used)
                ChangeHand(quickSlots[selectedSlot].item);
            else
                ChangeHand();
        }
        else
        {
            ChangeHand();
        }
    }

    private void SelectedSlot(int _num)
    {
        selectedSlot = _num;
        go_SelectedImage.transform.position = quickSlots[selectedSlot].transform.position;
    }

    private void ChangeHand(Item _item = null)
    {
        StartCoroutine(theWeaponManager.ChangeWeaponCoroutine("HAND", "hand"));
        if(_item != null)
        {
            // 아이템을 손 끝에 생성
            StartCoroutine(HandItemCoroutine());
        }
    }

    IEnumerator HandItemCoroutine()
    {
        HandController.isActivate = false;
        yield return new WaitUntil(() => HandController.isActivate);

        go_HandItem = Instantiate(quickSlots[selectedSlot].item.itemPrefab, tf_ItemPos.position,tf_ItemPos.rotation);
        go_HandItem.GetComponent<Rigidbody>().isKinematic = true;
        go_HandItem.GetComponent<BoxCollider>().enabled = false;
        go_HandItem.tag = "Untagged";
        go_HandItem.layer = 9;
        go_HandItem.transform.SetParent(tf_ItemPos);
    }

    public void EatItem()
    {
        quickSlots[selectedSlot].SetSlotCount(-1);
        Debug.Log(quickSlots[selectedSlot].itemCount);
        if (quickSlots[selectedSlot].itemCount <= 0)
        {
            Destroy(go_HandItem);
        }
            
    }
}
