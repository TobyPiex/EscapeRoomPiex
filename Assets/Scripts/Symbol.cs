using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Gvr.Internal;
using EscapeRoom;

public class Symbol : MonoBehaviour, ISymbol, IPointerClickHandler
{
    public GameObject symbolObject  { get; set; }
    public MatchingPuzzle thisPuzzle { get; set; }
    public  Symbol thisSymbol { get; set; }
        
    //Constructor for the symbol object.
    public void Start()
    {
        symbolObject = this.gameObject;
        thisSymbol = this.GetComponent<Symbol>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(thisSymbol);
        thisPuzzle.curInput.Add(thisSymbol);
        Debug.Log(this.name);
        if(thisPuzzle.curInput.Count >= thisPuzzle.answerLength)
        {
            thisPuzzle.SubmitAnswer();
        }
        thisPuzzle.thisBoard.DrawCurrentInput(thisPuzzle.curInput);
    }
}