using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    #region Variables
    private BoardManager boardManager;
    private bool onMove;
    public bool ButtonPressed;
    private Camera _cam;
    private Vector2 _touchStart, _touchEnd;
    float _distance;
    #endregion

    private void OnEnable()
    {
        boardManager = this.gameObject.GetComponent<BoardManager>();
        _cam = Camera.main;
        GameManager.Instance.NextButtonEvent.AddListener(NextBoardButton);
        GameManager.Instance.PreviousButtonEvent.AddListener(PreviousBoardButton);
        GameManager.Instance.TransferButtonEvent.AddListener(TranferButton);
    }
    private void OnDisable()
    {
        GameManager.Instance?.NextButtonEvent.RemoveListener(NextBoardButton);
        GameManager.Instance?.PreviousButtonEvent.RemoveListener(PreviousBoardButton);
        GameManager.Instance?.TransferButtonEvent.RemoveListener(TranferButton);
    }

    private void OnMouseDown()
    {
        _touchStart = _cam.ScreenToViewportPoint(Input.mousePosition);
    }
    private void OnMouseUp()
    {
        if (onMove || ButtonPressed) return;
        _touchEnd = _cam.ScreenToViewportPoint(Input.mousePosition);
        _distance = _touchEnd.y - _touchStart.y;
        if (_distance > 0.05f)
        {
            // Marbles moving up
            boardManager.Boards.ForEach(obj =>
            {
                if (obj.CanMove)
                {
                    obj.GoForward();
                    StartCoroutine(IsMoving());
                    //StartCoroutine(IsAction());
                }
            });
        }

        else if (_distance < -0.05f)
        {
            //  Marbles moving down
            boardManager.Boards.ForEach(obj =>
            {
                if (obj.CanMove)
                {
                    obj.GoBack();
                    StartCoroutine(IsMoving());
                    //StartCoroutine(IsAction());
                }
            });
        }
    }

    #region Functions
    public void NextBoardButton()
    {
        boardManager.BoardForward(false);
        StartCoroutine(IsMoving());
    }
    public void PreviousBoardButton()
    {
        boardManager.BoardForward(true);
        StartCoroutine(IsMoving());
    }
    public void TranferButton()
    {
        GameManager.Instance.OnTransfer.Invoke();
        StartCoroutine(IsMoving());
    }
    public IEnumerator IsMoving()
    {
        onMove = true;
        yield return new WaitForSeconds(BoardManager.Timer + 0.1f);
        onMove = false;
        StopCoroutine(IsMoving());
    }
    
    #endregion
    


    

}
