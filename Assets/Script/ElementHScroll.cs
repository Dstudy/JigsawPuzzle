// using System;
// using UnityEngine;
// using UnityEngine.EventSystems;
// using DG.Tweening;
// public class ElementHScroll : MonoBehaviour, IPointerDownHandler
// {
//     private HScrollController controller;
//     private bool isCanDrag;
//     private bool isDragging;
//     public RectTransform draggedItemRect;
//     
//     public void Init(HScrollController Controller)
//     {
//         this.controller = Controller;
//     }
//     
//
//     public void OnPointerDown(PointerEventData eventData)
//     {
//         //controller.currentClickScroll = this;
//         Debug.Log(this.GetComponent<PuzzlePiece>().id);
//     }
//
//     public void ActiveDrag(bool isActive)
//     {
//         if (isActive)
//         {
//             this.transform.parent = this.controller.parentDrag;
//             isCanDrag = true;
//             isDragging = true;
//             this.transform.DOKill();
//             this.transform.DOScale(1.3f, 0.25f);
//         }
//     }
//
//     public void ReturnScroll()
//     {
//         this.transform.parent = this.controller.parentElement;
//     }
//
//     private void Update()
//     {
//         if(isDragging)
//         {
//             Vector2 sceenPoint = Input.mousePosition + 300 * Vector3.up;
//             RectTransformUtility.ScreenPointToLocalPointInRectangle(draggedItemRect,sceenPoint, Camera.main, out var localPosition);
//             draggedItemRect.position = Vector3.Lerp(draggedItemRect.position, draggedItemRect.TransformPoint(localPosition), 10 * Time.deltaTime);
//         }
//
//         if (!Input.GetMouseButtonUp(0)) return;
//         if (!isCanDrag) return;
//         isDragging = false;
//         isCanDrag = false;
//         ReturnScroll();
//
//         controller.scroll.enabled = true;
//         this.transform.DOKill();
//         this.transform.DOScale(1f, 0.25f);
//     }
//
//     public Vector3 GetPointDistanceFromObject(float distance, Vector3 direction, Vector3 fromPoint)
//     {
//         distance -= 1;
//         var finalDirection = direction + direction.normalized * distance;
//         var targetPosition = fromPoint + finalDirection;
//         return targetPosition;
//     }
//
// }