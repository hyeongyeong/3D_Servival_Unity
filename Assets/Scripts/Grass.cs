using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    [SerializeField]
    private int hp;

    [SerializeField]
    private GameObject go_hit_effect;

    [SerializeField]
    private Item item_leaf;
    [SerializeField]
    private int leafCount;
    private Inventory theInven;


    private Rigidbody[] rigidbodys;
    private BoxCollider[] boxColliders;

    [SerializeField]
    private string hit_sound;

    [SerializeField]
    private float destroyTime;
    [SerializeField]
    private float force;


    // Start is called before the first frame update
    void Start()
    {
        theInven = FindObjectOfType<Inventory>();
        rigidbodys = this.transform.GetComponentsInChildren<Rigidbody>();
        boxColliders = transform.GetComponentsInChildren<BoxCollider>();
    }

    public void Damage()
    {
        hp--;
        Hit();
        if (hp <= 0)
        {
            Destruction();
        }


    }
    private void Destruction()
    {
        theInven.AcquireItem(item_leaf, leafCount);
        for (int i = 0; i < rigidbodys.Length; i++)
        {
            rigidbodys[i].useGravity = true;
            rigidbodys[i].AddExplosionForce(force, transform.position, 1f);
            boxColliders[i].enabled = true;
        }

        Destroy(this.gameObject, destroyTime);
    }
    private void Hit()
    {
        SoundManager.instance.PlaySE(hit_sound);
        var clone = Instantiate(go_hit_effect, transform.position + Vector3.up, Quaternion.identity);
        Destroy(clone, destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
