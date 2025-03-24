using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightBorneBoss : MonoBehaviour
{
    public GameObject player;
    public float teleportMinX;
    public float teleportMaxX;
    public float speed = 2.0f;
    public int damagePerHit;
    public float kb = 20;
    public float attackRange;
    public GameObject projectile;
    public AudioSource attackSFX;
    private bool hasTarget = false;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private Health health;
    private bool isAttacking;
    private bool chase;
    private bool range;
    private bool teleport;
    private BoxCollider2D bc;
    // Start is called before the first frame update
    private void OnEnable()
    {
        if (PlayerPrefs.GetInt("Ground Channel") == 1) Destroy(gameObject);
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        health = GetComponent<Health>();
        bc = GetComponent<BoxCollider2D>();
        chase = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!isAttacking)
        {
            transform.right = new Vector2(player.transform.position.x - transform.position.x, 0);
            if (chase) Chase();
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

    private void Chase()
    {
        if (Mathf.Abs(player.transform.position.x - transform.position.x) > attackRange)
        {
            anim.SetBool("IsRunning", true);
            rb.velocity = transform.right * speed;
        }
        else
        {
            anim.SetBool("IsRunning", false);
            rb.velocity = Vector2.zero;
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        attackSFX.Play();
        isAttacking = true;
        anim.SetTrigger("Attack");
        yield return new WaitForSecondsRealtime(2f);
        isAttacking = false;
        RandomState();
    }
    private IEnumerator Teleport()
    {
        rb.gravityScale = 0;
        bc.enabled = false;
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.5f);
        yield return new WaitForSecondsRealtime(0.3f);
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.1f);
        yield return new WaitForSecondsRealtime(0.3f);
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0f);

        transform.position = new Vector2(Random.Range(teleportMinX, teleportMaxX), transform.position.y);
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.1f);
        yield return new WaitForSecondsRealtime(0.3f);
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.5f);
        yield return new WaitForSecondsRealtime(0.3f);
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);

        bc.enabled = true;
        yield return new WaitForSecondsRealtime(0.3f);
        rb.gravityScale = 1;
        RandomState();
    }
    
    private IEnumerator RangedAttack()
    {
        GameObject curr = Instantiate(projectile);
        curr.transform.position = player.transform.position + new Vector3(0, 8, 0);
        yield return new WaitForSeconds(0.3f);
        RandomState();
    }

    private void RandomState()
    {
        chase = false;
        range = false;
        teleport = false;
        int rand = Random.Range(0, 3);
        if (rand == 0) chase = true;
        else if (rand == 1) range = true;
        else if (rand == 2) teleport = true;
        if (teleport)
        {
            StartCoroutine(Teleport());
        }
        else if (range)
        {
            StartCoroutine(RangedAttack());
        }
    }
}
