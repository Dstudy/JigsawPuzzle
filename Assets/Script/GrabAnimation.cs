// using System.Collections;
// using System.Collections.Generic;
// using Unity.VisualScripting;
// using UnityEngine;
//
// public class GrabAnimation : MonoBehaviour
// {
//     public ParticleSystem particle;
//     [SerializeField] ParticleSystem currentParticle;
//     // Start is called before the first frame update
//     void Start()
//     {
//         
//     }
//
//     // Update is called once per frame
//     void Update()
//     {
//         var mousePos = Input.mousePosition;
//         var wordPos = Camera.main.ScreenToWorldPoint(mousePos);
//         wordPos.z = 0;
//         if (Input.GetMouseButtonDown(0))
//         {
//             if(currentParticle != null)
//             {
//                 currentParticle.Stop();
//                 Destroy(currentParticle.gameObject);
//             }
//             if(PuzzleController.Instance.currentObject == null)
//                 Debug.Log("Not found");
//             Invoke("PlayParticle",0.01f);
//             if(GameController.Instance.controller.currentClickScroll != null)
//             {
//                 currentParticle = Instantiate(particle, wordPos, Quaternion.identity, GameController.Instance.controller.currentClickScroll.transform);
//                 currentParticle.Play();
//             }
//         }
//
//         // if (Input.GetMouseButtonUp(0))
//         // {
//         //     if(currentParticle != null)
//         //     {
//         //         currentParticle.Stop();
//         //         Destroy(currentParticle.gameObject);
//         //     }
//         // }
//     }
//     
//     public void PlayParticle()
//     {
//         if(currentParticle != null)
//         {
//             currentParticle.Stop();
//             Destroy(currentParticle.gameObject);
//         }
//         if(PuzzleController.Instance.currentObject != null)
//         {
//             currentParticle = Instantiate(particle, PuzzleController.Instance.currentObject.transform.position, Quaternion.identity, PuzzleController.Instance.currentObject.transform);
//             currentParticle.Play();
//         }
//         
//     }
// }
