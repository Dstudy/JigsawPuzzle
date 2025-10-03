using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class LevelCtl : MonoBehaviour
{
//     public List<List<PuzzlePiece>> listPieces = new List<List<PuzzlePiece>>();
     [NonSerialized]public List<PuzzleController> puzzles;
     [NonSerialized]public List<GameObject> level;
     public GameObject background;
     public PieceScroll prefabPiece;
     public SlotCtl slotCtl;
     public static LevelCtl Instance;

     public static PuzzleAnchor anchor;
     public Transform canvas;
     public Transform content;
     public Transform scrollView;
     public int pieceNum;
     [NonSerialized]public List<int> size;
     public static int currentSize;
     public float shrink;

     public float minHeight = 10000;
     public float maxHeight = 0;
     public HScrollController controller;
     
     private void Awake()
     {
         if(Instance == null)
             Instance = this;
         
         level = LevelData.Instance.level;
         puzzles = LevelData.Instance.puzzles;
         size = LevelData.Instance.size;
     }

     public void InitLevel(int index)
     {
        currentSize = size[index];
         Debug.Log("Level Index: " + index);
         PuzzleController currentPuzzle = null;
         if(puzzles[index] == null)
             Debug.Log("Puzzle null");
         currentPuzzle = Instantiate(puzzles[index], new Vector3(0, 0, 0), Quaternion.identity);
         currentPuzzle.name = puzzles[index].name;
         
         for(int i =0; i < currentPuzzle.transform.childCount; i++)
         {
             currentPuzzle.transform.GetChild(i).GetComponent<SpriteRenderer>().sortingOrder = 1;
         }
         
         // puzzles[index].Prepare();
         currentPuzzle.SetAnchor(PuzzleAnchor.Center);
         currentPuzzle.Prepare();
         
         
         if (index >= level.Count)
         {
             index = 0;
         }
         float screenScale = canvas.GetComponent<RectTransform>().localScale.x;
         GameObject backGround = Instantiate(background, new Vector3(0, 0, 0), Quaternion.identity);
          GameObject curlevel = Instantiate(level[index], new Vector3(100, 100, 100), Quaternion.identity, canvas);
          
          if(GenPuzzle.Instance.levelImage == null)
              Debug.Log("Level Image null");
          backGround.GetComponent<SpriteRenderer>().material.color = new Color(1, 1, 1, 1);
          backGround.GetComponent<SpriteRenderer>().sprite = GenPuzzle.Instance.levelImage[index];
          
         slotCtl = curlevel.GetComponent<SlotCtl>();
         slotCtl.slots.Clear();

         var slots = slotCtl.slots;

         scrollView.transform.SetAsLastSibling();
         pieceNum = level[index].transform.childCount;

         shrink = size[index] * 0.044f;
        
         currentPuzzle.LoadProgress(currentPuzzle.name);
         int countAssembled = 0;

         foreach (var piece in currentPuzzle.pieceAssembledIds)
         {
             Debug.Log(piece + " ");
         }
         
         
         for (int i = 0; i < pieceNum; i++)
         {
             slots.Add(level[index].transform.GetChild(i).gameObject.GetComponent<PieceScroll>());
             var sizeNew = slots[i].pieceImg.rectTransform.sizeDelta;
             if((sizeNew*shrink).y > maxHeight)
                 maxHeight = (sizeNew*shrink).y;
             if((sizeNew*shrink).y < minHeight)
                 minHeight = (sizeNew*shrink).y;
         }

         for (int i = 0; i < pieceNum; i++)
         {
             var piece = SlotCtl.Instance.slots;
             var PiecePre = Instantiate(prefabPiece, Vector3.zero, Quaternion.identity, content);
             PiecePre.id = i;
             PiecePre.transform.name = "Piece " + i;
             PiecePre.GetComponent<Image>().sprite = slots[i].GetComponent<Image>().sprite;
             PiecePre.Init(controller);
             if(PiecePre == null)
                 print("PiecePre null");
             PiecePre.GetComponent<Image>().material = slots[i].GetComponent<Image>().material;
             PiecePre.GetComponent<RectTransform>().pivot = piece[i].GetComponent<RectTransform>().pivot;
             if (PiecePre == null) 
                    print("PiecePre null");
             PiecePre.pieceImg.SetNativeSize();
             // PiecePre.pieceImg.rectTransform.sizeDelta = sizeNew*shrink;
             PiecePre.rect.anchoredPosition = Vector3.zero;
             PiecePre.rect.sizeDelta *= shrink;
            PiecePre.rect.localPosition = new Vector3(PiecePre.rect.localPosition.x, PiecePre.rect.localPosition.y, 0);
            
             float yCor;
             if (piece[i].state == "up")
             {
                 // Debug.Log("Piece " + i + " is up");
                 yCor = -(maxHeight + piece[i].GetComponent<RectTransform>().rect.height) / 2;
                 // Debug.Log("Piece " + i + " yCor: " + yCor);
             }
             else if(piece[i].state == "down")
                 yCor = -(maxHeight - piece[i].GetComponent<RectTransform>().rect.height)/2;
             else
                 yCor = 0;
             PiecePre.state = piece[i].state;
             PiecePre.startPos = new Vector3(0, yCor, 0);
             PiecePre.GetComponent<RectTransform>().localPosition = new Vector3(0,yCor, 0);
         }

         for (int i = 0; i < curlevel.transform.childCount; i++)
         {
             Destroy(curlevel.transform.GetChild(i).gameObject);
         }

         foreach (var pieceID in currentPuzzle.pieceAssembledIds)
         {
             // if(piece >= 0 && piece < SlotCtl.Instance.slots.Count)
             // {
             GameController.Instance.controller.RemovePiece(pieceID);
             // }
         }
     }
//     
//     
//     
}
