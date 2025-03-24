using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] Canvas canvas;
    [SerializeField] GameObject tutorialsMenu;

    private void Start()
    {
        canvas.worldCamera = Camera.main;
    }
    public void tutorialButtonClicked()
    {
        Debug.Log("opened tutorial menu");
        tutorialsMenu.SetActive(true);
    }
    public void closeTutorialMenu()
    {
        Debug.Log("closed tutorial menu");
        tutorialsMenu.SetActive(false);
    }
}
