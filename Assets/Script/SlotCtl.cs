using System;
using System.Collections;
using System.Collections.Generic;
// using System.Net.Mime;
// using DG.Tweening;
using UnityEngine;
// using UnityEngine.UI;
//
public class SlotCtl : MonoBehaviour
{
//     public List<PuzzlePiece> sidePieces;
//     public List<PuzzlePiece> normalPieces;
//     public List<PuzzlePiece> pieces;
     public List<PieceScroll> slots;
     public List<PieceScroll> movedSlots;
     public static SlotCtl Instance;
//     
     private void Awake()
     {
         Instance = this;
     }
//     private void Start()
//     {
//         Debug.Log("SlotCtl Start");
//     }
//
//     public void CheckDone()
//     {
//         print("Checking...");
//         for(int i = 0; i < slots.Count; i++)
//         {
//             if (slots[i].isDone == false)
//             {
//                 GameController.Instance.isWin = false;
//                 return;
//             }
//         }
//
//         for(int i = 0; i < slots.Count; i++)
//         {
//             slots[i].GetComponent<Image>().enabled = false;
//         }
//         GameController.Instance.isWin = true;
//         GameController.Instance.WinPuzzle();
//         Debug.Log("Dang Save day");
//         // GamePlayController.Instance.SaveData();
//     }
//
//     public void Done1Piece()
//     {
//         StartCoroutine(PlayAnim());
//     }
//
//     IEnumerator PlayAnim()
//     {
//         int rand = UnityEngine.Random.Range(0, pieces.Count);
//         pieces[rand].transform.SetParent(GamePlayController.Instance.controller.gameObject.transform);
//         
//         int index = pieces[rand].id;
//         int slotindex = 0;
//         for(int i = 0; i < slots.Count; i++)
//         {
//             if (slots[i].id == index)
//             {
//                 slotindex = i;
//                 break;
//             }
//         }
//
//         pieces[rand].GetComponent<RectTransform>().DOScale(new Vector2(slots[slotindex].thumbnail.rectTransform.sizeDelta.x*0.004f,slots[slotindex].thumbnail.rectTransform.sizeDelta.x*0.004f), 0.1f);
//         Debug.Log("SlotSize: " + slots[rand].thumbnail.rectTransform.sizeDelta);
//         pieces[rand].GetComponent<RectTransform>().DOMove(slots[slotindex].GetComponent<RectTransform>().position, .75f);
//         
//         Debug.Log("PlayAnim");
//         GamePlayController.Instance.blockRaycast.SetActive(true);
//         yield return new WaitForSeconds(.75f); 
//         GamePlayController.Instance.blockRaycast.SetActive(false);
//         
//         GamePlayController.Instance.controller.currentClickScroll = pieces[rand];
//         slots[slotindex].SlotDone();
//         GamePlayController.Instance.CheckThisGoalsPost();
//         Debug.Log("PlayedAnim");
//     }
}
