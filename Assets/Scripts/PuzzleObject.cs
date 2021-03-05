using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Gvr.Internal;
using EscapeRoom;

public class PuzzleObject : Movable
{
    //public override bool IsHeld { get; set; }
    //public override bool PointerHover { get; set; }
    //public bool FreeHand { get; set; }
    //public override float DistanceFromHand { get; set; }
    //public GameObject Controller { get; set; }
    public Outline myOutline;
    PointerEventData eData;

    public Transform hand;

    public Vector3 d;



    void Start()
    {
        IsHeld = false;
        DistanceFromHand = -1;
        thisOutline = this.GetComponent<Outline>();
        thisOutline.enabled = false;
        this.initPosition = this.transform.position;

        //hand = GameObject.Find("GvrControllerPointer").transform;
    }

    public override void PickUp()
    {
        DistanceFromHand = Vector3.Distance(this.transform.position, Controller.transform.position) / 2;
        IsHeld = true;
        FreeHand = false;
        ObjectHeld = this.gameObject;
        this.GetComponent<Collider>().enabled = false;
        this.GetComponent<Rigidbody>().useGravity = false;
        Debug.Log(this.name + " was picked up.");

        d = hand.transform.TransformPoint(this.transform.localPosition);
        Debug.Log(d);
        
    }   

    public override void PutDown()
    {
        IsHeld = false;
        FreeHand = true;
        ObjectHeld = null;
        DistanceFromHand = -1;
        this.GetComponent<Collider>().enabled = true;
        this.GetComponent<Rigidbody>().useGravity = true;
        Debug.Log(this.name + " was put down.");
    }

    public override void OnGvrPointerHover(PointerEventData eventData)
    {
        PointerHover = true;
        thisOutline.enabled = true;
        eData = eventData;
    }

    private void Update() 
    {
        if(hand == null && GameObject.Find("GvrControllerPointer") != null) 
            hand = GameObject.Find("GvrControllerPointer").transform;
        if(IsHeld)
        { 
            //transform.position = (Controller.transform.forward * 2);

            transform.localPosition =  (Controller.transform.transform.position + (Controller.transform.forward * 2) );//GvrControllerInput.GetDevice(GvrControllerHand.Dominant).
            transform.rotation = GvrControllerInput.GetDevice(GvrControllerHand.Dominant).Orientation;

            //Vector3 rot = new Vector3(controllerDevice.TouchPos.x, controllerDevice.TouchPos.y, 0f);
            //transform.Rotate(rot, 1.0f, Space.World);

            //TouchPos will be used to bring the object closer and further away from the controller.



            // Rotate the cube by converting the angles into a quaternion.
            //Quaternion target = Quaternion.Euler(60 * controllerDevice.TouchPos.x, 0, 60 * controllerDevice.TouchPos.y);

            // Dampen towards the target rotation
            //transform.rotation = Quaternion.Slerp(transform.rotation, target,  Time.deltaTime * 5.0f);
                
            if(controllerDevice.GetButtonDown(GvrControllerButton.TouchPadButton))
            {
                PutDown();
            }
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
