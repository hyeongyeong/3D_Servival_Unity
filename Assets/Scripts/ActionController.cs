using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    // ���� ������ �ִ� �Ÿ�
    [SerializeField]
    private float range;
    [SerializeField]
    private string sound_meat;

    // ���� ������ �� true
    private bool pickupActivated = false;
    private bool dissoveActivated = false;
    private bool isDissolving = false; // ��� ��ü ��?


    private bool fireLookActivated = false; // ���� �����ؼ� �ٶ� �� true

    private RaycastHit hitInfo; // �浹ü ���� ����

    // ������ ���̾�� �����ϵ��� ���̾� ����ũ ����
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
                // ��� ��ü �ǽ�
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
            actionText.text = "���õ� ������ �ҿ� �ֱ�" + "<color=yellow>" + "(E)" + "</color>";
        }
    }

    private void CanDropFire()
    {
        if (fireLookActivated)
        {
            if(hitInfo.transform.tag == "Fire" && hitInfo.transform.GetComponent<Fire>().GetIsFire())
            {
                // �տ� ��� �ִ� �������� �ҿ� �ֱ�
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
                if (_selectedSlot.item.itemName.Contains("���"))
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
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + "ȹ��" + "<color=yellow>" + "(E)" + "</color>";
    }

    private void MeatInfoAppear()
    {
        initialize();
        if (hitInfo.transform.GetComponent<Animal>().isDead)
        {
            dissoveActivated = true;
            actionText.gameObject.SetActive(true);
            actionText.text = hitInfo.transform.GetComponent<Animal>().animalName + "��ü�ϱ�" + "<color=yellow>" + "(E)" + "</color>";
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
                Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + "ȹ���߽��ϴ�.");
                Destroy(hitInfo.transform.gameObject);
                InfoDissapear();
            }
        }
    }
}
