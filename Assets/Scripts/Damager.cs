using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float kb;
    [SerializeField] private float kbDir;
    [SerializeField] private bool dies;
    [SerializeField] private bool specialDir;
    [SerializeField] private float lifeSpan;

    private void Start()
    {
        if (dies) Destroy(gameObject, lifeSpan);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "HurtBox")
        {
            GameObject player = collision.transform.parent.gameObject;
            Health hit = player.GetComponent<Health>();
            if (specialDir)
            {
                kbDir = transform.parent.right.x;
            }
            hit.ApplyKnockback(kb, (new Vector2(kbDir, 0.4f)).normalized);
            hit.ChangeHealth(-damage);
        }
    }
}
