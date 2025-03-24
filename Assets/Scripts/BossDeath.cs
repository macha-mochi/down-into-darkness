using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeath : MonoBehaviour
{
    [SerializeField] ItemQualities bossDrop;
    [SerializeField] string methodName;
    public void OnBossDeath()
    {
        ItemManager.instance.SetAwardScreen(bossDrop.itemName, bossDrop.itemImage, bossDrop.description);

        StartCoroutine(ItemManager.instance.ActivatePickupEffects(FindObjectOfType<PlayerMovement>().transform));

        FindObjectOfType<PlayerMovement>().SendMessage(methodName);
        FindObjectOfType<PlayerMovement>().ToggleBossfightMusic(false);
        Debug.Log("Boss is dead");
    }
}
