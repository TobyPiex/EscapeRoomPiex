using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Gvr.Internal;
using EscapeRoom;

public class Key : Movable, IKey
{
    public KeyLockPuzzle thisPuzzle { get; set; }
    [SerializeField]
    public GameObject puzzleLock { get; set; }

    private Outline myOutline;
    private PointerEventData eData;

    void Start()
    {
        IsHeld = false;
        DistanceFromHand = -1;

        thisOutline = this.GetComponent<Outline>();
        thisOutline.enabled = false;
    }

    public override void PickUp()
    {
        DistanceFromHand = Vector3.Distance(this.transform.position, Controller.transform.position);
        this.GetComponent<Collider>().enabled = (false);
        this.GetComponent<Rigidbody>().useGravity = false;
        IsHeld = true;
        FreeHand = false;
        ObjectHeld = this.gameObject;
    }   

    public override void PutDown()
    {
        IsHeld = false;
        FreeHand = true;
        this.GetComponent<Collider>().enabled = (true);
        this.GetComponent<Rigidbody>().useGravity = true;
        ObjectHeld = null;
        DistanceFromHand = -1;
    }

    public override void OnGvrPointerHover(PointerEventData eventData)
    {
        PointerHover = true;
        thisOutline.enabled = true;
        eData = eventData;
    }

    private void Update() 
    {
        if(IsHeld)
        {

            transform.localPosition =  (Controller.transform.transform.position + (Controller.transform.forward * 2) );//GvrControllerInput.GetDevice(GvrControllerHand.Dominant).
            Vector3 direction = this.transform.position - Controller.transform.position;
            this.transform.rotation = Quaternion.LookRotation(direction);
            if(GvrControllerInput.GetDevice(GvrControllerHand.Dominant).GetButtonDown(GvrControllerButton.TouchPadButton))
            {
                PutDown();
            }
        }
        else
        {
        }

        
        if(eData != null
            && !eData.hovered.Contains(this.gameObject))
            {
                thisOutline.enabled = false;
            }
        
        if(transform.position.y <= -200)
            transform.position = initPosition;
    }
}
