using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class NavBarController : MonoBehaviour
{
    public Button backButton;
    public Button showMusicBtn;
    public Button showBgBtn;
    public Button changeBgBtn;
    public Button sortBtn;
    public bool isShowed;
    public bool isOn;
    public bool isSort;
    public GameObject sort, unSort;
    public GameObject showBg, showMusic;
    public GameObject hideBg, hideMusic;
    public SpriteRenderer backGround;

    public GameObject BGPanel;

    public static NavBarController Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Debug.Log(transform.position + " + " + GetComponent<Image>().sprite.bounds.size);
        GameObject foundObject = GameObject.FindGameObjectWithTag("Puzzle_Background");
        if (foundObject)
        {
            backGround = foundObject.GetComponent<SpriteRenderer>();
            Debug.Log(foundObject.gameObject.name);
        }
        else
        {
            Debug.Log("Not found");
        }
        
        showBgBtn.onClick.AddListener( (() =>
        {
            // Debug.Log("CLickShowBtn");
            isShowed = backGround.enabled;
            showBg.SetActive(!isShowed);
            hideBg.SetActive(isShowed);
            backGround.enabled = !isShowed;
        }));
        
        showMusicBtn.onClick.AddListener( (() =>
        {
            // Debug.Log("CLickShowBtn");
            isOn = GameController.Instance.musicPlayer.enabled;
            showMusic.SetActive(!isOn);
            hideMusic.SetActive(isOn);
            GameController.Instance.musicPlayer.enabled = !isOn;
        }));
        
        changeBgBtn.onClick.AddListener((() =>
        {
            if (!BGPanel.activeInHierarchy)
            {
                BGPanel.SetActive(true);
            }
            else
            {
                BGPanel.SetActive(false);
            }
        }));
        
        sortBtn.onClick.AddListener((() =>
        {
            isSort = sort.activeInHierarchy;
            if(!isSort)
            {
                StartCoroutine(GameController.Instance.controller.Arrange());
                isSort = true;
                sort.SetActive(true);
                unSort.SetActive(false);
            }
            else
            {
                sort.SetActive(false);
                unSort.SetActive(true);
                StartCoroutine(GameController.Instance.controller.Shuffle());
            }
        }));
        
        backButton.onClick.AddListener((() =>
        {
            GameController.Instance.Save();
            SceneManager.LoadScene("Menu");
        }));
    }

    
}
