using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TransferArea : MonoBehaviour
{
    [SerializeField] private BoardManager boardManager;
    private Board boardFirst, boardSecond;
    [SerializeField] private Vector3 movePosFirst, movePosSecond;


    public List<BoardMarble> CarriedMarbles = new();
    
    public static bool transferAreaPoint = true;

    /// <summary>
    /// Added
    /// </summary>


    private void OnEnable()
    {
        BoardsTake();
        GameManager.Instance.OnTransfer.AddListener(LoadMarbles);
        GameManager.Instance.TransferAreaUpdate.AddListener(BoardsTake);
    }
    private void OnDisable()
    {
        GameManager.Instance?.OnTransfer.RemoveListener(LoadMarbles);
        GameManager.Instance?.TransferAreaUpdate.RemoveListener(BoardsTake);
    }

    private void LoadMarbles()
    {
        Debug.Log("CALISTI");
        if (transferAreaPoint)
        {
            boardFirst.GetMarbles(this);
        }
        else
        {
            boardSecond.GetMarbles(this);
        }
        
        Mover();
    }

    private void Mover() // Boncuk aktarim Algoritmasi.
    {
        switch (transferAreaPoint) // Transfer Alanim nerede?
        {
            case true: 
                gameObject.transform.DOMove(movePosSecond, BoardManager._timer).OnComplete(() =>
                {
                    boardSecond.SetMarbles(this); 
                    transferAreaPoint = false;
                    boardManager.GetComponent<InputManager>().ButtonPressed = false;
                    if (boardManager.CheckBoards())
                    {
                        GameManager.Instance.LevelSuccess.Invoke();
                    }
                    
                });
                break;
            case false:
                gameObject.transform.DOMove(movePosFirst, BoardManager._timer).OnComplete(() =>
                {
                    boardFirst.SetMarbles(this);
                    transferAreaPoint = true;
                    boardManager.GetComponent<InputManager>().ButtonPressed = false;
                    if (boardManager.CheckBoards())
                    {
                        GameManager.Instance.LevelSuccess.Invoke();
                    }
                });
                break;
        }
    }

    public void TakeMarble(BoardMarble marble)
    {
        marble.transform.parent = transform;
    }

    private void BoardsTake()
    {
        boardFirst = boardManager.Boards[0];
        boardSecond = boardManager.Boards[1];
    }
}
