using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Gvr.Internal;
using Ludiq;
using Bolt;

 /// <summary>The Escape Room Package - PIEX official code.</summary>
namespace EscapeRoom
{
    /// <summary>Interface declaring a generic puzzle.</summary>
    public interface IPuzzle<T>
    {
        T answer { get; set; }
        
        bool solved { get; set; }


        void Solve();
    }

    /// <summary>Interface declaring a movable game object.</summary>
    public interface IMovable : IPointerClickHandler, IGvrPointerHoverHandler
    {
        bool IsHeld { get; set; }
        float DistanceFromHand { get; set; }
        bool PointerHover { get; set; }

        void PickUp();
        void PutDown();

    }

    //KEY-LOCK OBJECT INTERFACES.

    /// <summary>Interface declaring a Key. To be used in the Key-Lock Puzzle.</summary>
    public interface IKey
    {
        [SerializeField]
        KeyLockPuzzle thisPuzzle { get; set; }
        GameObject puzzleLock { get; set; }
    }

    /// <summary>Interface declaring a Lock. To be used in the Key-Lock Puzzle.</summary>
    public interface ILock : IGvrPointerHoverHandler
    {
        KeyLockPuzzle thisPuzzle { get; set; }
        GameObject puzzleKey { get; set; }
    }

    //MATCHING OBJECT INTERFACES.

    /// <summary>Interface declaring a Sequence Board. To be used in the Matching Puzzle.</summary>
    public interface ISequenceBoard
    {
        MatchingPuzzle thisPuzzle { get; set; }
        SymbolSet thisSymbolSet { get; set; }
    }

    /// <summary>Interface declaring a symbol.</summary>
    public interface ISymbol
    {
        GameObject symbolObject  { get; set; }
    }

    /// <summary>Interface declaring a collection of symbols.</summary>
    public interface ISymbolSet
    {
        List<Symbol> allSymbols { get; set; }
        List<Symbol> answerSymbols { get; set; }
    }

/***************************************************************************************/

    ///Symbol classes.

    //
    public class SymbolSet : ISymbolSet
    {
        public List<Symbol> allSymbols { get; set; }
        public List<Symbol> answerSymbols { get; set; }

        public SymbolSet(List<Symbol> _allSymbols, List<Symbol> _ansSymbols)
        {
            allSymbols = _allSymbols;
            answerSymbols = _ansSymbols;
        }
    }

    //Movable game object.
    public class Movable : MonoBehaviour, IMovable
    {
        public static bool FreeHand { get; set; }
        public static GameObject Controller { get; set; }
        public static GameObject ObjectHeld { get; set; }
        public static GvrControllerInputDevice controllerDevice { get; set; }
        
        public virtual float DistanceFromHand { get; set; }
        public virtual bool IsHeld { get; set; }
        public virtual bool PointerHover { get; set; }
        public virtual Vector3 initPosition { get; set; }

        public Outline thisOutline { get; set; }
        public PointerEventData eData { get; set; }
        public Transform hand { get; set; }

        public void Awake() 
        {
            FreeHand = true;
            controllerDevice = GvrControllerInput.GetDevice(GvrControllerHand.Dominant);
        }

        public void Start() 
        {    
            initPosition = this.transform.position;
            IsHeld = false;
            DistanceFromHand = -1;
            thisOutline = this.GetComponent<Outline>();
            thisOutline.enabled = false;
        }

        private void LateUpdate() 
        {
            if(Controller == null)
                Controller = GameObject.Find("GvrControllerPointer");
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(FreeHand)
            {
                if(!this.IsHeld)
                    PickUp();
            }
        }

        public virtual void OnGvrPointerHover(PointerEventData eventData) 
        {
            PointerHover = true;
            thisOutline.enabled = true;
            eData = eventData;
        }

        public virtual void PickUp() 
        {
            Debug.Log(this.name + " was picked up.");
            CustomEvent.Trigger(this.gameObject, "PickUpEvent");
            DistanceFromHand = Vector3.Distance(this.transform.position, Controller.transform.position) / 2;
            IsHeld = true;
            FreeHand = false;
            ObjectHeld = this.gameObject;
            this.GetComponent<Collider>().enabled = false;
            this.GetComponent<Rigidbody>().useGravity = false;
            gameObject.layer = 2;
        }

        public virtual void PutDown() 
        {
            Debug.Log(this.name + " was put down.");
            IsHeld = false;
            FreeHand = true;
            ObjectHeld = null;
            DistanceFromHand = -1;
            this.GetComponent<Collider>().enabled = true;
            this.GetComponent<Rigidbody>().useGravity = true;
            gameObject.layer = 0;
        }

        public virtual void Update()
        {
            if(hand == null && GameObject.Find("GvrControllerPointer") != null) 
                hand = GameObject.Find("GvrControllerPointer").transform;
            
            if(IsHeld)
            { 

                transform.localPosition =  (Controller.transform.transform.position + (Controller.transform.forward * 10) );//GvrControllerInput.GetDevice(GvrControllerHand.Dominant).
                transform.rotation = GvrControllerInput.GetDevice(GvrControllerHand.Dominant).Orientation;
                
                if(controllerDevice.GetButtonDown(GvrControllerButton.TouchPadButton))
                {
                    //PutDown();
                }            
                if(controllerDevice.GetButtonDown(GvrControllerButton.App))
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
            {
                transform.position = initPosition;
                this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
    }

    ///Puzzles.

    public class KeyLockPuzzle : IPuzzle<GameObject>
    {
        public GameObject answer { get; set; }
        public bool solved { get; set; }
        public GameObject puzzleManager { get; set; }
        public GameObject puzzleLock;

        public KeyLockPuzzle(GameObject _pManager, GameObject _key, GameObject _lock)
        {
            puzzleManager = _pManager;
            answer = _key;
            puzzleLock = _lock;
            solved = false;

            //Set the key and lock code to have this as it's "thisPuzzle"
            SetupPuzzle();
        }

        private void SetupPuzzle()
        {
            answer.GetComponent<Key>().thisPuzzle = this;
            puzzleLock.GetComponent<LockPuzzle>().thisPuzzle = this;
            answer.GetComponent<Key>().puzzleLock = puzzleLock;
            puzzleLock.GetComponent<LockPuzzle>().puzzleKey = answer;
        }

        public bool TryKey() 
        {
            if(answer.GetComponent<Key>().IsHeld)
                return true;
            return false;
        }

        //Calls this function when the puzzle is solved.
        public void Solve() 
        {
            solved = true;
            answer.GetComponent<Key>().PutDown();
            CustomEvent.Trigger(puzzleManager, "SolvedEvent");
            //This puzzle is solved.
        }
    }

    public class MatchingPuzzle : IPuzzle<List<Symbol>>
    {
        public List<Symbol> answer { get; set; }
        public List<Symbol> curInput;
        public bool solved { get; set; }
        public int answerLength { get; set; }
        public SequenceBoard thisBoard { get; set; }
        public GameObject puzzleController { get; set; }
        public List<Symbol> allSymbols { get; set; }
        public GameObject puzzleManager { get; set; }

        public MatchingPuzzle(GameObject _pManager, SymbolSet _symbolSet, GameObject _board, GameObject _pController)
        {
            puzzleManager = _pManager;
            curInput = new List<Symbol>();
            answer = _symbolSet.answerSymbols;
            allSymbols = _symbolSet.allSymbols;
            solved = false;
            thisBoard = _board.GetComponent<SequenceBoard>();
            answerLength = answer.Count;
            puzzleController = _pController;

            SetupPuzzle();
        }

        public MatchingPuzzle(GameObject _pManager, SymbolSet _symbolSet, GameObject _board)
        {
            puzzleManager = _pManager;
            curInput = new List<Symbol>();
            answer = _symbolSet.answerSymbols;
            allSymbols = _symbolSet.allSymbols;
            solved = false;
            thisBoard = _board.GetComponent<SequenceBoard>();
            answerLength = answer.Count;
            puzzleController = null;

            SetupPuzzle();
        }

        
        private void SetupPuzzle()
        {
            foreach (Symbol s in answer)
            {
                s.thisPuzzle = this;
                s.thisSymbol = s;
            }
            thisBoard.thisPuzzle = this;
        }

        public void SubmitAnswer()
        {
            for(int i = 0; i<answerLength; i++)
            {
                if(curInput[i] != answer[i])
                {
                    curInput = new List<Symbol>();
                    
                    if(thisBoard.GetComponent<AudioSource>() != null)
                        thisBoard.GetComponent<AudioSource>().Play(0);
                    
                    thisBoard.Reset();
                    return;
                }
            }
            Solve();
        }

        //Calls this function when the puzzle is solved.
        public void Solve() 
        {
            CustomEvent.Trigger(puzzleManager, "SolvedEvent"); 
            //This puzzle is solved.
            Debug.Log("Puzzle Solved!");
            solved = true;
        }
    }
}