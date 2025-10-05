using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WinAnimation : MonoBehaviour
{
    public static WinAnimation Instance;
    public float waitTime = 0.0001f;
    public GameObject winPanel;
    public GameObject winFrame;
    public GameObject spinObject;
    
    public ParticleSystem winPuzzleAnim;
    public ParticleSystem winConfetti;

    public GameObject BG1;
    public GameObject BG2;
    public GameObject gameInfor;
    public GameObject textContainer;
    public GameObject botNav;

    public Text timeText;
    public Text pieceNum;

    public GameObject BgFrame;

    private Vector3 orgiginalPos;
    
    private void Awake()
    {
        Instance = this;
    }
    
    public void Win()
    {
        StartCoroutine(WinAnim());
    }

    public void ShowWinPanelOnly()
    {
        winPanel.SetActive(true);
        winFrame.SetActive(true);
        botNav.SetActive(true);
        gameInfor.SetActive(true);
        textContainer.SetActive(true);
        spinObject.SetActive(true);
        winFrame.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = GenPuzzle.Instance.levelImage[GameController.Instance.currentLevel];
        ShowInfor();
    }

    IEnumerator WinAnim()
    {
        BgFrame.SetActive(false);
        PuzzleController.Instance.gameObject.SetActive(false);
        if(!winConfetti.gameObject.activeInHierarchy)
            winConfetti.gameObject.SetActive(true);
        winConfetti.Play();
        
        GameController.Instance.PlayMusic(GameController.Instance.musicWin, false);
        GameController.Instance.blockRaycast.SetActive(true);
        
        GameController.Instance.controller.gameObject.SetActive(false);
        
        yield return new WaitForSeconds(1f);
        
        if(!winPanel.activeInHierarchy)
            winPanel.SetActive(true);
        
        orgiginalPos = textContainer.GetComponent<RectTransform>().position;
        textContainer.GetComponent<RectTransform>().position = Vector3.zero;
        
        for(int i = 0; i < textContainer.transform.childCount; i++)
        {
            textContainer.transform.GetChild(i).GetComponent<Text>().color = new Color(232, 78, 64, 0);
        }
        
        StartCoroutine(TextAnim());
        
        if(!winPuzzleAnim.gameObject.activeInHierarchy)
            winPuzzleAnim.gameObject.SetActive(true);
        winPuzzleAnim.Play();
        
        
        BG1.transform.DOScaleY(1, 1f).SetEase(Ease.OutCirc);
        BG2.transform.DOScaleY(1, 1.2f).SetEase(Ease.OutCirc);
        
        yield return new WaitForSeconds(1.5f);
        
        textContainer.transform.GetComponent<RectTransform>().DOMove(orgiginalPos, 1.5f).SetEase(Ease.InOutCubic);
        
        yield return new WaitForSeconds(0.5f);
        
        if(!botNav.activeInHierarchy)
            botNav.SetActive(true);
        botNav.GetComponent<Animator>().Play("FloatIn");
        
        if (!winFrame.activeInHierarchy)
        {
            winFrame.SetActive(true);
        }
        winFrame.GetComponent<Animator>().Play("FloatIn");
        winFrame.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = GenPuzzle.Instance.levelImage[GameController.Instance.currentLevel];
        
        
        yield return new WaitForSeconds(0.75f);
        
        ShowInfor();
        if(!spinObject.activeInHierarchy)
            spinObject.SetActive(true);
        spinObject.transform.DOLocalRotate(new Vector3(0,0,360), 100f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental);
        GameController.Instance.blockRaycast.SetActive(false);
    }

    private void ShowInfor()
    {
        gameInfor.SetActive(true);
        if(!textContainer.activeInHierarchy)
            textContainer.SetActive(true);
        string textTime = GameController.Instance.GetElapsedTime();
        if(timeText != null)
            timeText.text = textTime;
        if(pieceNum != null)
            pieceNum.text = GameController.Instance.puzzle.totalPieces.ToString();
    }

    IEnumerator TextAnim()
    {
        for(int i = 0; i < textContainer.transform.childCount; i++)
        {
            textContainer.transform.GetChild(i).GetComponent<Text>().DOFade(1, .5f);
            textContainer.transform.GetChild(i).GetComponent<RectTransform>().DOMoveY(1, 1f).SetEase(Ease.OutBack);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
