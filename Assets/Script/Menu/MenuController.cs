using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Button[] buttons;
    public static MenuController Instance;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetupLevelButtons();
    }

    public void SetupLevelButtons()
    {
        for(int i = 0; i < buttons.Length; i++)
        {
            int index = i; 
            buttons[i].onClick.AddListener(() => OnLevelButtonClick(index));
        }
    }
    
    private void OnLevelButtonClick(int index)
    {
        PlayerPrefs.SetInt("currentLevel", index);
        SceneManager.LoadScene("MainGame");
    }
}
