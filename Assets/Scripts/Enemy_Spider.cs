using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spider : MonoBehaviour
{
    public GameObject player;
    public GameObject cobweb;
    public float speed;
    public float trackingRange; //radius from original position
    public float attackingRange; //radius from spider position
    public float attackCooldown = 1.0f;
    private Vector2 originalPos;
    private Vector2 dir;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private float timeBeforeNextAttack = 0f;
    /*
     * spiders moves in the same space as character (cave walls), can move very quickly, basically top down
     * when the player is within a certain range, spider crawls towards player and spits cobwebs.
     * when hit by a cobweb, player is slowed down, but cobwebs dont do a lot of dmg
     * spider can only hurt player via cobwebs
     * will go towards player until it reaches the end of its range (circle with radius trackingRange), at which point it will
     * just not move towards player anymore and keep spitting cobwebs until player is out of cobweb range
     * when player is out of cobweb range it moves back to original position
     * cobwebs will be their own game objects, an opening animation will play when they hit something
     */

    //Nice comments :D
    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<PlayerMovement>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(GetComponent<Health>().GetHealth());
        Vector2 playerPos = player.transform.position;
        if((playerPos - (Vector2)transform.position).magnitude <= trackingRange)
        {
            if ((playerPos - (Vector2)transform.position).magnitude <= 2)
            {
                rb.velocity = Vector2.zero;
            }
            else
            {
                dir = decideDirection(playerPos);
                rb.velocity = dir * speed;
            }
            if ((playerPos - (Vector2)transform.position).magnitude <= attackingRange && timeBeforeNextAttack == 0f)
            {
                //Debug.Log("moving and shooting");
                ShootCobweb();
                timeBeforeNextAttack = attackCooldown;
            }
            else {
                //ebug.Log("moving towards player");
            }
        }
        else if((playerPos - (Vector2)transform.position).magnitude <= attackingRange && timeBeforeNextAttack == 0f)
        {
            //Debug.Log("staying still and shooting");
            //stay there and shoot cobweb
            ShootCobweb();
            timeBeforeNextAttack = attackCooldown;
            rb.velocity = new Vector2(0, 0);
        }
        else
        {
            //move back to original spot
            if (((Vector2)transform.position - originalPos).magnitude > 0.1f)
            {
                //Debug.Log("moving back to original spot");
                dir = decideDirection(originalPos);
                rb.velocity = dir * speed;
            }
            else
            {
                //Debug.Log("at original spot");
                rb.velocity = new Vector2(0, 0);
            }
        }
        if (timeBeforeNextAttack > 0)
        {
            timeBeforeNextAttack = Mathf.Max(0, timeBeforeNextAttack - Time.deltaTime);
        }
        if(rb.velocity.magnitude != 0)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
        if(dir == Vector2.left)
        {
            anim.SetBool("facingFront", false);
            anim.SetBool("facingUp", false);
            anim.SetBool("facingSide", true);
            sr.flipX = true;
        }
        else if(dir == Vector2.right)
        {
            anim.SetBool("facingFront", false);
            anim.SetBool("facingUp", false);
            anim.SetBool("facingSide", true);
            sr.flipX = false;
        }else if(dir == Vector2.up)
        {
            anim.SetBool("facingFront", false);
            anim.SetBool("facingUp", true);
            anim.SetBool("facingSide", false);
        }else if(dir == Vector2.down)
        {
            anim.SetBool("facingFront", true);
            anim.SetBool("facingUp", false);
            anim.SetBool("facingSide", false);
        }
    }
    private void ShootCobweb()
    {
        GameObject cw = Instantiate(cobweb, transform.position, transform.rotation);
        Cobweb_Projectile pr = cw.GetComponent<Cobweb_Projectile>();
        pr.player = player;
        pr.spider = this.gameObject;
        pr.dir = (Vector2)(player.transform.position - this.transform.position).normalized;
    }
    private Vector2 decideDirection(Vector2 target)
    {
        float xDiff = target.x - transform.position.x;
        float yDiff = target.y - transform.position.y;
        if(Mathf.Abs(xDiff) > Mathf.Abs(yDiff))
        {
            //prioritize closing x distance
            if (xDiff < 0) return Vector2.left;
            else return Vector2.right;
        }
        else
        {
            if (yDiff < 0) return Vector2.down;
            else return Vector2.up;
        }
    }
}
