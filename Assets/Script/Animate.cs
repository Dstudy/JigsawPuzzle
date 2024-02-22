// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.EventSystems;
// using DG.Tweening;
// using UnityEngine.UIElements;
// using Image = UnityEngine.UI.Image;
//
// public class Animate : MonoBehaviour
// {
//     public GameObject circle1;
//     public static Animate Instance;
//     public GameObject particle;
//     private GamePlayController _gamePlayController;
//     private void Awake()
//     {
//         if (Instance == null)
//             Instance = this;
//         _gamePlayController = GetComponent<GamePlayController>();
//     }
//
//     private void Update()
//     {
//         if (Input.GetMouseButtonDown(0))
//         {
//             if (_gamePlayController.controller.currentClickScroll != null)
//             {
//                 var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//                 Debug.Log("Runnig");
//                 StartCoroutine(TouchAnim(mousePos));
//                 if(Input.GetMouseButtonUp(0))
//                 {
//                     StopCoroutine(TouchAnim(mousePos));
//                 }
//             }
//         }
//     }
//
//     public void DoneAnim(Slot slot)
//     {
//         var particleObj = Instantiate(particle, slot.transform.position, Quaternion.identity, slot.transform);
//         // particleObj.transform.localScale = Vector3.one;
//         particleObj.transform.localPosition = Vector3.zero;
//         particleObj.GetComponent<ParticleSystem>().Play();
//         Destroy(particleObj, 1f);
//     }
//
//     IEnumerator TouchAnim(Vector3 pos)
//     {
//         var circle = Instantiate(circle1, new Vector3(pos.x, pos.y, 0), Quaternion.identity, _gamePlayController.controller.currentClickScroll.transform.GetChild(0).transform);
//         // circle.GetComponent<SpriteMask>().sprite = _gamePlayController.controller.currentClickScroll.GetComponent<Image>().sprite;
//         circle.GetComponent<RectTransform>().DOSizeDelta(circle.GetComponent<RectTransform>().sizeDelta * 50f, .5f);
//         yield return new WaitForSeconds(.5f);
//         Destroy(circle);
//     }
//
// }
