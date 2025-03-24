using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern : MonoBehaviour
{
    [SerializeField] float moveThresh;
    [SerializeField] float moveMulti;
    [SerializeField] float maxSpeed;
    [SerializeField] float maxTiltAngle;
    [SerializeField] Rigidbody2D rb2d;

    [SerializeField] ItemQualities itemQualities;

    private GameObject player;
    private Vector2 playerPos;
    private float distToPlayer;

    private bool pickedUp;

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().gameObject;
        SetPickedUp(PlayerPrefs.GetInt("Lantern") == 1);
        if (pickedUp)
        {
            transform.position = new Vector2(player.transform.position.x - 2, player.transform.position.y + 2);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pickedUp)
        {
            playerPos = player.transform.position;
            distToPlayer = Vector2.Distance(playerPos, transform.position);

            if (distToPlayer > moveThresh)
            {
                float speed = Mathf.Min(maxSpeed, distToPlayer * moveMulti);
                Vector2 dir = new Vector2(playerPos.x - transform.position.x, playerPos.y - transform.position.y);
                rb2d.velocity = dir.normalized * speed;

                float angle = rb2d.velocity.x > 0 ? -maxTiltAngle : maxTiltAngle * speed / maxSpeed;
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, angle);
            }
            else
            {
                rb2d.velocity = Vector2.zero;
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
            }
        }
    }

    public void SetPickedUp(bool pickedUp)
    {
        this.pickedUp = pickedUp;
        
    }

    public bool GetPickedUp()
    {
        return pickedUp;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.Equals(player) && !pickedUp)
        {
            pickedUp = true;
            ItemManager.instance.SetAwardScreen(itemQualities.itemName, itemQualities.itemImage, itemQualities.description);
            StartCoroutine(ItemManager.instance.ActivatePickupEffects(transform));

        }
    }
}
