using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Gvr.Internal;
using EscapeRoom;

public class Symbol : MonoBehaviour, ISymbol, IPointerClickHandler, IGvrPointerHoverHandler
{
    public GameObject symbolObject  { get; set; }
    public MatchingPuzzle thisPuzzle { get; set; }
    public  Symbol thisSymbol { get; set; }
    public Outline thisOutline { get; set; }
    public PointerEventData eData { get; set; }
        
    //Constructor for the symbol object.
    public void Start()
    {
        symbolObject = this.gameObject;
        thisSymbol = this.GetComponent<Symbol>();
        thisOutline = this.GetComponent<Outline>();
        if(thisOutline != null)
            thisOutline.enabled = false;
    }

    public void OnGvrPointerHover(PointerEventData eventData) 
    {
        if(thisOutline != null)
            thisOutline.enabled = true;
        eData = eventData;
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("You deffo clicked it.. " + thisPuzzle);
        if(thisPuzzle.puzzleController == null
        || thisPuzzle.puzzleController.GetComponent<PuzzleObject>().IsHeld)
        {
            Debug.Log(thisSymbol);
            if(GetComponent<AudioSource>() != null)
                GetComponent<AudioSource>().Play(0);
            thisPuzzle.curInput.Add(thisSymbol);
            Debug.Log(this.name);
            if(thisPuzzle.curInput.Count >= thisPuzzle.answerLength)
            {
                thisPuzzle.SubmitAnswer();
            }
            thisPuzzle.thisBoard.DrawCurrentInput(thisPuzzle.curInput);
        }
    }

    public void Update()
    {
        if(eData != null
        && !eData.hovered.Contains(this.gameObject)
        && thisOutline != null)
        {
            thisOutline.enabled = false;
        }
    }
}