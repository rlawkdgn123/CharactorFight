using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(AudioSource))]
public class Attack : MonoBehaviour
{
    [SerializeField] public int attackDmg = 10;
    [SerializeField] public int chargeMana = 5;
    [SerializeField] public Vector2 knockback = Vector2.zero;
    [SerializeField] private PlayerBase player;
    [SerializeField] private PlayerMana pMana;
    [SerializeField] private AudioSource audio;
    [SerializeField] public bool mpCharge;   // 공격 적중 시 마나 충전 여부

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        Damageable damageable = collision.GetComponent<Damageable>();
        
        if(damageable != null)
        {
           
            bool gotHit = damageable.Hit(attackDmg, knockback);

            if(gotHit)
            {
                Debug.Log(collision.name + " hit for " + attackDmg);
                if(mpCharge)
                    pMana.GetComponent<PlayerMana>().MpCharge(chargeMana);
            }
        }
    }
    private void OnEnable()
    {
        audio.Play();
    }
    private void PlayerInitialize()
    {
        player = gameObject.GetComponentInParent<PlayerBase>();
        pMana = player.GetComponent<PlayerMana>();
        audio = gameObject.GetComponent<AudioSource>();
    }
}   
