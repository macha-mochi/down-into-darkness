using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_SlimeBlob : MonoBehaviour
{
    public GameObject player;
    public float minX;
    public float maxX;
    public float maxYDiff;
    public float speed = 2.0f;
    public float attackCooldown = 1.0f;
    public float attackRange;
    public int damagePerHit;
    public float kb = 20;
    private bool hasTarget = false;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private Vector2 dir;
    private float timeBeforeNextAttack = 0f;
    private Health health;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        health = GetComponent<Health>();
        player = FindObjectOfType<PlayerMovement>().gameObject;
        dir = Vector2.right;
        maxX = transform.position.x + 2;
        minX = transform.position.x - 2;
    }

    // Update is called once per frame
    void Update()
    {
        /*
         * if they have not spotted the player: just patrol the platform they are on
         * or define an active range so that they arent limited to just platforms
         * when the player gets within their range, they will go towards the player and 
         * they will attack when within a certain range by jumping (give them an attack cooldown), 
         * they can only hurt you once per attack. once you leave their active range they leave you alone
         */

        /*
         * ok this might not be super optimized but for now ill just make it so that it has a target
         * if the player is within its minX and maxX and within a certain y distance.
         * if we want to optimize we can do something like 2 box colliders at min X and max X
         * to detect when the player enters/exits
         */
        Vector2 playerPos = player.transform.position;
        if (minX <= playerPos.x && playerPos.x <= maxX && Mathf.Abs(playerPos.y - transform.position.y) <= maxYDiff)
        {
            hasTarget = true;
        }
        else
        {
            hasTarget = false;
        }
        if (hasTarget)
        {
            /*
             * check if player is within attacking range
             * if yes, attack, if player moves out of range, chase them
             */
            if(Mathf.Abs(playerPos.x - transform.position.x) <= attackRange && timeBeforeNextAttack == 0f) {
                Attack();
            }
            else
            {
                if (playerPos.x < transform.position.x)
                {
                    //player is to the left of enemy
                    dir = Vector2.left;
                    sr.flipX = false;
                    
                }
                else
                {
                    //player is to the right of enemy
                    dir = Vector2.right;
                    sr.flipX = true;
                }
            }
        }
        else
        {
            if(transform.position.x >= maxX)
            {
                dir = Vector2.left;
                sr.flipX = false;
            }else if(transform.position.x <= minX)
            {
                dir = Vector2.right;
                sr.flipX = true;
            }
        }
        if (!health.IsTakingKnockback())
            rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
        if(timeBeforeNextAttack > 0)
        {
            timeBeforeNextAttack = Mathf.Max(0, timeBeforeNextAttack - Time.deltaTime);
        }
    }
    void Attack()
    {
        if(!health.IsTakingKnockback()) anim.SetTrigger("isAttacking");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "HurtBox" &&  collision.transform.parent.gameObject == player && timeBeforeNextAttack == 0f) {
            Debug.Log("player will take damage");
            timeBeforeNextAttack = attackCooldown;
            Health hit = player.GetComponent<Health>();
            int kbdir = 1;
            if ((player.transform.position.x - transform.position.x) < 0) kbdir = -1;
            hit.ApplyKnockback(kb, (new Vector2(kbdir, 0.4f)).normalized);
            hit.ChangeHealth(-damagePerHit);
        }
    }
}
