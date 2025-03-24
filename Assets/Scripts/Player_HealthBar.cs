using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_HealthBar : MonoBehaviour
{
    [SerializeField] private Health health;
    private Slider slider;
    [SerializeField]private float scale = 150;

    //TODO: change player health script to update healthbar

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    private void Update()
    {
        slider.value = (float)health.GetHealth()/health.GetMaxHealth();
        transform.localScale = new Vector3((float)health.GetMaxHealth()/scale, 1);
    }
}
