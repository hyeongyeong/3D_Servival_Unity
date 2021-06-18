using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField]
    private int hp; // ���� ü��

    [SerializeField]
    private float destroyTime; // ���� ���� �ð�

    [SerializeField]
    private SphereCollider col;

    // �ʿ��� ���� ������Ʈ
    [SerializeField]
    private GameObject go_rock; // �Ϲ� ����.
    [SerializeField]
    private GameObject go_debrix; // ���� ����
    [SerializeField]
    private GameObject go_effect; // ä�� ����Ʈ

    [SerializeField]
    private string strike_Sound;
    [SerializeField]
    private string destroy_Sound;

    [SerializeField]
    private GameObject rock_item_prefab;
    [SerializeField]
    private int item_count;

    public void Mining()
    {
        SoundManager.instance.PlaySE(strike_Sound);
        var clone = Instantiate(go_effect, col.bounds.center, Quaternion.identity);
        Destroy(clone, 3f);

        hp--;
        if(hp <= 0)
        {
            Destruction();
        }
    }

    private void Destruction()
    {
        SoundManager.instance.PlaySE(destroy_Sound);
        col.enabled = false;
        for (int i = 0; i < item_count; i++)
        {
           Instantiate(rock_item_prefab, go_rock.transform.position, Quaternion.identity);
        }

        Destroy(go_rock);

        go_debrix.SetActive(true);
        Destroy(go_debrix, destroyTime);
    }

}
