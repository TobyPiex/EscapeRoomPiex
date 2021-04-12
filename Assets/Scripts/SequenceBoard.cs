using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Gvr.Internal;
using EscapeRoom;

public class SequenceBoard : MonoBehaviour, ISequenceBoard
{
    
    public MatchingPuzzle thisPuzzle { get; set; }
    public SymbolSet thisSymbolSet { get; set; }

    public GameObject display;


    void Start()
    {
        display = transform.Find("Display").gameObject;
        foreach(Transform child in display.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void DrawCurrentInput(List<Symbol> curInput)
    {
        foreach(Symbol s in curInput)
        {
            Debug.Log("curInput = " + s.gameObject.name);
            display.transform.Find(s.gameObject.name).gameObject.SetActive(true);
        }
    }

    public void Reset()
    {
        foreach(Transform child in display.transform)
        {
            child.gameObject.SetActive(false);
        }
    }
    
}
