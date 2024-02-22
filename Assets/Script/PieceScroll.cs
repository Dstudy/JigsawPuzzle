// using System;
// using System.Collections;
// using System.Collections.Generic;

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
// using UnityEngine.Assertions.Must;
// using UnityEngine.EventSystems;
using UnityEngine.UI;
//
public class PieceScroll : MonoBehaviour, IPointerDownHandler
{
//     public bool isDone = false;
        public RectTransform rect;
        public int id;
        public Image pieceImg;
        public string state;
        public bool isDragging;
        public HScrollController controller;
        [SerializeField]public bool canBeDragged;
        
        public float yCor = -25;
        public int order;
        public bool isSide;

        public Vector3 startSize;
        public Vector2 startScale;
        public Vector3 startPos;
        Vector2 oldPointerPosition;

        
        public static PieceScroll Instance;
        public GameObject clonePiece;
        public bool isOut = false;
        public bool fromScroll = true;
        public bool firstTime = true;

        private float fixedW = 90.78f, fixedH = 72.6f;
//
     private void Awake()
     {
         Instance = this;
         rect = GetComponent<RectTransform>();
         pieceImg = GetComponent<Image>();
         
     }

     private void Start()
     {
         
     }

     public void Init(HScrollController Controller)
     {
         controller = Controller;
     }
     public void ActivePieceDrag()
     {
         Debug.Log(rect.anchoredPosition);
         if (this.controller.canDrag)
         {
             startSize = rect.sizeDelta;
             startScale = rect.localScale;
             order = this.gameObject.transform.GetSiblingIndex();
             Debug.Log("Active Piec Drag at " + order);
             
             clonePiece = Instantiate(this.gameObject, this.gameObject.transform.position, Quaternion.identity, controller.parentElement);
             clonePiece.transform.SetSiblingIndex(order);
             clonePiece.GetComponent<Image>().enabled = false;
             startPos = rect.position;
             Debug.Log("ActivePieceDrag");
             // Set lại parent cho mảnh tranh, cập nhật mảnh tranh có thể được kéo và đang được kéo 
             // this.transform.SetParent(this.controller.parentDrag);
             transform.SetParent(controller.parentDrag);
             isDragging = true;
             canBeDragged = true;
         }
     }

     public void ReturnPiece()
     {
         PuzzleController.Instance.pieces[id].transform.position = new Vector3(100, 100, 100);
     }

     public void ReturnToScroll()
     {
         // if (transform.parent == controller.parentElement)
         //     return;
         firstTime = true;
         if(clonePiece != null)
            Destroy(clonePiece);
         
         Vector2 screenPoint = Input.mousePosition ;
         RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, screenPoint, controller.camera, out var localPosition);
         var wordPos = Camera.main.ScreenToWorldPoint(screenPoint);
         wordPos.y += PuzzleController.Instance.pieces[0].renderer.bounds.size.y*0.5f;
         wordPos.z = controller.zCor;
         order = GetSiblingIndexFromPosition(wordPos);
         
         Debug.Log(controller.posList[order] + " " + order);
         
         isDragging = false;
         rect.localScale = startScale;
         rect.sizeDelta = startSize;
         isOut = false;
         if(!fromScroll)
            StartCoroutine(PieceInAnim());
         else
         {
                transform.SetParent(controller.parentElement);
                transform.SetSiblingIndex(order);
                rect.localPosition = new Vector3(controller.posList[order], startPos.y, startPos.z);
                transform.GetComponent<Image>().enabled = true;
                ReturnPiece();
         }
         controller.contentSizeFitter.enabled = true;
         fromScroll = true;
         
         if(PuzzleController.Instance.currentObject != null)
         {
             // PuzzleController.Instance.currentObject.transform.position = new Vector3(100, 100, 0);
             // PuzzleController.Instance.currentObject = null;
         }
         
     }

     private void Update()
     {
         // var wordPos = GetMousePos(tempPos);
         Vector2 screenPoint = Input.mousePosition ;
         RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, screenPoint, controller.camera, out var localPosition);
         var wordPos = Camera.main.ScreenToWorldPoint(screenPoint);
         wordPos.y += PuzzleController.Instance.pieces[0].renderer.bounds.size.y*0.5f;
         wordPos.z = controller.zCor;
         if (controller.canDrag && isDragging)
         {
             // Di chuyển Rect Transform của mảnh tranh đến localPosition (Vị trí của chuột)
             rect.position = Vector3.Lerp(rect.position, wordPos, 1000 * Time.deltaTime);
             
             if (controller.outScroll && !isOut)
             {
                 controller.contentSizeFitter.enabled = false;
                 Debug.Log("OutToScroll");
                 if(clonePiece != null)
                    Destroy(clonePiece);
                 var dragPiece = PuzzleController.Instance.pieces[id];
                 
                 if(firstTime)
                 {
                     PuzzleController.Instance.pieces[id].transform.position =
                         new Vector3(wordPos.x - dragPiece.size.x * 0.5f, wordPos.y + dragPiece.size.y * 0.5f, -1);
                     if(PuzzleController.Instance.pieces[id].transform.childCount > 0)
                         PuzzleController.Instance.pieces[id].transform.GetChild(0).gameObject.SetActive(true);
                     firstTime = false;
                 }
                 PuzzleController.Instance.pieces[id].transform.localScale = Vector3.one;
                 
                 isOut = true;
                 isDragging = false;
                 transform.SetParent(controller.parentDrag);
                 transform.GetComponent<Image>().enabled = false;
                 if(fromScroll)
                    StartCoroutine(PieceOutAnim());
                 
                 // PuzzleController.Instance.currentPiece = id;
             }
         }

         if (controller.contentSizeFitter.enabled == false)
             StartCoroutine(AdjustScroll());

         if (Input.GetMouseButtonUp(0) && !isOut && isDragging)
         {
             isDragging = false;
             Debug.Log("ReturnAll");
             Debug.Log(transform.position);
             ReturnToScroll();
         }

         if (PuzzleController.Instance.currentPiece == this.id && PuzzleController.Instance.currentGroup == null)
         {
             PuzzlePiece piece = PuzzleController.Instance.pieces[id];
             if(!controller.outScroll)
             {
                 fromScroll = false;

                 var scale = piece.transform.GetComponent<Renderer>().bounds.size;
                 StartCoroutine(
                     PieceToScroll(scale));
                 
                 // Debug.Log("Image width and height" + GetComponent<Image>().sprite.bounds.size + "Piece Size" + piece.size + "Piece Scale" + GetComponent<Image>().sprite.bounds.size.x / piece.renderer.bounds.size.x);
                 piece.transform.localScale = Vector3.one * (GetComponent<Image>().sprite.bounds.size.x / piece.renderer.bounds.size.x);
                 transform.SetParent(controller.parentDrag);
                 
                 // Vector3 localPos = controller.parentElement.InverseTransformPoint(wordPos);
                 // rect.localPosition = localPos;
                 controller.canDrag = true;
                 isDragging = true;
                 isOut = false;
                 // puzzleCtl.currentPiece = -1;
             }
         }
     }
     
     IEnumerator PieceOutAnim()
     {
         clonePiece = Instantiate(this.gameObject, this.gameObject.transform.position, Quaternion.identity, controller.parentElement);
         clonePiece.transform.SetSiblingIndex(order);
         clonePiece.GetComponent<Image>().color = new Color(0, 0, 0, 0);
         clonePiece.transform.DOScaleX(0, 0.2f).SetEase(Ease.InOutCubic);
         yield return new WaitForSeconds(0.2f);
         Destroy(clonePiece);
         clonePiece.transform.DOKill(true);
     }
     
     IEnumerator PieceInAnim()
     {
         controller.scroll.enabled = false;
         if(PuzzleController.Instance.pieces[id].transform.childCount > 0)
             PuzzleController.Instance.pieces[id].transform.GetChild(0).gameObject.SetActive(false);
         
         clonePiece = Instantiate(this.gameObject, this.gameObject.transform.position, Quaternion.identity, controller.parentElement);
         clonePiece.GetComponent<RectTransform>().localScale = new Vector3(0, 1, 1);
         Destroy(clonePiece.GetComponent<PieceScroll>());
         clonePiece.transform.SetSiblingIndex(order);
         if(clonePiece.GetComponent<Image>().enabled == false)
             clonePiece.GetComponent<Image>().enabled = true;
         clonePiece.GetComponent<Image>().color = new Color(0, 0, 0, 0);
         clonePiece.transform.DOScaleX(1, .3f).SetEase(Ease.InOutCubic);
         Vector3 tempPos = clonePiece.transform.TransformPoint(Vector3.zero);
         // Instantiate(GameController.Instance.dot2, clonePiece.GetComponent<RectTransform>().position, Quaternion.identity, controller.parentDrag);

         yield return new WaitForSeconds(0.00001f);
         
         // Debug.Log("Piece localPos: " + transform.position + " ClonePiece Pos: " + clonePiece.transform.position);
         PuzzleController.Instance.pieces[id].transform.DOMove(clonePiece.transform.position, .3f).SetEase(Ease.InOutCubic);
         
         yield return new WaitForSeconds(.3f);
         
         Destroy(clonePiece);
         clonePiece.transform.DOKill(true);
         
         Debug.Log(order);
         transform.SetParent(controller.parentElement);
         transform.SetSiblingIndex(order);
         rect.localPosition = new Vector3(controller.posList[order], startPos.y, startPos.z);
         transform.GetComponent<Image>().enabled = true;
         ReturnPiece();
         controller.scroll.enabled = true;
     }
     
     
     IEnumerator AdjustScroll()
     {
        yield return new WaitForSeconds(0.01f);
         controller.contentSizeFitter.enabled = true;
     }

     IEnumerator PieceToScroll(Vector3 scale)
     {
         // float canvasScale = controller.transform.parent.GetComponent<RectTransform>().localScale.x;
         // rect.localScale = Vector3.one;
         // rect.sizeDelta = new Vector2(scale.x/canvasScale, scale.y/canvasScale);
         
         yield return new WaitForSeconds(0.01f);
         
         // Debug.Log("Scale " + scale + " canvasScale " + canvasScale);
         // rect.DOSizeDelta(startSize, 0.4f);
         // rect.DOScale(startScale, 0.4f);
         
         yield return new WaitForSeconds(0.4f);
         
         // rect.localPosition = new Vector3(rect.localPosition.x, rect.localPosition.y, -1);
     }
     
     private int GetSiblingIndexFromPosition(Vector3 position)
     {
         for (int i = 0; i < controller.scroll.content.childCount; i++)
         {
             Transform child = controller.scroll.content.GetChild(i);
             if (child != null && child.position.x > position.x)
             {
                 return i;
             }
         }
         return controller.scroll.content.childCount;
     }
     
    public void OnPointerDown(PointerEventData eventData)
    {
            Debug.Log("Update PieceScroll " + order);
            rect = GetComponent<RectTransform>();
            controller.currentClickScroll = this;
    }

    
}