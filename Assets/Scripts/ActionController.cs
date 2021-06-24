using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    // 습득 가능한 최대 거리
    [SerializeField]
    private float range;
    [SerializeField]
    private string sound_meat;

    // 습득 가능할 시 true
    private bool pickupActivated = false;
    private bool dissoveActivated = false;
    private bool isDissolving = false; // 고기 해체 중?


    private bool fireLookActivated = false; // 불을 근접해서 바라볼 시 true

    private RaycastHit hitInfo; // 충돌체 정보 저장

    // 아이템 레이어에만 반응하도록 레이어 마스크 설정
    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private Text actionText;
    [SerializeField]
    private Inventory theInventory;
    [SerializeField]
    private WeaponManager theWeaponManager;
    [SerializeField]
    private QuickSlotController theQuickSlot;
    [SerializeField]
    private Transform tf_MeatDissolveTool;

    // Update is called once per frame
    void Update()
    {
        CheckAction();
        TryAction();
    }

    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckAction();
            CanPickUp();
            CanDissolve();
            CanDropFire();
        }
    }

    private void CanDissolve()
    {
        if (dissoveActivated)
        {
            if(hitInfo.transform.tag == "GentleAnimal" || hitInfo.transform.tag == "PreemptiveAnimal" && hitInfo.transform.GetComponent<Animal>().isDead && !isDissolving)
            {
                isDissolving = true;
                InfoDissapear();
                // 고기 해체 실시
                StartCoroutine(DissolveCoroutine());
            }
               
        }
    }

    IEnumerator DissolveCoroutine()
    {
        WeaponManager.isChangeWeapon = true;
        WeaponManager.currentWeaponAnim.SetTrigger("Weapon_Out");
        PlayerController.isActivated = false ;
        WeaponSway.isActivated = false;
        yield return new WaitForSeconds(0.2f);

        WeaponManager.currentWeapon.gameObject.SetActive(false);
        tf_MeatDissolveTool.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.2f);
        SoundManager.instance.PlaySE(sound_meat);
        yield return new WaitForSeconds(1.8f);

        theInventory.AcquireItem(hitInfo.transform.GetComponent<Animal>().GetItem(), hitInfo.transform.GetComponent<Animal>().acquiredItem);

        WeaponManager.currentWeapon.gameObject.SetActive(true);
        tf_MeatDissolveTool.gameObject.SetActive(false);

        WeaponManager.isChangeWeapon = false;
        isDissolving = false;
        PlayerController.isActivated = true;
        WeaponSway.isActivated = true;
    }

    private void CheckAction()
    {
        
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, range, layerMask))
        {
            if (hitInfo.transform.tag == "Item")
            {
                ItemInfoAppear();
            }
            else if (hitInfo.transform.tag == "GentleAnimal" || hitInfo.transform.tag == "PreemptiveAnimal")
            {
                MeatInfoAppear();
            }
            else if (hitInfo.transform.tag == "Fire")
                FireInfoAppear();
            else
            {
                InfoDissapear();
            }
        }
        else
            InfoDissapear();
    }

    private void initialize()
    {
        pickupActivated = false;
        dissoveActivated = false;
        fireLookActivated = false;
    }

    private void FireInfoAppear()
    {
        initialize();
        fireLookActivated = true;
        
        if (hitInfo.transform.GetComponent<Fire>().GetIsFire())
        {
            actionText.gameObject.SetActive(true);
            actionText.text = "선택된 아이템 불에 넣기" + "<color=yellow>" + "(E)" + "</color>";
        }
    }

    private void CanDropFire()
    {
        if (fireLookActivated)
        {
            if(hitInfo.transform.tag == "Fire" && hitInfo.transform.GetComponent<Fire>().GetIsFire())
            {
                // 손에 들고 있는 아이템을 불에 넣기
                Slot _selectedSlot = theQuickSlot.GetSelectedSlot();
                if(_selectedSlot.item != null)
                {
                    DropAnItem(_selectedSlot);
                }

            }
        }
    }
    private void DropAnItem(Slot _selectedSlot)
    {
        switch (_selectedSlot.item.itemType)
        {
            case Item.ItemType.Used:
                if (_selectedSlot.item.itemName.Contains("고기"))
                {
                    Instantiate(_selectedSlot.item.itemPrefab, hitInfo.transform.position + Vector3.up, Quaternion.identity);
                    _selectedSlot.SetSlotCount(-1);
                    theQuickSlot.DecreaseSelectedItem();
                }
                break;
            case Item.ItemType.Ingredient:
                break;
        }
    }

    private void ItemInfoAppear()
    {
        initialize();
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + "획득" + "<color=yellow>" + "(E)" + "</color>";
    }

    private void MeatInfoAppear()
    {
        initialize();
        if (hitInfo.transform.GetComponent<Animal>().isDead)
        {
            dissoveActivated = true;
            actionText.gameObject.SetActive(true);
            actionText.text = hitInfo.transform.GetComponent<Animal>().animalName + "해체하기" + "<color=yellow>" + "(E)" + "</color>";
        }
    }

    private void InfoDissapear()
    {
        pickupActivated = false;
        dissoveActivated = false;
        fireLookActivated = false;
        actionText.gameObject.SetActive(false);
    }

    private void CanPickUp()
    {
        if (pickupActivated)
        {
            if(hitInfo.transform != null)
            {
                theInventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickUp>().item);
                Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + "획득했습니다.");
                Destroy(hitInfo.transform.gameObject);
                InfoDissapear();
            }
        }
    }
}
