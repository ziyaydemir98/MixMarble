using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TransferArea : MonoBehaviour
{
    #region Variables
    [SerializeField] private BoardManager boardManager;
    [SerializeField] private Vector3 movePosFirst, movePosSecond;
    public List<BoardMarble> CarriedMarbles = new();
    public static bool transferAreaPoint = true;

    private Board boardFirst, boardSecond;
    #endregion

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

    #region Functions
    private void LoadMarbles() //Which board does the transfer agent go to?
    {
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

    private void Mover() // bead transfer
    {
        switch (transferAreaPoint) // Where is my Transfer Area?
        {
            case true:
                gameObject.transform.DOMove(movePosSecond, BoardManager.Timer).OnComplete(() =>
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
                gameObject.transform.DOMove(movePosFirst, BoardManager.Timer).OnComplete(() =>
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

    public void TakeMarble(BoardMarble marble) //pass the swap beads into the transfer agent
    {
        marble.transform.parent = transform;
    }

    private void BoardsTake() //Updates the destination of the transfer agent.
    {
        boardFirst = boardManager.Boards[0];
        boardSecond = boardManager.Boards[1];
    }
    #endregion

}
