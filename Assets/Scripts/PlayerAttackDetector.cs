using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackDetector : MonoBehaviour
{
    [SerializeField] private LayerMask enemy;
    [SerializeField] private int damage = 30;
    [SerializeField] private Transform player;
    [SerializeField] private float kb = 20f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Health hit = collision.GetComponent<Health>();
            hit.ChangeHealth(-damage);
            int kbDir = 1;
            if (player.eulerAngles.y == 180) kbDir = -1;
            hit.ApplyKnockback(kb, (new Vector2(kbDir, 0.4f)).normalized);
        }
    }

    public int GetDamage()
    {
        return damage;
    }

    public void ChangeDamage(int amt)
    {
        damage += amt;
    }
}
    