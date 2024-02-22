using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextJumpAnim : MonoBehaviour
{
    public static TextJumpAnim Instance;
    public float waitTime = 0.5f;
    private List<Animator> _animators;
    
    private void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        _animators = new List<Animator>(GetComponentsInChildren<Animator>());
    }

    public void Jump()
    {
        StartCoroutine(JumpAnim());
    }
    
    IEnumerator JumpAnim()
    {
        foreach (var animator in _animators)
        {
            animator.Play("TextAnim");
            yield return new WaitForSeconds(waitTime);
        }
    }
}
