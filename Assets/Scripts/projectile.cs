using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private int damage;
    [SerializeField] private float speed;
    [SerializeField] private float kb;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
        Destroy(gameObject, duration);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "HurtBox")
        {
            GameObject player = collision.transform.parent.gameObject;
            Health hit = player.GetComponent<Health>();
            int kbdir = 1;
            if ((player.transform.position.x - transform.position.x) < 0) kbdir = -1;
            hit.ApplyKnockback(kb, (new Vector2(kbdir, 0.4f)).normalized);
            hit.ChangeHealth(-damage);
        }
    }
}
