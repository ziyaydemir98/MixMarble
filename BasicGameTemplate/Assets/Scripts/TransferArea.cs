using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TransferArea : MonoBehaviour
{
    [SerializeField] private BoardManager boardManager;
    [SerializeField] private Board boardFirst, boardSecond;
    [SerializeField] private Vector3 movePosFirst, movePosSecond;


    public List<BoardMarble> CarriedMarbles = new();
    
    private bool _inFirstPoint = true;
    private bool _canTransfer = true;

    /// <summary>
    /// Added
    /// </summary>
    private void OnEnable()
    {
        GameManager.Instance.OnTransfer.AddListener(OnMouseDown);
    }
    private void OnDisable()
    {
        GameManager.Instance?.OnTransfer.RemoveListener(OnMouseDown);
    }
    private void OnMouseDown()
    {
        if (!_canTransfer) return;
        
        _canTransfer = false;
        LoadMarbles();
    }

    private void LoadMarbles()
    {
        if (_inFirstPoint)
        {
            boardFirst.GetMarbles(this);
        }
        else
        {
            boardSecond.GetMarbles(this);
        }
        
        Mover();
    }

    private void Mover()
    {
        switch (_inFirstPoint)
        {
            case true:
                gameObject.transform.DOMove(movePosSecond,1f).OnComplete(() =>
                {
                    boardSecond.SetMarbles(this);
                    _inFirstPoint = false;
                    if (boardManager.CheckBoards())
                    {
                        GameManager.Instance.LevelSuccess.Invoke();
                        _canTransfer = false;
                    }
                    Debug.Log(boardManager.CheckBoards());
                    _canTransfer = true;
                });
                break;
            case false:
                gameObject.transform.DOMove(movePosFirst,1f).OnComplete(() =>
                {
                    boardFirst.SetMarbles(this);
                    _inFirstPoint = true;
                    if (boardManager.CheckBoards())
                    {
                        GameManager.Instance.LevelSuccess.Invoke();
                        _canTransfer = false;
                    }
                    Debug.Log(boardManager.CheckBoards());
                    _canTransfer = true;
                });
                break;
        }
    }

    public void TakeMarble(BoardMarble marble)
    {
        marble.transform.parent = transform;
    }
}
