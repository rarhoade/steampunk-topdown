using System.Collections;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    public int maxHealth;
    [SerializeField] private int currentHealth;
    private SpriteRenderer[] sprite;
    private List<Material> material;
    public float spriteEffectCount = 1f;
    private Rigidbody2D rb;
    public Action<Enemy> DamageEffectDelegate;
    //public DamageEffectDelegate DamageEffect;


    private bool CanTakeDamage;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth; 
        sprite = GetComponentsInChildren<SpriteRenderer>();
        material = new List<Material>();
        foreach(SpriteRenderer sp in sprite){
            material.Add(sp.material);
        }
        CanTakeDamage = true;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    public void TakeDamage(int damage, Vector2 knockback){
        if(CanTakeDamage){
            currentHealth -= damage;
            //play hurt animation or die
            StartCoroutine(HitStunFreeze());
            
            if(currentHealth > 0){
                rb.AddForce(knockback * 5f, ForceMode2D.Impulse);
                StartCoroutine(DamageBlink());
            }
        }
        
    }

    void Die(){
        Destroy(this.gameObject);
    }

    private IEnumerator HitStunFreeze() {
        Time.timeScale = 0.05f;
        float pauseEndTime = Time.realtimeSinceStartup + 0.1f;
        while(Time.realtimeSinceStartup < pauseEndTime){    
            yield return 0;
        }
        Time.timeScale = 1;
        if(currentHealth <= 0){
            Die();
        }
    }
    private IEnumerator DamageBlink(){
        CanTakeDamage = false;
        //set blinking state
        for(int i = 0; i < 4; i++){
            foreach(Material mat in material){
                mat.SetFloat("_FlashAmount", 1f);
            }
            yield return new WaitForSeconds(0.1f);
            foreach(Material mat in material){
                mat.SetFloat("_FlashAmount", 0f);
            }
            yield return new WaitForSeconds(0.1f);
        }
        CanTakeDamage = true;
    }
}