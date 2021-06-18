using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeComponent : MonoBehaviour
{
    [SerializeField]
    private GameObject[] treePieces;

    [SerializeField]
    private GameObject treeCenter;

    [SerializeField]
    private CapsuleCollider parentCol;
    [SerializeField]
    private CapsuleCollider childCol;
    [SerializeField]
    private Rigidbody childRigid;
    [SerializeField]
    private float force;
    [SerializeField]
    private GameObject childTree;


    //  파편
    [SerializeField]
    private GameObject go_hit_effect;

    // 파편 제거시간
    [SerializeField]
    private float debrisDestroyTime;

    // 나무 제거 시간
    [SerializeField]
    private float destroyTime;


    [SerializeField]
    private string chop_sound;
    [SerializeField]
    private string falldown_sound;
    [SerializeField]
    private string logChange_sound;


    [SerializeField]
    private GameObject go_log;

    public void Chop(Vector3 _pos, float angleY)
    {
        Hit(_pos);

        AngleCalc(angleY);

        if (CheckTreePieces())
        {
            return;
        }

        FallDownTree();
    }

    private void Hit(Vector3 _pos)
    {
        SoundManager.instance.PlaySE(chop_sound);

        GameObject clone = Instantiate(go_hit_effect, _pos, Quaternion.Euler(Vector3.zero));
        Destroy(clone, debrisDestroyTime);
    }

    private void AngleCalc(float _angleY)
    {
        Debug.Log(_angleY);
        if (0 <= _angleY && _angleY <= 70)
            DestroyPiece(2);
        else if (70 <= _angleY && _angleY <= 140)
            DestroyPiece(3);
        else if (140 <= _angleY && _angleY <= 210)
            DestroyPiece(4);
        else if (210 <= _angleY && _angleY <= 280)
            DestroyPiece(0);
        else if (280 <= _angleY && _angleY <= 360)
            DestroyPiece(1);
    }

    private void DestroyPiece(int _num)
    {
        if(treePieces[_num].gameObject != null)
        {
            GameObject clone = Instantiate(go_hit_effect, treePieces[_num].transform.position, Quaternion.Euler(Vector3.zero));
            Destroy(clone, debrisDestroyTime);
            Destroy(treePieces[_num].gameObject);
        }
    }

    private bool CheckTreePieces()
    {
        for (int i = 0; i < treePieces.Length; i++)
        {
            if(treePieces[i].gameObject != null)
            {
                return true;
            }
        }
        return false;
    }
    
    private void FallDownTree()
    {
        SoundManager.instance.PlaySE(falldown_sound);
        Destroy(treeCenter);
        parentCol.enabled = false;
        childCol.enabled = true;
        childRigid.useGravity = true;

        childRigid.AddForce(Random.Range(-force, force), 0f, Random.Range(-force, force));

        StartCoroutine(LogCoroutine());
    }

    IEnumerator LogCoroutine()
    {
        yield return new WaitForSeconds(destroyTime);

        SoundManager.instance.PlaySE(logChange_sound);

        Instantiate(go_log, childTree.transform.position + (childTree.transform.up * 3f) ,Quaternion.LookRotation(childTree.transform.up));
        Instantiate(go_log, childTree.transform.position + (childTree.transform.up * 6f), Quaternion.LookRotation(childTree.transform.up));
        Instantiate(go_log, childTree.transform.position + (childTree.transform.up * 9f), Quaternion.LookRotation(childTree.transform.up));
        Destroy(childTree.gameObject);
    }

    public Vector3 GetTreeCenterPosistion()
    {
        return treeCenter.transform.position;
    }
}
