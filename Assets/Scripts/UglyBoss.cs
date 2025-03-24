using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UglyBoss : MonoBehaviour
{
    public GameObject player;
    //public float speed = 2.0f;
    public int damagePerHit;
    public float kb = 20;
    public GameObject enemy;
    public Transform spawnPos;
    private bool hasTarget = false;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private Health health;
    // Start is called before the first frame update
    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        health = GetComponent<Health>();
        StartRandomState();

        if(PlayerPrefs.GetInt("Dash") == 1)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "HurtBox" && collision.transform.parent.gameObject == player)
        {
            Health hit = player.GetComponent<Health>();
            int kbdir = -1;
            hit.ApplyKnockback(kb, (new Vector2(kbdir, 0.4f)).normalized);
            hit.ChangeHealth(-damagePerHit);
        }
    }

    private IEnumerator SpawnSpider()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        GameObject curr = Instantiate(enemy);
        curr.GetComponent<Enemy_SlimeBlob>().player = player;
        curr.transform.position = spawnPos.position;
        yield return new WaitForSecondsRealtime(3f);
        StartRandomState();
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(1.5f);
        anim.SetTrigger("Explode");
        yield return new WaitForSeconds(1.5f);
        StartRandomState();
    }
    private IEnumerator Laser()
    {
        yield return new WaitForSeconds(1.5f);
        anim.SetTrigger("Laser");
        yield return new WaitForSeconds(1.5f);
        StartRandomState();
    }

    private void StartRandomState()
    {
        int rand = (Random.Range(1, 5));
        if(rand == 1)
        {
            StartCoroutine(SpawnSpider());
        }
        else if(rand == 2)
        {
            StartCoroutine(Explode());
        }
        else if(rand >= 3)
        {
            StartCoroutine(Laser());
        }
    }
}
