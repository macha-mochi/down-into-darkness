using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    [SerializeField] private float speed = 10;
    [SerializeField] private float turnSpeed = 10;
    [SerializeField] private Animator anim;
    public GameObject player;
    private bool isDead = false;
    private void Start()
    {
        Invoke("SelfDestruct", 2);
    }
    void Update()
    {
        if (!isDead)
        {
            Vector3 targetDirection = player.transform.position - transform.position;
            float singleStep = turnSpeed * Time.deltaTime;
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
            Quaternion targetRot = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, turnSpeed * Time.deltaTime);
            transform.position += transform.right * speed * Time.deltaTime;
        }
    }

    private void SelfDestruct()
    {
        anim.SetTrigger("Die");
        isDead = true;
        Destroy(gameObject, 0.5f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "HurtBox")
        {
            Health hit = player.GetComponent<Health>();
            int kbdir = 1;
            if ((player.transform.position.x - transform.position.x) < 0) kbdir = -1;
            hit.ApplyKnockback(10, (new Vector2(kbdir, 0.4f)).normalized);
            hit.ChangeHealth(-20);
            CancelInvoke();
            SelfDestruct();
        }
    }
}
