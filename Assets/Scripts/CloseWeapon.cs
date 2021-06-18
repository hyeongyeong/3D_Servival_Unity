using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWeapon : MonoBehaviour
{
    public string closeWeaponName; 

    // 무기 유형
    public bool isAxe;
    public bool isPickawe;
    public bool isHand;

    public float range; // 공격 범위
    public int damage; // 공격력
    public float workSpeed; // 작업속도
    public float attackDelay; // 공격 딜레이
    public float attackDelayA; // 공격 활성화 시점
    public float attackDelayB; // 공격 비활성화 시점

    public float workDelay; // 공격 딜레이
    public float workDelayA; // 공격 활성화 시점
    public float workDelayB; // 공격 비활성화 시점

    public Animator anim;
    
}
