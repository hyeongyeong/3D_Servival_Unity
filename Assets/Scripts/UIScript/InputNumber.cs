using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputNumber : MonoBehaviour
{
    private bool activated = false;

    [SerializeField]
    private Text textPreview;
    [SerializeField]
    private Text text_Input;
    [SerializeField]
    private InputField if_Text;
    [SerializeField]
    private GameObject go_Base;
    // Start is called before the first frame update

    [SerializeField]
    private ActionController thePlayer;

    public void Call()
    {
        activated = true;
        go_Base.SetActive(true);
        if_Text.text = "";
        textPreview.text = DragSlot.instance.dragSlot.itemCount.ToString();
    }

    public void Cancel()
    {
        activated = false;
        go_Base.SetActive(false);
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;
    }

    public void OK()
    {
        int num;
        DragSlot.instance.SetColor(0);
        if (text_Input.text != "" )
        {
            if(CheckNumber(text_Input.text))
            {
                num = int.Parse(text_Input.text);
                if (num > DragSlot.instance.dragSlot.itemCount)
                    num = DragSlot.instance.dragSlot.itemCount;
                else
                {
                    num = int.Parse(text_Input.text);
                }
            }
            else
            {
                Cancel();
                return;
            }
        }
        else
        {
            num = int.Parse(textPreview.text);
        }

        StartCoroutine(DropItemCoroutine(num));
        
    }

    IEnumerator DropItemCoroutine(int _num)
    {
        for (int i = 0; i < _num; i++)
        {
            if(DragSlot.instance.dragSlot.item.itemPrefab != null)
                Instantiate(DragSlot.instance.dragSlot.item.itemPrefab, thePlayer.transform.position + thePlayer.transform.forward, Quaternion.identity);
            DragSlot.instance.dragSlot.SetSlotCount(-1);
            yield return new WaitForSeconds(0.05f);
        }
        if (int.Parse(textPreview.text) == _num)
            if (QuickSlotController.go_HandItem != null)
                Destroy(QuickSlotController.go_HandItem);

        DragSlot.instance.dragSlot = null;
        go_Base.SetActive(false);
        activated = false;
    }

    private bool CheckNumber(string _argString)
    {
        char[] _tempCharArray = _argString.ToCharArray();
        for (int i = 0; i < _tempCharArray.Length; i++)
        {
            if( 48 > _tempCharArray[i] || _tempCharArray[i] > 57)
            {
                return false;
            }
        }
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        if (activated) {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                OK();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cancel();
            }
        }
    }
}
