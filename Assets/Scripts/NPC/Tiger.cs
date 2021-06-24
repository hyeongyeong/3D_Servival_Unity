using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiger : PreemptiveAnimal
{
    [SerializeField] protected AudioClip Attack;
    protected override void initialize()
    {
        base.initialize();
        RandomAction();
    }
    private void RandomAction()
    {
        RandomSound();
        isAction = true;
        int _random = Random.Range(0, 3);

        if (_random == 0)
            Wait();
        else if (_random == 1)
            Roaring();
        else if (_random == 2)
            TryWalk();

    }


    private void Wait()
    {
        currentTime = waitTime;
    }
    private void Roaring()
    {
        currentTime = waitTime;
        anim.SetTrigger("Roaring");
    }
    
    protected override void Update()
    {
        base.Update();
        
        if(theViewAngle.View() && !isDead && !isAttacking)
        {
            StopAllCoroutines();
            StartCoroutine(ChaseTargetCoroutine());
        }
    }

}
