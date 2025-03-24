using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossFightTrigger : MonoBehaviour
{
    public GameObject player;
    public int bossNumb;
    public UglyBoss uglyBoss;
    public NightBorneBoss nightBorneBoss;
    public BulletHellBoss bulletHellBoss;
    public GameObject[] walls;
    public Health bossHealth;
    public GameObject healthbar;
    private Slider slider;
    private bool hasTriggered;
    private bool isDead = false;
    private void Start()
    {
        slider = healthbar.GetComponent<Slider>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!hasTriggered && collision.gameObject == player)
        {
            player.GetComponent<PlayerMovement>().ToggleBossfightMusic(true);
            hasTriggered = true;
            healthbar.SetActive(true);
            if(bossNumb == 2)
            {
                uglyBoss.enabled = true;
            }
            else if(bossNumb == 3)
            {
                nightBorneBoss.enabled = true;
            }
            else if (bossNumb == 4)
            {
                bulletHellBoss.enabled = true;
            }
            for(int i = 0; i < walls.Length; i++)
            {
                walls[i].SetActive(true);
            }
        }
    }
    private void Update()
    {
        if(hasTriggered && !isDead)slider.value = ((float)bossHealth.GetHealth() / (float)bossHealth.GetMaxHealth());
        Debug.Log(((float)bossHealth.GetHealth() / (float)bossHealth.GetMaxHealth()));
        if(bossHealth.GetHealth() <= 0 && !isDead)
        {
            isDead = true;
            healthbar.SetActive(false);
            for (int i = 0; i < walls.Length; i++)
            {
                walls[i].SetActive(false);
            }
        }
    }
}

