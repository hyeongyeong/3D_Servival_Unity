using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiger : PreemptiveAnimal
{
    protected override void Update()
    {
        base.Update();
        
        if(theViewAngle.View() && !isDead)
        {
            StopAllCoroutines();
            StartCoroutine(ChaseTargetCoroutine());
        }
    }

    IEnumerator ChaseTargetCoroutine()
    {
        currentChaseTime = 0;

        while (currentChaseTime < chaseTime)
        {
            Chase(theViewAngle.GetTargetPos());
            yield return new WaitForSeconds(chaseDelayTime);
            currentChaseTime += chaseDelayTime;
        }
        isRunning = false; isChasing = false;
        anim.SetBool("Running", isRunning);
        nav.ResetPath();
    }

    // Update is called once per frame

}
