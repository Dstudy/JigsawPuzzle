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
using Debug = UnityEngine.Debug;

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
        public bool ok = false;

        private Vector3 offset;
//
     private void Awake()
     {
         Instance = this;
         rect = GetComponent<RectTransform>();
         pieceImg = GetComponent<Image>();
         
     }

     private void Start()
     {
         rect = GetComponent<RectTransform>();
         
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
             var ps = clonePiece.GetComponent<PieceScroll>();
             if (ps) Destroy(ps);
             clonePiece.transform.SetSiblingIndex(order);
             clonePiece.GetComponent<Image>().enabled = false;
             Debug.Log("Gen ClonePiece " + gameObject.name);
             startPos = rect.position;

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
         RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, screenPoint, controller.uiCamera, out var localPosition);
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
         controller.scroll.enabled = true;
         fromScroll = true;
         
         if(PuzzleController.Instance.currentObject != null)
         {
             // PuzzleController.Instance.currentObject.transform.position = new Vector3(100, 100, 0);
             // PuzzleController.Instance.currentObject = null;
         }
     }

     private void Update()
     {
         // if (!isDragging && gameObject.activeInHierarchy)
         // {
         //     transform.localPosition = new Vector3(transform.localPosition.x, -26f, transform.localPosition.z);
         // }
         
         if (controller.canDrag && isDragging)
         {
             // var wordPos = GetMousePos(tempPos);
             Vector2 screenPoint = Input.mousePosition ;
             RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, screenPoint, controller.uiCamera, out var localPosition);
             var wordPos = Camera.main.ScreenToWorldPoint(screenPoint);
             wordPos.x -= PuzzleController.Instance.pieces[0].renderer.bounds.size.x*(LevelCtl.currentSize/15f);
             wordPos.y += PuzzleController.Instance.pieces[0].renderer.bounds.size.y * (LevelCtl.currentSize/12f);
             wordPos.z = controller.zCor;
             Vector3 pos = Input.mousePosition;
             // Di chuyển Rect Transform của mảnh tranh đến localPosition (Vị trí của chuột)
             rect.position = Vector3.Lerp(rect.position, wordPos, 1000 * Time.deltaTime);
             
             if (controller.outScroll && !isOut)
             {
                 controller.scroll.enabled = false;
                 controller.contentSizeFitter.enabled = false;
                 Debug.Log("OutToScroll");
                 var dragPiece = PuzzleController.Instance.pieces[id];
                 
                 if(firstTime)
                 {
                     PuzzleController.Instance.pieces[id].transform.position = 
                         new Vector3(wordPos.x , wordPos.y + dragPiece.size.y * 0.5f, -1);
                     if(PuzzleController.Instance.pieces[id].transform.childCount > 0)
                         PuzzleController.Instance.pieces[id].transform.GetChild(0).gameObject.SetActive(true);
                     firstTime = false;
                 }
                 PuzzleController.Instance.pieces[id].transform.localScale = Vector3.one;
                 
                 isOut = true;
                 isDragging = false;
                 transform.SetParent(controller.parentDrag);
                 transform.GetComponent<Image>().enabled = false;
                 if (fromScroll)
                 {
                     Debug.Log("Out anim");
                     StartCoroutine(PieceOutAnim());
                 }

                 ok = false;
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

                 var parent = (gameObject.transform as RectTransform);
                 var sprite = piece.transform.GetComponent<SpriteRenderer>().sprite;
                 
                 if(!ok)
                 {
                     piece.transform.DOScale(
                         parent.rect.size / sprite.rect.size *
                         controller.canvas.GetComponent<RectTransform>().localScale *
                         sprite.pixelsPerUnit, 0.2f);
                     ok = true;
                 }
                 
                 controller.canDrag = true;
                 isDragging = true;
                 isOut = false;
             }
         }
     }
     
     IEnumerator PieceOutAnim()
     {
         if(clonePiece == null)
            clonePiece = Instantiate(this.gameObject, this.gameObject.transform.position, Quaternion.identity, controller.parentElement);
         clonePiece.transform.SetSiblingIndex(order);
         if(clonePiece.GetComponent<Image>() == null)
             Debug.Log("ClonePiece Image null");
         clonePiece.GetComponent<Image>().color = new Color(0, 0, 0, 0);
         if(clonePiece.GetComponent<RectTransform>() == null)
                Debug.Log("ClonePiece RectTransform null");
         clonePiece.GetComponent<RectTransform>().DOSizeDelta(new Vector2(-controller.hlg.spacing, clonePiece.GetComponent<RectTransform>().sizeDelta.y), .3f);
         yield return new WaitForSeconds(.3f);
         clonePiece.transform.DOKill(true);
         Destroy(clonePiece);
     }
     
     IEnumerator PieceInAnim()
     {
         controller.scroll.enabled = false;
         if(PuzzleController.Instance.pieces[id].transform.childCount > 0)
             PuzzleController.Instance.pieces[id].transform.GetChild(0).gameObject.SetActive(false);
         
         clonePiece = Instantiate(this.gameObject, this.gameObject.transform.position, Quaternion.identity, controller.parentElement);
         clonePiece.GetComponent<RectTransform>().sizeDelta = new Vector2(-controller.hlg.spacing,
             clonePiece.GetComponent<RectTransform>().sizeDelta.y);
         Destroy(clonePiece.GetComponent<PieceScroll>());
         clonePiece.transform.SetSiblingIndex(order);
         if(clonePiece.GetComponent<Image>().enabled == false)
             clonePiece.GetComponent<Image>().enabled = true;
         clonePiece.GetComponent<Image>().color = new Color(0, 0, 0, 0);
         
         // Instantiate(GameController.Instance.dot2, clonePiece.GetComponent<RectTransform>().position, Quaternion.identity, controller.parentDrag);

         yield return new WaitForSeconds(0.00001f);
         
         // Debug.Log("Piece localPos: " + transform.position + " ClonePiece Pos: " + clonePiece.transform.position);
         PuzzleController.Instance.pieces[id].transform.DOMove(clonePiece.transform.position, .3f).SetEase(Ease.InOutCubic);
         
         clonePiece.GetComponent<RectTransform>().DOSizeDelta(rect.sizeDelta, .3f).SetEase(Ease.InOutCubic);
         
         yield return new WaitForSeconds(.3f);
         
         // clonePiece.transform.DOKill(true);
         Destroy(clonePiece);
         
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
        yield return new WaitForSeconds(2f);
         controller.contentSizeFitter.enabled = true;
         controller.scroll.enabled  = true;
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
            
            controller.currentClickScroll = this;
    }

    void UpdateScale(float pixelHeightOnCanvas)
    {
        SpriteRenderer _sprite = PuzzleController.Instance.pieces[id].transform.GetComponent<SpriteRenderer>();
        CanvasScaler matchCanvas = GetComponentInParent<CanvasScaler>();
        // The canvas will try to scale its reference resolution
        // to match the screen's dimensions in either x or y.
        // (Assuming it's in Overlay mode or using a fullscreen camera
        // - if rendering to a smaller rect, use that pixel rect instead)
        Vector2 scaleFactorRange = new Vector2(
            Screen.width / matchCanvas.referenceResolution.x,
            Screen.height / matchCanvas.referenceResolution.y);

        // When the screen's aspect ratio isn't the same as the reference,
        // the canvas picks between two scale factors with matchWidthOrHeight
        float scaleFactor = Mathf.Lerp(
            scaleFactorRange.x,
            scaleFactorRange.y,
            matchCanvas.matchWidthOrHeight);

        // We can now compute how much it will scale our in-canvas
        // dimensions to produce on-screen pixel dimensions.
        float heightInScreenPixels = pixelHeightOnCanvas * scaleFactor;

        // For the next part, we need to know what camera we're
        // being rendered by - consider caching this if it's constant.
        Camera cam = Camera.main;

        // We'll convert the screen height into a fraction of the camera's
        // vertical span (which might be less than the screen's if rendering
        // to a smaller viewport rect).
        float heightAsViewFraction = heightInScreenPixels / cam.pixelRect.height;

        // Now we can convert that to a desired world height by multiplying
        // by the camera's vertical size - note that orthographicSize is
        // only half the height of the camera's view, hence the 2x.    
        float heightInWorldUnits = 2f * cam.orthographicSize * heightAsViewFraction;

        // Lastly, we need to know how big "this" sprite is at scale = 1.
        float nativeWorldHeight = _sprite.sprite.rect.height / _sprite.sprite.pixelsPerUnit;

        // And our scale factor is the multiplier that gets us from our
        // native world size to the desired world size.
        PuzzleController.Instance.pieces[id].transform.localScale = Vector3.one * heightInWorldUnits / nativeWorldHeight;
    }
    
    
    
}