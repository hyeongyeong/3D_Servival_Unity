using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreemptiveAnimal : Animal
{
    [SerializeField]
    protected float chaseTime; // 총 추격 시간
    protected float currentChaseTime; // 현재 추격 시간
    [SerializeField]
    protected float chaseDelayTime; 

    public void Chase(Vector3 _targetPos)
    {
        isChasing = true; isRunning = true;
        destination = _targetPos;
        nav.speed = runSpeed;
        anim.SetBool("Running", isRunning);
        nav.SetDestination(destination);
    }

    public override void Damage(int _dmg, Vector3 _targetPos)
    {
        base.Damage(_dmg, _targetPos);
        if (!isDead)
            Chase(_targetPos);
    }
}
