using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleBox : MonoBehaviour
{

    Transform hinge;
    Transform door;

    void Start()
    {
        hinge = transform.Find("Hinge");
        door = transform.Find("Door");
    }

    public void Open()
    {
        StartCoroutine(OpenCoroutine());
    }

    public void Close()
    {
        StartCoroutine(CloseCoroutine());
    }

    private IEnumerator OpenCoroutine()
    {
        while(door.localEulerAngles.y >= 90)
        {
            door.RotateAround(hinge.position, -hinge.up, 35 * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator CloseCoroutine()
    {        
        while(door.localEulerAngles.y <= 270)
        {
            door.RotateAround(hinge.position, -hinge.up, -35 * Time.deltaTime);
            yield return null;
        }
    }
}
