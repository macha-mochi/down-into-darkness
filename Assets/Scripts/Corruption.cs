using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corruption : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Corruption Pick Up")]
    [SerializeField] float pickupDist;
    [SerializeField] int minCorruption;
    [SerializeField] int maxCorruption;
    [SerializeField] float sizeMulti;
    [SerializeField] float moveMulti;
    [SerializeField] float maxSpeed;
    [SerializeField] float pickedUpRad;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;
    [SerializeField] CircleCollider2D circleCollider2D;
    [SerializeField] Rigidbody2D rb2d;

    [Header("Permanent Upgrades")]
    [SerializeField] bool givesPermHealth;
    [SerializeField] bool givesPermDamage;
    [SerializeField] int maxHealthBoost;
    [SerializeField] int damageBoost;

    [Header("Temporary Upgrades")]
    [SerializeField] int healthBoost;

    private int corruptionAmt;
    private GameObject player;
    private Health healthScript;
    private PlayerAttackDetector playerAttackScript;
    private Vector2 playerPos;
    private float distToPlayer;
    private bool popped;
    private bool popping;
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().gameObject;
        healthScript = player.GetComponent<Health>();
        playerAttackScript = FindObjectOfType<PlayerAttackDetector>();
        corruptionAmt = Random.Range(minCorruption, maxCorruption + 1);
        transform.localScale = new Vector2(corruptionAmt * sizeMulti, corruptionAmt * sizeMulti);
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = player.transform.position;
        distToPlayer = Vector2.Distance(playerPos, transform.position);
        if (popped)
        {
            float speed = Mathf.Min(maxSpeed, moveMulti/distToPlayer);
            Vector2 dir = new Vector2(playerPos.x - transform.position.x, playerPos.y - transform.position.y);
            rb2d.velocity = dir.normalized * speed;


        } else if (distToPlayer < pickupDist && !popping)
        {
            animator.SetTrigger("Popped");
            StartCoroutine(PoppedCorruption());
            popping = true;
        }
    }

    IEnumerator PoppedCorruption()
    {
        yield return new WaitForSeconds(.2f);
        popping = false;
        popped = true;
        transform.localScale = new Vector2(0.35f, 0.35f);
        circleCollider2D.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.Equals(player))
        {
            if (popped)
            {
                GiveCorruption();
                Destroy(gameObject);
            }
            
        }
    }

    private void GiveCorruption()
    {
        if (givesPermHealth)
            healthScript.ChangeMaxHealth(maxHealthBoost);
        if (givesPermDamage)
            playerAttackScript.ChangeDamage(damageBoost);
        healthScript.ChangeHealth(healthBoost);
    }
}
