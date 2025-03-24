using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject player;
    [SerializeField] GameObject fire;
    [SerializeField] private bool clearAll = false; //only set to true if you want to clear all the keys
    [SerializeField] AudioClip fireNoise;
    AudioSource audioSource;
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().gameObject;
        Debug.Log("found player: " + player != null);
        if (clearAll)
        {
            PlayerPrefs.DeleteAll();
        }
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Checkpoint");
            fire.SetActive(true);
            StorePlayerValues();
            audioSource.PlayOneShot(fireNoise);
        }
    }

    private void StorePlayerValues()
    {
        PlayerPrefs.SetFloat("X Location", player.transform.position.x);
        PlayerPrefs.SetFloat("Y Location", player.transform.position.y);
        //Powerups here (make it an int, 0 for if its not been collected, 1 if it has)
        //example: PlayerPrefs.SetInt("Reaper Boss Killed", reaperKilled ? 1 : 0);


        //Powerups here (make it an int, 0 for if its not been collected, 1 if it has)
        if (FindObjectOfType<Lantern>() != null) {
        bool collectedLantern = FindObjectOfType<Lantern>().GetPickedUp();
        PlayerPrefs.SetInt("Lantern", collectedLantern ? 1 : 0);
           }
        
        bool collectedDash = player.GetComponent<PlayerMovement>().getHasDash();
        PlayerPrefs.SetInt("Dash", collectedDash ? 1 : 0);
        bool collectedGroundChannel = player.GetComponent<PlayerMovement>().getHasGroundChannel();
        PlayerPrefs.SetInt("Ground Channel", collectedGroundChannel? 1 : 0);
        PlayerPrefs.SetInt("Max Health", player.GetComponent<Health>().GetMaxHealth());
        PlayerPrefs.SetInt("Damage", player.GetComponentInChildren<PlayerAttackDetector>().GetDamage());

    }
}
