using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Gvr.Internal;
using EscapeRoom;

public class Key : Movable, IKey
{
    public KeyLockPuzzle thisPuzzle { get; set; }
    public GameObject puzzleLock { get; set; }

    public void FreezePosition(Vector3 _position, Vector3 _rotation)
    {
        transform.position = _position;
        transform.eulerAngles = _rotation;

        Rigidbody rBody = this.GetComponent<Rigidbody>();
        rBody.useGravity = false;
        rBody.detectCollisions = false;
    }
}
