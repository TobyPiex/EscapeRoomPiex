using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Gvr.Internal;

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

        public Outline thisOutline;

        public void Awake() 
        {
            FreeHand = true;
            controllerDevice = GvrControllerInput.GetDevice(GvrControllerHand.Dominant);
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
                Debug.Log("PointerClicked!");
                if(this.IsHeld)
                    PutDown();
                else
                    PickUp();
            }
            else
            {
                PutDown();
            }
        }

        public virtual void OnGvrPointerHover(PointerEventData eventData) {}

        public virtual void PickUp() {}

        public virtual void PutDown() {}
    }

    ///Puzzles.

    public class KeyLockPuzzle : IPuzzle<GameObject>
    {
        public GameObject answer { get; set; }
        public bool solved { get; set; }
        public GameObject puzzleLock;

        public KeyLockPuzzle(GameObject _key, GameObject _lock)
        {
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
            answer.GetComponent<Key>().PutDown();
            //This puzzle is solved.
            solved = true;
        }
    }

    public class MatchingPuzzle : IPuzzle<List<Symbol>>
    {
        public List<Symbol> answer { get; set; }
        public List<Symbol> curInput;
        public bool solved { get; set; }
        public int answerLength { get; set; }
        public SequenceBoard thisBoard { get; set; }

        public MatchingPuzzle(SymbolSet _symbolSet, GameObject _board)
        {
            curInput = new List<Symbol>();
            answer = _symbolSet.answerSymbols;
            solved = false;
            thisBoard = _board.GetComponent<SequenceBoard>();
            answerLength = answer.Count;

            SetupPuzzle();
        }

        
        private void SetupPuzzle()
        {
            foreach (Symbol s in answer)
            {
                s.thisPuzzle = this;
                s.thisSymbol = s;
            }
            Debug.Log("Done");
        }

        public void SubmitAnswer()
        {
            for(int i = 0; i<answerLength; i++)
            {
                if(curInput[i] != answer[i])
                {
                    curInput = new List<Symbol>();
                    thisBoard.DrawCurrentInput(curInput);
                    return;
                }
            }
            Solve();
        }

        //Calls this function when the puzzle is solved.
        public void Solve() 
        {
            //This puzzle is solved.
            Debug.Log("Puzzle Solved!");
            solved = true;
        }
    }
}