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
    [SerializeField] private List<Board> SecondBoards = new();
    [SerializeField] private List<Board> boards = new();
    public List<Board> Boards
    {
        get
        {
            return boards;
        }
        private set
        {

        }
    }
    [SerializeField] Vector3 midVec, downVec, upVec;
    [SerializeField] AudioSource _audioSourceBack;

    public static float _timer = 0.25f;
    private int _boardCount;
    readonly List<Board> allBoards = new List<Board>();
    #endregion


    private void Awake()
    {
        RandomMarble();
        LoadBoards();
        GameManager.Instance.TransferAreaUpdate.Invoke();
    }

    #region Fucntions
    private void RandomMarble() // Randomize Algorithm.
    {
        List<BoardMarble> allMarbles = new List<BoardMarble>();
        List<int> MarbleTempCount = new List<int>();
        List<int> BoardTempCount = new List<int>();

        foreach (var item in boards)
        {
            if (item != null)
            {
                allBoards.Add(item);
            }
        }
        foreach (var item in SecondBoards)
        {
            allBoards.Add(item);
        }
        foreach (var item in allBoards)
        {
            foreach (var marbles in item.Marbles)
            {
                if (marbles != null)
                {

                    allMarbles.Add(marbles);
                }
            }
        }



        for (int i = 0; i < allBoards.Count; i++)
        {
            int randomCountBoard = Random.Range(0, allBoards.Count);
            while (BoardTempCount.Contains(randomCountBoard))
            {
                randomCountBoard = Random.Range(0, allBoards.Count);
            }
            BoardTempCount.Add(randomCountBoard);
            if (allBoards[randomCountBoard].Color.name != "White")
            {

                for (int b = 0; b < 17; b++)
                {

                    int randomCountMarble = Random.Range(0, allMarbles.Count);
                    while (MarbleTempCount.Contains(randomCountMarble))
                    {
                        randomCountMarble = Random.Range(0, allMarbles.Count);
                    }
                    MarbleTempCount.Add(randomCountMarble);
                    allMarbles[randomCountMarble].MarbleColorValue = (int)allBoards[randomCountBoard].BoardColors;
                    allMarbles[randomCountMarble].MarbleDye(allBoards[randomCountBoard].Color);
                }
            }
            else
            {
                for (int c = 0; c < 20; c++)
                {
                    int randomCountMarble = Random.Range(0, allMarbles.Count);
                    while (MarbleTempCount.Contains(randomCountMarble))
                    {
                        randomCountMarble = Random.Range(0, allMarbles.Count);
                    }
                    MarbleTempCount.Add(randomCountMarble);
                    allMarbles[randomCountMarble].MarbleColorValue = (int)allBoards[randomCountBoard].BoardColors;
                    allMarbles[randomCountMarble].MarbleDye(allBoards[randomCountBoard].Color);
                }
            }
        }

    }
    public bool CheckBoards() //Are all boards complete?
    {
        foreach (var board in allBoards)
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

            SecondBoards[_boardCount].gameObject.transform.DOMove(upVec, 0.75f);
            _boardCount++;
            ListControl();
            SecondBoards[_boardCount].gameObject.transform.DOMove(downVec, 0f);
            SecondBoards[_boardCount].gameObject.transform.DOMove(midVec, 0.75f).OnComplete(() =>
            {
                gameObject.GetComponent<InputManager>().ButtonPressed = false;
            });
        }
        else
        {
            SecondBoards[_boardCount].gameObject.transform.DOMove(downVec, 0.75f);
            _boardCount--;
            ListControl();
            SecondBoards[_boardCount].gameObject.transform.DOMove(upVec, 0f);
            SecondBoards[_boardCount].gameObject.transform.DOMove(midVec, 0.75f).OnComplete(() =>
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
            _boardCount = SecondBoards.Count - 1;
        }
        if (_boardCount > SecondBoards.Count - 1)
        {
            _boardCount = 0;
        }
    }
    private void LoadBoards() //Secondary board update after change
    {
        boards[1] = SecondBoards[_boardCount];
        boards[1].gameObject.SetActive(true);
    }
}
#endregion

