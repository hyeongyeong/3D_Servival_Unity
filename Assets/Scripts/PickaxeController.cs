using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeController : CloseWeaponController
{
    public static bool isActivate = false;

    public override void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        base.CloseWeaponChange(_closeWeapon);
        isActivate = true;
    }

    // Mining
    protected override IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                if (hitInfo.transform.tag == "Rock")
                {
                    hitInfo.transform.GetComponent<Rock>().Mining();
                }
                else if (hitInfo.transform.tag == "GentleAnimal")
                {
                    hitInfo.transform.GetComponent<GentleAnimal>().Damage(currentCloseWeapon.damage, transform.position);
                    SoundManager.instance.PlaySE("Animal_Hit");
                }
                else if (hitInfo.transform.tag == "PreemptiveAnimal")
                {
                    
                }


                isSwing = false;
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }
    void Update()
    {
        if (isActivate)
            TryAttack();
    }
}
