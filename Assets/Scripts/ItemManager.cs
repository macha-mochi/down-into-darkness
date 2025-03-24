using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    [Header("Item Awarded Group Settings")]
    [SerializeField] CanvasGroup awardItemGroup;
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] Image itemSprite;
    [SerializeField] TextMeshProUGUI description;

    [Header("Particle FX")]
    [SerializeField] ParticleSystem powerFX;

    [Header("Quality Settings")]
    [SerializeField] float fadeSpeed = 0.5f;

    private void Awake()
    {
        instance = this;
    }
    public void SetAwardScreen(string itemName, Sprite itemImage, string itemDescription)
    {
        this.itemName.text = itemName;
        itemSprite.sprite = itemImage;
        description.text = itemDescription;
    }
    public IEnumerator ActivatePickupEffects(Transform pos)
    {
        Instantiate(powerFX, pos);
        FindObjectOfType<PlayerMovement>().DisablePlayer(true);
        yield return new WaitForSeconds(4);

        Time.timeScale = 0;

        while (awardItemGroup.alpha < 0.999f)
        {
            yield return new WaitForSecondsRealtime(.1f);
            awardItemGroup.alpha = Mathf.Lerp(awardItemGroup.alpha, 1, fadeSpeed);
        }

        yield return new WaitForSecondsRealtime(5f);
        while (awardItemGroup.alpha > 0.1f)
        {
            yield return new WaitForSecondsRealtime(.1f);
            awardItemGroup.alpha = Mathf.Lerp(awardItemGroup.alpha, 0, fadeSpeed);
        }
        awardItemGroup.alpha = 0;
        Time.timeScale = 1;
        FindObjectOfType<PlayerMovement>().DisablePlayer(false);
    }
}
[Serializable]
public struct ItemQualities
{
    public string itemName;
    public Sprite itemImage;

    [Multiline]
    public string description;
}