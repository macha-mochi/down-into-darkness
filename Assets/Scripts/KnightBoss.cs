using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnightBoss : MonoBehaviour
{
    public GameObject player;
    public GameObject lantern;
    public float speed = 2.0f;
    public int damagePerHit;
    public float kb = 20;
    public GameObject healthbar;
    public GameObject projectile;
    public AudioSource attackSFX;
    private bool hasTarget = false;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private Health health;
    private Slider slider;
    private Dialogue d;
    private bool givenLantern = false;
    
    // Start is called before the first frame update
    private void OnEnable()
    {
        player.GetComponent<PlayerMovement>().ToggleBossfightMusic(true);
        healthbar.SetActive(true);
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        health = GetComponent<Health>();
        d = GetComponent<Dialogue>();
        slider = healthbar.GetComponent<Slider>();
        StartCoroutine(Attack());
    }
    private void Update()
    {
        if(!givenLantern) slider.value = ((float)health.GetHealth() / (float) health.GetMaxHealth());
        if(health.GetHealth() == 0 && !givenLantern)
        {
            player.GetComponent<PlayerMovement>().ToggleBossfightMusic(false);
            StopAllCoroutines();
            healthbar.SetActive(false);
            givenLantern = true;
            GameObject go = Instantiate(lantern, transform.position, transform.rotation);
            d.fightEnded = true;
            this.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "HurtBox" && collision.transform.parent.gameObject == player)
        {
            Health hit = player.GetComponent<Health>();
            int kbdir = 1;
            if ((player.transform.position.x - transform.position.x) < 0) kbdir = -1;
            hit.ApplyKnockback(kb, (new Vector2(kbdir, 0.4f)).normalized);
            hit.ChangeHealth(-damagePerHit);
        }
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(1.5f);
        attackSFX.Play();
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f);
        GameObject curr = Instantiate(projectile);
        curr.transform.right = -transform.right;
        curr.transform.position = transform.position - transform.right * 1f;
        yield return new WaitForSecondsRealtime(1f);
        attackSFX.Play();
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f);
        curr = Instantiate(projectile);
        curr.transform.right = -transform.right;
        curr.transform.position = transform.position - transform.right * 1f;
        yield return new WaitForSecondsRealtime(1f);
        attackSFX.Play();
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f);
        curr = Instantiate(projectile);
        curr.transform.right = -transform.right;
        curr.transform.position = transform.position - transform.right * 1f;
        yield return new WaitForSecondsRealtime(1f);
        StartCoroutine(Walk());
    }

    private IEnumerator Walk()
    {
        yield return new WaitForSeconds(2f);
        anim.SetBool("isWalking", true);
        rb.velocity = -transform.right * speed;
        yield return new WaitForSeconds(2f);
        rb.velocity = Vector2.zero;
        transform.right = -transform.right;
        anim.SetBool("isWalking", false);
        StartCoroutine(Attack());
    }

}
