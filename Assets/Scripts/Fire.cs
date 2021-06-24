using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] private string fireName;
    [SerializeField] private int damage;
    [SerializeField] private float damageTime;
    private float currentDamageTime;

    [SerializeField] private float durationTime;
    private float currentDurationTime;

    [SerializeField]
    private ParticleSystem ps_Flame;

    private StatusController thePlayerStatus;

    private bool isFire = true;

    private void Start()
    {
        thePlayerStatus = FindObjectOfType<StatusController>();
        currentDurationTime = durationTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFire)
        {
            ElapseTime();
        }
    }

    private void ElapseTime()
    {
        currentDurationTime -= Time.deltaTime;
        
        if(currentDurationTime <= 0)
        {
            // ºÒ²ô±â
            Fire_Off();
        }
        if (currentDamageTime > 0)
            currentDamageTime -= Time.deltaTime;
    }

    private void Fire_Off()
    {
        ps_Flame.Stop();
        isFire = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (isFire && other.transform.tag == "Player")
        {
            if (currentDamageTime <= 0)
            {
                thePlayerStatus.DecreaseHP(damage);
                currentDamageTime = damageTime;
            }
        }
    }
}
