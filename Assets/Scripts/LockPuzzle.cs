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

    private void Awake() 
    {
        this.gameObject.SetActive(true);
    }
    
    public void OnGvrPointerHover(PointerEventData eventData)
    {
        if(puzzleKey.GetComponent<Key>().IsHeld
            && GvrControllerInput.GetDevice(GvrControllerHand.Dominant).GetButtonDown(GvrControllerButton.TouchPadButton))
        {
            Debug.Assert(thisPuzzle != null, "Warning: This Lock isn't used in any Puzzle. Create a new Key-Lock Puzzle in Bolt, and add this Lock to the `_lock` parameter.");
            //Puzzle Solved.

            thisPuzzle.Solve();
            Debug.Log("Puzzle Solved.");
            this.gameObject.SetActive(false);
        }
    }
}
