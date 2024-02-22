using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PieceCtl : MonoBehaviour
{
    int id;
    public static PieceCtl Instance;
    
    public ScrollRect scrollRect;
    private Vector3 initialPosition;
    private Transform ownTransform;
    
    private void Awake()
    {
        Instance = this;
        scrollRect = GameController.Instance.controller.scroll;
    }
    
    public void Init(int id)
    {
        this.id = id;
    }
    

    private void AdjustElementPositions(int startIndex)
    {
        for (int i = startIndex; i < scrollRect.content.childCount; i++)
        {
            Transform child = scrollRect.content.GetChild(i);
            if (child != null)
            {
                float newPosition = child.position.x + ownTransform.lossyScale.x;
                child.position = new Vector3(newPosition, child.position.y, child.position.z);
            }
        }
    }
}
