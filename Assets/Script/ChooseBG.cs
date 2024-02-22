using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChooseBG : MonoBehaviour
{
    public GameObject[] BgList;
    public static int currentBG = 0;
    public static ChooseBG Instance;
    
    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }
    
    void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            gameObject.SetActive(false);
            Debug.Log("Click Outside");
        }
    }

    public void SelectBG(int num)
    {
        currentBG = num;
        for (int i = 0; i < BgList.Length; i++)
        {
            BgList[i].SetActive(false);
        }
        BgList[num].SetActive(true);
    }
}

