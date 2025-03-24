using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float knockbackDuration = 0.5f;
    [SerializeField] private bool takeskb = true;
    [SerializeField]private bool isPlayer;
    [SerializeField] private bool isSmallEnemy;
    [SerializeField] private GameObject enemyCorruption;
    [SerializeField] private AudioSource damageSFX;
    private Rigidbody2D rb;
    private int health;
    private Animator anim;
    private SpriteRenderer sr;
    [SerializeField] private float iframes;
    private bool takingKnockback = false;
    private bool canTakeDamage;
    [SerializeField] UnityEvent eventOnDeath;

    private void Start()
    {
        health = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        canTakeDamage = true;
    }
    public void ChangeHealth(int amt)
    {
        if(amt > 0)
        {
            health += amt;
            if (health > maxHealth) health = maxHealth;
        }
        else if (canTakeDamage)
        {
            health += amt;
            if (damageSFX != null) damageSFX.Play();
            if (health <= 0) Die();
            canTakeDamage = false;
            StartCoroutine(Iframes());
        }
    }

    public void ChangeMaxHealth(int amt)
    {
        maxHealth += amt;
    }

    public int GetHealth()
    {
        return health;
    }
    public int GetMaxHealth()
    {
        return maxHealth;
    }

    private void Die()
    {
        if(eventOnDeath != null)
        {
            eventOnDeath.Invoke();
        }
        if(anim!=null)anim.SetTrigger("isDead");
        if (isSmallEnemy)
        {
            GameObject spawnedCorruption = Instantiate(enemyCorruption);
            spawnedCorruption.transform.position = transform.position;
        }
        if(!isPlayer) StartCoroutine(Dead());
    }

    private IEnumerator Dead()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1;
        Destroy(gameObject);
    }

    public bool IsTakingKnockback()
    {
        return takingKnockback;
    }
    public void ApplyKnockback(float amt, Vector2 dir)
    {
        if (takeskb)
        {
            takingKnockback = true;
            StartCoroutine(knockback(amt, dir));
        }
    }

    private IEnumerator knockback(float amt, Vector2 dir)
    {
        rb.velocity= (dir * amt);
        Debug.Log(rb.velocity);
        yield return new WaitForSeconds(knockbackDuration);
        takingKnockback = false;

    }

    private IEnumerator Iframes()
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(0.1f);
        Time.timeScale = 1;
        sr.color = new Color(106, 13, 173, 0.5f); //purple and half opacity
        yield return new WaitForSecondsRealtime(iframes);
        canTakeDamage = true;
        sr.color = new Color(256, 256, 256); //regualar
    }
}
