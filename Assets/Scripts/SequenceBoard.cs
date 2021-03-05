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
        display = GameObject.Find("Display");
    }

    public void DrawCurrentInput(List<Symbol> curInput)
    {
        foreach(Transform child in display.transform)
        {
            Destroy(child.gameObject);
        }
        foreach(Symbol symbol in curInput)
        {
            Instantiate(symbol.gameObject, display.transform);
        }
        Debug.Log("symbol created.");
    }
}
