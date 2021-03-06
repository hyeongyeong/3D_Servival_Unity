using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    [SerializeField] public string animalName;
    [SerializeField] protected int hp;
    [SerializeField] protected int attackRange;

    [SerializeField] protected float walkSpeed;
    [SerializeField] protected float runSpeed;

    protected Vector3 destination;

    protected bool isWalking;
    protected bool isAction;
    protected bool isRunning;
    public bool isDead = false;
    protected bool isChasing;
    protected bool isAttacking;

    [SerializeField] protected float walkTime;
    [SerializeField] protected float waitTime;
    [SerializeField] protected float runTime;
    protected float currentTime;

    
    // 필요한 컴포넌트
    [SerializeField] protected Item itemPrefab;
    [SerializeField] public int acquiredItem;

    [SerializeField] protected Animator anim;
    [SerializeField] protected Rigidbody rigid;
    [SerializeField] protected BoxCollider boxCol;
    protected FieldOfViewAngle theViewAngle;
    protected NavMeshAgent nav;

    private AudioSource theAudio;
    [SerializeField] protected AudioClip[] sound_normal;
    [SerializeField] protected AudioClip sound_Hurt;
    [SerializeField] protected AudioClip sound_Dead;
    protected StatusController statusController;

    void Start()
    {
        statusController = FindObjectOfType<StatusController>();
        theViewAngle = GetComponent<FieldOfViewAngle>();
        nav = GetComponent<NavMeshAgent>();
        theAudio = GetComponent<AudioSource>();
        currentTime = waitTime;
        isAction = true;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!isDead)
        {
            Move();
            ElapseTime();
        }
    }

    protected void Move()
    {
        if (isWalking || isRunning)
            //rigid.MovePosition(transform.position + (transform.forward * applySpeed * Time.deltaTime));
            nav.SetDestination(transform.position + destination * 5f);
    }

    protected void ElapseTime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0 && !isChasing && !isAttacking)
                // 다음 랜덤 행동
                initialize();
        }
    }
    protected virtual void initialize()
    {
        isWalking = false;
        isRunning = false;
        isAction = true;
        nav.speed = walkSpeed;
        nav.ResetPath();
        anim.SetBool("Walking", isWalking);
        anim.SetBool("Running", isRunning);
        destination.Set(Random.Range(-0.2f, 0.2f), 0f, Random.Range(0.5f, 1f));
        
    }


    protected void TryWalk()
    {
        isWalking = true;
        anim.SetBool("Walking", isWalking);
        currentTime = walkTime;
        nav.speed = walkSpeed;
    }

    

    public virtual void Damage(int _dmg, Vector3 _targetPos)
    {
        if (!isDead)
        {
            hp -= _dmg;

            if (hp <= 0)
            {
                Dead();
                return;
            }
            PlaySE(sound_Hurt);
            anim.SetTrigger("Hurt");
        }
    }
    protected virtual void Dead()
    {
        PlaySE(sound_Dead);
        isWalking = false;isRunning = false; isChasing = false; isDead = true;
        nav.ResetPath();
        anim.SetTrigger("Dead");
        transform.GetComponentsInChildren(typeof(NavMeshAgent), false);
        StopAllCoroutines();
    }

    protected void RandomSound()
    {
        int _random = Random.Range(0, 3); // 일상 사운드
        PlaySE(sound_normal[_random]);
    }
    protected void PlaySE(AudioClip _clip)
    {
        theAudio.clip = _clip;
        theAudio.Play();
    }

    public Item GetItem()
    {
        this.gameObject.tag = "Untagged";
        Destroy(this.gameObject, 3f);
        return itemPrefab;
    }

}
