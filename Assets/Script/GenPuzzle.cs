using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenPuzzle: MonoBehaviour
{
    public List<int> levelSize;
    public List<Sprite> levelImage;
    public List<Texture2D> levelTexture;
    public static GenPuzzle Instance;

    private void Awake()
    {
        Instance = this;
        Debug.Log("The object is awake");
    }

    private void OnEnable()
    {
        Instance = this;
    }
}
