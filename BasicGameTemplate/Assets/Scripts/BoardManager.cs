using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;
using System.Drawing;
using UnityEngine.PlayerLoop;

public class BoardManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private List<Board> _secondBoards = new();
    [SerializeField] public List<Board> Boards = new();
    [SerializeField] private Vector3 _midVec, _downVec, _upVec;

    public static float Timer = 0.25f;
    private int _boardCount;
    readonly List<Board> _allBoards = new List<Board>();
    #endregion


    private void OnEnable()
    {
        RandomMarble();
        LoadBoards();
        GameManager.Instance.TransferAreaUpdate.Invoke();
    }

    #region Fucntions

    private void RandomMarble() // Randomize Algorithm.
    {
        List<BoardMarble> _allMarbles = new List<BoardMarble>();
        List<int> _marbleTempCount = new List<int>();
        List<int> _boardTempCount = new List<int>();

        foreach (var item in Boards)
        {
            if (item != null)
            {
                _allBoards.Add(item);
            }
        }
        foreach (var item in _secondBoards)
        {
            _allBoards.Add(item);
        }
        foreach (var item in _allBoards)
        {
            foreach (var marbles in item.Marbles)
            {
                if (marbles != null)
                {

                    _allMarbles.Add(marbles);
                }
            }
        }



        for (int i = 0; i < _allBoards.Count; i++)
        {
            int randomCountBoard = Random.Range(0, _allBoards.Count);
            while (_boardTempCount.Contains(randomCountBoard))
            {
                randomCountBoard = Random.Range(0, _allBoards.Count);
            }
            _boardTempCount.Add(randomCountBoard);
            if (_allBoards[randomCountBoard].Color.name != "White")
            {

                for (int b = 0; b < 17; b++)
                {

                    int randomCountMarble = Random.Range(0, _allMarbles.Count);
                    while (_marbleTempCount.Contains(randomCountMarble))
                    {
                        randomCountMarble = Random.Range(0, _allMarbles.Count);
                    }
                    _marbleTempCount.Add(randomCountMarble);
                    _allMarbles[randomCountMarble].MarbleColorValue = (int)_allBoards[randomCountBoard].BoardColors;
                    _allMarbles[randomCountMarble].MarbleDye(_allBoards[randomCountBoard].Color);
                }
            }
            else
            {
                for (int c = 0; c < 20; c++)
                {
                    int randomCountMarble = Random.Range(0, _allMarbles.Count);
                    while (_marbleTempCount.Contains(randomCountMarble))
                    {
                        randomCountMarble = Random.Range(0, _allMarbles.Count);
                    }
                    _marbleTempCount.Add(randomCountMarble);
                    _allMarbles[randomCountMarble].MarbleColorValue = (int)_allBoards[randomCountBoard].BoardColors;
                    _allMarbles[randomCountMarble].MarbleDye(_allBoards[randomCountBoard].Color);
                }
            }
        }

    }
    public bool CheckBoards() //Are all boards complete?
    {
        foreach (var board in _allBoards)
        {
            if (board.CheckAllMarbles() == false)
                return false;
        }
        return true;
    }
    public void BoardForward(bool key) // Board Replace
    {
        ListControl();
        if (key)
        {

            _secondBoards[_boardCount].gameObject.transform.DOMove(_upVec, 0.75f);
            _boardCount++;
            ListControl();
            _secondBoards[_boardCount].gameObject.transform.DOMove(_downVec, 0f);
            _secondBoards[_boardCount].gameObject.transform.DOMove(_midVec, 0.75f).OnComplete(() =>
            {
                gameObject.GetComponent<InputManager>().ButtonPressed = false;
            });
        }
        else
        {
            _secondBoards[_boardCount].gameObject.transform.DOMove(_downVec, 0.75f);
            _boardCount--;
            ListControl();
            _secondBoards[_boardCount].gameObject.transform.DOMove(_upVec, 0f);
            _secondBoards[_boardCount].gameObject.transform.DOMove(_midVec, 0.75f).OnComplete(() =>
            {
                gameObject.GetComponent<InputManager>().ButtonPressed = false;
            });
        }
        LoadBoards();
        GameManager.Instance.TransferAreaUpdate.Invoke();
    }
    private void ListControl() // Boards List Count
    {
        if (_boardCount < 0)
        {
            _boardCount = _secondBoards.Count - 1;
        }
        if (_boardCount > _secondBoards.Count - 1)
        {
            _boardCount = 0;
        }
    }
    private void LoadBoards() //Secondary board update after change
    {
        Boards[1] = _secondBoards[_boardCount];
        Boards[1].gameObject.SetActive(true);
    }
}
#endregion

