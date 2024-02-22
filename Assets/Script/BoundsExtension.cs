using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsExtension : MonoBehaviour
{
    public BoundsExtension Instance;
    
    private void Awake()
    {
        Instance = this;
    }
    
    
}
