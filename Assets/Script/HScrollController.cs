using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Serialization;

//
public class HScrollController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
     /*
      scroll: Vùng cuộn chứa các mảnh tranh
      currentClickScroll: Lưu mảnh tranh đang được kéo
      parentElement: Transform của GameObject làm cha của các mảnh tranh đang ở trong vùng cuộn Scroll
      parentDrag: Transform của GameObject làm cha của các mảnh tranh khi chúng được kéo 
      camera: Camera để chuyển đổi vị trí toạ độ trên màn hình thành vị trí cục bộ trên UI
      canDrag: Kiểm tra xem có được phép kéo các mảnh tranh không 
      startPoint: Vị trí của chuột hoặc ngón tay trên màn hình khi bă đầu kéo mảnh tranh
     */
     public Canvas canvas;
     public ScrollRect scroll;
     public PieceScroll currentClickScroll;
     public RectTransform parentElement;
     public Transform parentDrag;
     public Camera uiCamera;
     public bool canDrag;
     [SerializeField] private Vector2 startPoint;
//     
     public HorizontalLayoutGroup hlg;
     public ContentSizeFitter contentSizeFitter;

     public List<float> posList = new List<float>();
     public List<float> yPosList = new List<float>();
     public float zCor;
     
     public List<int> arrangeList = new List<int>();
     
     private Ray ray;
     private RaycastHit hit;

     public bool outScroll = true;
     private bool dragActivated;

     // private bool ok = true;
     GameObject tempObject;
     private const float DRAG_SQR_THRESHOLD = 700f;
     private const float ANGLE_THRESHOLD = 60f;
     
     bool _layoutDirty;


    private void Start()
    {
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(Vector3.zero);
        
        StartCoroutine(LateStart(0.1f));
        InitPiece();
        if(Camera.main.orthographicSize * Screen.width/Screen.height < 9f)
        {
            Camera.main.orthographicSize += (9f - Camera.main.orthographicSize * Screen.width / Screen.height);
        }

        PuzzleController.Instance.onPieceAssembled += RemovePiece;
    }

    private void LateUpdate()
    {
        if (!_layoutDirty) return;
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(parentElement);
        _layoutDirty = false;
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //Your Function You Want to Call
        InitPos();
    }

    public void InitPiece()
    {
        var size = LevelCtl.currentSize;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (i == 0 || j == 0 || i == size - 1 || j == size - 1)
                {
                    parentElement.GetChild(i*size + j).GetComponent<PieceScroll>().isSide = true;
                }
                else
                {
                    parentElement.GetChild(i*size + j).GetComponent<PieceScroll>().isSide = false;
                }
            }
        }
    }
    
     // Khi bắt đầu kéo mảnh tranh
     public void OnBeginDrag(PointerEventData eventData)
     {
         // Gán startPoint = vị trí của chuột hoặc ngón tay trên màn hình
         startPoint = eventData.position;
         dragActivated = false;
         
         FreezeScroll(); 
     }
//     
     // Khi thả không kéo mảnh tranh nữa
     public void OnEndDrag(PointerEventData eventData)
     {
         dragActivated = false;   
         UnfreezeScroll();
     }
//     
//     // Khi đang kéo mảnh tranh
     public void OnDrag(PointerEventData eventData)
     {
         if (dragActivated) return;  
         if (!canDrag || currentClickScroll == null) return; 

         var directionDrag = eventData.position - startPoint;
         if (directionDrag.sqrMagnitude < DRAG_SQR_THRESHOLD) return;

         var angleDrag = Vector2.Angle(directionDrag.normalized, Vector2.up);
         if (angleDrag >= ANGLE_THRESHOLD) return;

         dragActivated = true;

         if (!currentClickScroll.isDragging)
             currentClickScroll.ActivePieceDrag();

         scroll.enabled = false;
     }
     

     public void InitPos()
     {
         posList.Clear();
         zCor =  parentElement.GetChild(0).GetComponent<RectTransform>().position.z;
         for(int i = 0; i < parentElement.childCount; i++)
         {
             RectTransform child = parentElement.GetChild(i).GetComponent<RectTransform>();
             parentElement.GetChild(i).GetComponent<PieceScroll>().order = i;
             posList.Add(child.localPosition.x);
             // parentElement.GetChild(i).GetComponent<RectTransform>().localPosition = new Vector3(posList[i], 0, zCor);
             yPosList.Add(child.localPosition.y);
             // Debug.Log(child.localPosition);
         }
     }
     
     public void ArrangePos()
     {
         arrangeList.Clear();
         for(int i = 0; i < parentElement.childCount; i++)
         {
             if (parentElement.GetChild(i).GetComponent<PieceScroll>().isSide)
             {
                arrangeList.Add(i);
             }
         }
        for(int i = 0; i < parentElement.childCount; i++)
        {
            if (!parentElement.GetChild(i).GetComponent<PieceScroll>().isSide)
            {
                arrangeList.Add(i);
            }
        }
        
        int index = 0;
        
        
        for(int i = 0; i < parentElement.childCount; i++)
        {
            if (index == arrangeList[i] && parentElement.GetChild(i).GetComponent<PieceScroll>().isSide)
            {
                index++;
                continue;
            }
            if (parentElement.GetChild(i).GetComponent<PieceScroll>().isSide)
            {
                parentElement.GetChild(i).SetSiblingIndex(index);
            }
            else
            {
                continue;
            }
        }
        
        if(tempObject != null)
            Destroy(tempObject);
        tempObject = Instantiate(LevelCtl.Instance.prefabPiece.transform.gameObject, Vector3.zero, Quaternion.identity, parentElement);
        Destroy(tempObject.GetComponent<PieceScroll>());
        tempObject.transform.SetSiblingIndex(0);
        tempObject.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width,1);
     }

     public IEnumerator Arrange()
     {
         FreezeScroll();
         GameController.Instance.blockRaycast.SetActive(true);
         int n = Mathf.Min(10, parentElement.childCount);
         int safeIndex = Mathf.Min(10, posList.Count - 1);
         float targetX = posList.Count > 0 ? posList[safeIndex] : 0f;
         for (int i = 0; i < n; i++)
         {
             parentElement.GetChild(i).transform.DOLocalMove(new Vector3(targetX, parentElement.GetChild(i).localPosition.y, parentElement.GetChild(i).localPosition.z), 0.4f);
         }
         yield return new WaitForSeconds(0.4f);
         ArrangePos();
         tempObject.transform.DOScaleX(0, 0.1f);
         contentSizeFitter.enabled = false;
         yield return new WaitForSeconds(0.1f);
         tempObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0,0);
         Destroy(tempObject);
         contentSizeFitter.enabled = true;
         // ReArrange();
        GameController.Instance.blockRaycast.SetActive(false);
        UnfreezeScroll();
     }

     void SlotOut()
     {
         for(int i = 0; i < parentElement.childCount; i++)
         {
             if (parentElement.GetChild(i).GetComponent<PieceScroll>().id == currentClickScroll.id)
             {
                 SlotCtl.Instance.slots.RemoveAt(i);
             }
         }
     }
     
     public void RemovePiece(int id)
     {
            for(int i = 0; i < parentElement.childCount; i++)
            {
                if (parentElement.GetChild(i).GetComponent<PieceScroll>().id == id)
                {
                    SlotCtl.Instance.slots.RemoveAt(i);
                    Destroy(parentElement.GetChild(i).gameObject);
                    _layoutDirty = true;
                }
            }
     }

     void ReturnSlot()
     {
         if (!currentClickScroll.canBeDragged)
             return;
         currentClickScroll.pieceImg.enabled = true;
         currentClickScroll.ReturnPiece();
     }

    void CheckDrag()
    {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Draw ray
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
            if(Physics.Raycast(ray, out hit))
            {
                if (hit.collider.name == "ScrollArea" || hit.collider.name == "Scroll View")
                {
                    outScroll = false;
                }
                else /*if(hit.collider.name == "PlayArea") */
                {
                    outScroll = true;
                }
            }

    }
    
    
     // Nếu người chơi thả chuột khỏi manh tranh dang kéo thì set currentClickScroll thành null (Không có mảnh tranh nào đang được kéo)
     private void Update()
     {
         if(Input.GetMouseButton(0))
            CheckDrag();
         
         if (Input.GetMouseButtonUp(0) && currentClickScroll != null && currentClickScroll.canBeDragged)
         {
             Debug.Log("Realeasing...");
             if (outScroll)
             {
                 Debug.Log("Bay ra ngoai");
                 SlotOut();
                 outScroll = true;
             }
             scroll.enabled = true;
             //isScroll = false;
             if(currentClickScroll != null )
             {
                 currentClickScroll = null;
             }
         }
     }
     
     public void ShufflePuzzle()
     {
         List<int> list = new List<int>();
            for (int i = 0; i < parentElement.childCount; i++)
            {
                list.Add(i);
            }
         for (int i = 0; i < list.Count; i++)
         {
             int temp = list[i];
             int randomIndex = UnityEngine.Random.Range(i, list.Count);
             list[i] = list[randomIndex];
             list[randomIndex] = temp;
         }
         for(int i = 0; i < list.Count; i++)
         {
             parentElement.GetChild(i).SetSiblingIndex(list[i]);
         }
         if(tempObject != null)
             Destroy(tempObject);
         tempObject = Instantiate(LevelCtl.Instance.prefabPiece.transform.gameObject, Vector3.zero, Quaternion.identity, parentElement);
         Destroy(tempObject.GetComponent<PieceScroll>());
         tempObject.transform.SetSiblingIndex(0);
         tempObject.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width,1);
     }
     
     public IEnumerator Shuffle()
     {
         FreezeScroll();
         GameController.Instance.blockRaycast.SetActive(true);
         int n = Mathf.Min(10, parentElement.childCount);
         for (int i = 0; i < n; i++)
         {
             parentElement.GetChild(i).transform.DOLocalMove(new Vector3(posList[10], parentElement.GetChild(i).localPosition.y, parentElement.GetChild(i).localPosition.z), 0.4f);
         }
         yield return new WaitForSeconds(0.4f);
         ShufflePuzzle();
         tempObject.transform.DOScaleX(0, 0.1f);
         contentSizeFitter.enabled = false;
         yield return new WaitForSeconds(0.1f);
         tempObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0,0);
         Destroy(tempObject);
         contentSizeFitter.enabled = true;
         GameController.Instance.blockRaycast.SetActive(false);
        UnfreezeScroll();
     }
     
     void FreezeScroll()
     {
         if (!scroll) return;
         scroll.StopMovement();
         scroll.velocity = Vector2.zero;
         scroll.inertia = false;         // tắt tạm
     }

     void UnfreezeScroll(bool inertia = true)
     {
         if (!scroll) return;
         scroll.inertia = inertia;       // bật lại nếu muốn
     }
     
}
