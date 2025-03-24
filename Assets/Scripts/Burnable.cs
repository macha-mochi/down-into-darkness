using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burnable : MonoBehaviour
{
    [SerializeField] float minBurn;
    [SerializeField] float maxBurn;
    [SerializeField] GameObject particleSystem;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Lantern"))
        {
            startBurn();
        }
    }

    private void startBurn()
    {
        StartCoroutine(burnTile());
    }

    private IEnumerator burnTile()
    {
        particleSystem.SetActive(true);
        float waitAmt = Random.Range(minBurn, maxBurn);
        yield return new WaitForSeconds(waitAmt);

        Collider2D[] surroundingTiles = Physics2D.OverlapCircleAll(transform.position, 2f);
        for (int i = 0; i < surroundingTiles.Length; i++)
        {
            if (surroundingTiles[i].tag == "Burnable" && surroundingTiles[i].gameObject != this)
            {
                surroundingTiles[i].gameObject.GetComponent<Burnable>().startBurn();
            }
                
        }

        Destroy(gameObject);
    }
}
