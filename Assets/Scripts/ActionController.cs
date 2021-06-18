using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    // 습득 가능한 최대 거리
    [SerializeField]
    private float range;

    // 습득 가능할 시 true
    private bool pickupActivated = false;

    private RaycastHit hitInfo; // 충돌체 정보 저장

    // 아이템 레이어에만 반응하도록 레이어 마스크 설정
    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private Text actionText;
    [SerializeField]
    private Inventory theInventory;

    // Update is called once per frame
    void Update()
    {
        CheckItem();
        TryAction();
    }

    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckItem();
            CanPickUp();
        }
    }

    private void CheckItem()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, range, layerMask))
        {
            if (hitInfo.transform.tag == "Item")
            {
                ItemInfoAppear();
            }
        }
        else
            InfoDissapear();
    }

    private void ItemInfoAppear()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + "획득" + "<color=yellow>" + "(E)" + "</color>";
    }

    private void InfoDissapear()
    {
        pickupActivated = false;
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
