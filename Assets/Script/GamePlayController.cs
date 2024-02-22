// using System;
// using System.Collections;
// using System.Collections.Generic;
// // using GplaySDK.Ads;
// using UnityEngine;
// using UnityEngine.UI;
//
// public class GamePlayController : MonoBehaviour
// {
//     public LevelCtl levelCtl;
//     public HScrollController controller;
//     public static GamePlayController Instance;
//     public GameObject blockRaycast;
//     public GameObject UI;
//     public static int curLevel  = 0;
//     
//     //GameUI
//     public Text hintCounterUI;
//     public Text timerCounterUI;
//     public Text elapsedTimeUI;
//     public GameObject loseUI;
//     public GameObject sortPuzzleUI;
//     
//     public bool sortPuzzle = false;
//
//     //Timer
//     public float timer = 150f;					// Time limit for level
//     float timerTime = 20.0f;
//     float remainingTime, elapsedTime;
//     
//     //Hint 
//     public int remainingHints;
//     public int hintLimit = -1;			// Hints limit for level
//     
//     bool gameFinished = false;
//
//     private void Awake()
//     {
//         Instance = this;
//     }
//     
//     public void Init(int index)
//     {
//         // LoadData();
//         levelCtl.InitLevel(index);
//         controller.Init();
//         GameController.Instance.FindPuzzle();
//         // LoadData();
//         UI.gameObject.transform.SetAsLastSibling();
//     }
//
//     private void Update()
//     {
//         ProcessTimer ();
//         if (elapsedTimeUI) elapsedTimeUI.text = GetElapsedTime();
//     }
//
//     private void OnEnable()
//     {   
//         // LoadData();
//         Init(curLevel);
//         timerTime = Time.time + timer;
//         Time.timeScale = 1.0f;
//         if (elapsedTimeUI)
//             elapsedTimeUI.text = SecondsToTimeString(elapsedTime);
//     }
//
//     private void Start()
//     {
//         if (hintCounterUI) 
//         {
//             if (hintCounterUI.transform.parent)
//                 hintCounterUI.transform.parent.GetChild(3).gameObject.SetActive(remainingHints == 0);
//            
//             hintCounterUI.transform.gameObject.SetActive(remainingHints > 0);
//             hintCounterUI.transform.parent.GetChild(1).gameObject.SetActive(remainingHints > 0);
//             
//             hintCounterUI.text = remainingHints.ToString();
//         }
//         
//         ChooseBG.Instance.SelectBG(ChooseBG.currentBG);
//         
//         ResetPivot();
//     }
//
//     void ProcessTimer () 
//     {
//         if (timer > 0)
//             if (timerTime < Time.time && !gameFinished == false)
//             { // Lose game if time is out
//                 if (loseUI)
//                     loseUI.SetActive(true);
//                 gameFinished = true;
//             }
//             else
//             {
//                 if (timerCounterUI)
//                 {
//                     float minutes_tmp = (int)(Mathf.Abs(Time.time - timerTime) / 60);
//                     float seconds_tmp = (int)(Mathf.Abs(Time.time - timerTime) % 60);
//
//                     seconds_tmp = (seconds_tmp == 60) ? 0 : seconds_tmp;
//
//                     timerCounterUI.text = minutes_tmp.ToString() + ":" + seconds_tmp.ToString("00");
//
//                 }
//             }
//
//     }
//     
//     string GetElapsedTime()
//     {
//         elapsedTime = Mathf.Abs(timer - (timerTime - Time.time));
//
//         return SecondsToTimeString(elapsedTime);
//     }
//     
//     string SecondsToTimeString(float _seconds)
//     {
//         elapsedTime = Mathf.Abs(timer - (timerTime - Time.time));
//
//         float minutes_tmp = (int)(elapsedTime / 60);
//         float hours_tmp = (int)(minutes_tmp / 60);
//         minutes_tmp = (int)(minutes_tmp % 60);
//         float seconds_tmp = (int)(elapsedTime % 60);
//         seconds_tmp = (seconds_tmp == 60) ? 0 : seconds_tmp;
//
//         return hours_tmp.ToString() + ":" + minutes_tmp.ToString() + ":" + seconds_tmp.ToString("00");
//     }
//     
//     // private void LoadData()
//     // {
//     //     Debug.Log("LoadData");
//     //     Debug.Log("Current Level " + curLevel);
//     //     remainingHints = Utils.hintCount;
//     //     curLevel = Utils.levelIndex;
//     //     if (curLevel > levelCtl.level.Count)
//     //     {
//     //         Debug.Log(curLevel);
//     //         Utils.levelIndex = 0;
//     //     }
//     //     Debug.Log(remainingHints + " " + curLevel);
//     // }
//
//     // public void SaveData()
//     // {
//     //     Utils.hintCount = remainingHints;
//     //     Utils.levelIndex = curLevel+1;
//     //     if (curLevel > levelCtl.level.Count)
//     //     {
//     //         Debug.Log(curLevel);
//     //         Utils.levelIndex = 0;
//     //     }
//     //     Utils.Save();
//     // }
//     
//     // Show Hint and update remainingHints
//     public void ShowHint () 
//     {
//         Debug.Log("remainingHints: " + remainingHints);
//         if (remainingHints == 0)
//         {
//             // AdsController.ShowRewardedVideo((() =>
//             // {
//             //     remainingHints++;
//             // }), () =>
//             // {
//             //     Debug.Log("noads");
//             // }, () =>
//             // {
//             //     Debug.Log("Close");
//             //     Debug.Log("Remaining Hint: " + remainingHints);
//             // if (hintCounterUI.transform.parent)
//             //     hintCounterUI.transform.parent.GetChild(3).gameObject.SetActive(remainingHints == 0);
//             // hintCounterUI.transform.gameObject.SetActive(remainingHints > 0);
//             // hintCounterUI.transform.parent.GetChild(1).gameObject.SetActive(remainingHints > 0);
//             // hintCounterUI.text = remainingHints.ToString();
//             // }, StringHelper.RewardAds.hintAd);
//             
//             remainingHints++;
//             if (hintCounterUI.transform.parent)
//                 hintCounterUI.transform.parent.GetChild(3).gameObject.SetActive(remainingHints == 0);
//             hintCounterUI.transform.gameObject.SetActive(remainingHints > 0);
//             hintCounterUI.transform.parent.GetChild(1).gameObject.SetActive(remainingHints > 0);
//             hintCounterUI.text = remainingHints.ToString();
//
//
//
//         }
//         if (gameFinished  ||  remainingHints == 0 || blockRaycast.activeSelf)  return;
//         else
//         {
//             SlotCtl.Instance.Done1Piece();
//         }
//
//         remainingHints--;
//         
//         if (hintCounterUI) 
//             hintCounterUI.text = remainingHints.ToString();
//
//     }
//
//     public void SortPuzzle()
//     {
//         if (sortPuzzleUI.transform.GetChild(0).transform.gameObject.activeSelf)
//         {
//             sortPuzzleUI.transform.GetChild(0).gameObject.SetActive(false);
//             sortPuzzleUI.transform.GetChild(1).gameObject.SetActive(true);
//             controller.Sort();
//             controller.InitPieces();
//         }
//         else
//         {
//             sortPuzzleUI.transform.GetChild(0).gameObject.SetActive(true);
//             sortPuzzleUI.transform.GetChild(1).gameObject.SetActive(false);
//             controller.Shuffle();
//             controller.InitPieces();
//         }
//     }
//     
//     
//     public void CheckCompletePiece()
//     {
//         var slots = SlotCtl.Instance.slots;
//         for (var j = 0; j < slots.Count; j++)
//             {
//                 if(slots[j].isDone) continue;
//                 if(controller == null || controller.currentClickScroll == null ) continue;
//                 
//                 if(slots[j].id == controller.currentClickScroll.id)
//                 // Tính khoảng cách giữa GoalsPost với Piece
//                 {
//                     var distance = Vector2.Distance(slots[j].transform.position,
//                         controller.currentClickScroll.transform.position);
//                     // Nếu khoảng cách nhỏ hơn 0.5f thì bật BlockRayCast, cập nhật có thể kéo mảnh tranh trong controller, bật GoalPost
//                     if (distance >= 1f/LevelCtl.Instance.size[curLevel]) continue;
//                     
//                     SlotCtl.Instance.slots[j].SlotDone();
//                     blockRaycast.SetActive(false);
//                     controller.canDrag = true;
//                     // Check khi 1 GoalsPost được điền đúng
//                     CheckThisGoalsPost();
//                     
//                 }
//                 
//             }
//         
//         
//     }
//
//     public void ResetPivot()
//     {
//         SlotCtl slotcontroller = SlotCtl.Instance;
//         List<Slot> slots = SlotCtl.Instance.slots;
//         for (int i = 0; i < slots.Count; i++)
//         {
//             RectTransform rt = slots[i].GetComponent<RectTransform>();
//             int index = 0;
//             
//             
//             var pos = rt.localPosition;        
//             // rt.anchorMin = new Vector2(.5f, 1);
//             // rt.anchorMax = new Vector2(.5f, 1);
//             Vector2 startPivot = rt.pivot;
//             rt.pivot = new Vector2(.5f, .5f);
//             
//             Vector2 startanchor = rt.anchoredPosition;
//             float resY, resX;
//             if (startPivot.x > 0.1f)
//             {
//                 resX = rt.rect.width * (0.5f - startPivot.x);
//             }
//             else
//             {
//                 resX = rt.rect.width * 0.5f;
//             }
//
//             if (startPivot.y < .9f)
//             {
//                 resY = rt.rect.height * (startPivot.y - 0.5f);
//             }
//             else
//             {
//                 resY = rt.rect.height * 0.5f;
//             }
//             rt.localPosition = new Vector2(startanchor.x +resX,  startanchor.y - resY);
//             
//             
//         }
//     }
//     
//     public void CheckThisGoalsPost()
//     {
//         // Xử lý khi 1 mảnh tranh điền vào đúng GoalsPost
//         SlotCtl.Instance.CheckDone();
//         controller.scroll.enabled = true;
//         controller.currentClickScroll = null;
//         // gamePlayControl.SortingLayer(this);
//         
//
//     }
//     
//     
// }
