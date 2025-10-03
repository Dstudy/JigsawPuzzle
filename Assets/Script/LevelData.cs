using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    public static LevelData Instance;
    public List<PuzzleController> puzzles;
    public List<GameObject> level;
    public List<int> size;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }
}
