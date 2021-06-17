using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField]
    private int hp; // 바위 체력

    [SerializeField]
    private float destroyTime; // 파편 제거 시간

    [SerializeField]
    private SphereCollider col;

    // 필요한 게임 오브젝트
    [SerializeField]
    private GameObject go_rock; // 일반 바위.
    [SerializeField]
    private GameObject go_debrix; // 깨진 바위
    [SerializeField]
    private GameObject go_effect; // 채굴 이펙트

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip effect_sound;
    [SerializeField]
    private AudioClip effect_sound2;

    public void Mining()
    {
        audioSource.clip = effect_sound;
        audioSource.Play();
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
        audioSource.clip = effect_sound2;
        audioSource.Play();
        col.enabled = false;
        Destroy(go_rock);

        go_debrix.SetActive(true);
        Destroy(go_debrix, destroyTime);
    }

}
