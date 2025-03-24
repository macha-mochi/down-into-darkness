using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cobweb_Projectile : MonoBehaviour
{
    public float speed = 2f;
    public Vector2 dir;
    public GameObject player;
    public GameObject spider;
    public float duration = 5f; //while its on the ground it slows down player?
    private Rigidbody2D rb;
    private Animator anim;
    private bool opened = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!opened)
        {
            rb.velocity = dir * speed;
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collided with " + collision.gameObject.name);
        if(collision.gameObject != spider)
        {
            anim.SetBool("hit", true);
            opened = true;
            StartCoroutine(breakdown());
            if(collision.gameObject == player)
            {
                player.GetComponent<Health>().ChangeHealth(-10);
            }
        }
    }
    private IEnumerator breakdown()
    {
        yield return new WaitForSeconds(duration);
        anim.SetTrigger("breakDown");
        Destroy(this.gameObject, 0.5f);
    }
}
