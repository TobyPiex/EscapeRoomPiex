using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using EscapeRoom;
using Gvr.Internal;

public class LockPuzzle : MonoBehaviour, ILock
{
    public KeyLockPuzzle thisPuzzle { get; set; }
    public GameObject puzzleKey { get; set; }
    public PointerEventData eData { get; set; }
    public Outline thisOutline { get; set; }

    private void Awake() 
    {
        this.gameObject.SetActive(true);
        thisOutline = this.GetComponent<Outline>();
        thisOutline.enabled = false;
    }
    
    public void OnGvrPointerHover(PointerEventData eventData)
    {
        
        thisOutline.enabled = true;
        eData = eventData;

        if(puzzleKey.GetComponent<Key>().IsHeld
        && GvrControllerInput.GetDevice(GvrControllerHand.Dominant).GetButtonDown(GvrControllerButton.TouchPadButton))
        {
            //Puzzle Solved.

            thisPuzzle.Solve();
            Debug.Log("Puzzle Solved.");
            this.gameObject.SetActive(false);
        }
    }

    private void Update() 
    {
        if(eData != null
        && !eData.hovered.Contains(this.gameObject))
        {
            thisOutline.enabled = false;
        }
    }
}
