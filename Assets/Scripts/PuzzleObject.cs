using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Gvr.Internal;
using EscapeRoom;
using Ludiq;
using Bolt;


public class PuzzleObject : Movable
{
    public Vector3 d;

    public override void PickUp()
    {
        base.PickUp();
        
        d = hand.transform.TransformPoint(this.transform.localPosition);
        Debug.Log(d);
        
    }
}
