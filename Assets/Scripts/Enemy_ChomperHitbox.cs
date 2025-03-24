using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_ChomperHitbox : MonoBehaviour
{
    private GameObject player;
    [SerializeField] Enemy_Chomper chomperScript;
    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>().gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "HurtBox" && collision.transform.parent.gameObject == player)
        {
            chomperScript.HitPlayer();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "HurtBox" && collision.transform.parent.gameObject == player)
        {
            chomperScript.UnhitPlayer();
        }
    }
}
