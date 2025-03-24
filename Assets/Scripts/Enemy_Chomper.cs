using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Chomper : MonoBehaviour
{
    private GameObject player;
    private Vector2 playerPos;
    [SerializeField] float attackThresh;
    [SerializeField] float attackCooldown = 1.0f;
    [SerializeField] float hitOffsetX;
    [SerializeField] float hitOffsetY;
    [SerializeField] int damagePerHit;
    [SerializeField] float kb = 20;
    [SerializeField] BoxCollider2D childCollider2D;
    private bool hasTarget = false;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider2D;
    private Animator anim;
    private SpriteRenderer sr;

    private Vector2 dir;
    private float timeBeforeNextAttack = 0f;
    private Health health;
    private bool canHit;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().gameObject;
        rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        health = GetComponent<Health>();
        dir = Vector2.right;
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = player.transform.position;
        float distToPlayer = Vector2.Distance(playerPos, transform.position);
        if (distToPlayer <  attackThresh && timeBeforeNextAttack == 0f)
        {
            Attack();
        }
        if(timeBeforeNextAttack > 0)
        {
            timeBeforeNextAttack = Mathf.Max(0, timeBeforeNextAttack - Time.deltaTime);
        }
    }
    void Attack()
    {
        if (!health.IsTakingKnockback())
        {
            childCollider2D.enabled = true;
            if (playerPos.x < transform.position.x) {
                anim.SetTrigger("isAttackingLeft");
                childCollider2D.offset = new Vector2(-hitOffsetX, hitOffsetY);
            } else
            {
                anim.SetTrigger("isAttackingRight");
                childCollider2D.offset = new Vector2(hitOffsetX, hitOffsetY);
            }
                
        }
            
    }

    public void HitPlayer()
    {
        timeBeforeNextAttack = attackCooldown;
        canHit = true;
        StartCoroutine(AttackBuffer());
    }

    public void UnhitPlayer()
    {
        canHit = false;
    }


    IEnumerator AttackBuffer()
    {
        yield return new WaitForSeconds(0.85f);
        if (canHit)
        {
            Health hit = player.GetComponent<Health>();
            int kbdir = 1;
            if ((player.transform.position.x - transform.position.x) < 0) kbdir = -1;
            hit.ApplyKnockback(kb, (new Vector2(kbdir, 0.4f)).normalized);
            hit.ChangeHealth(-damagePerHit);
            childCollider2D.enabled = false;

        }
    }
}
