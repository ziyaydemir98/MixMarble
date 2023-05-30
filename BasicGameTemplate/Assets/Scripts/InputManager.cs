using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    private BoardManager boardManager;
    private bool onMove;
    private bool buttonPressed;
    public bool ButtonPressed 
    {
        get
        {
            return buttonPressed;
        }
        set
        {
            buttonPressed = value;
        }
    }
    private Camera _cam;
    private Vector2 _touchStart, _touchEnd;
    float _distance;
    public List<Button> _buttons = new List<Button>();
    
    [SerializeField] AudioSource _audioSourceButton;
    [SerializeField] AudioSource _audioSourceBoardButton;


    private void Awake()
    {
        
        _cam = Camera.main;
        boardManager = this.gameObject.GetComponent<BoardManager>();
        _buttons[0].onClick.AddListener(() =>
        {    
            boardManager.BoardForward(true);
            StartCoroutine(IsMoving());
            StartCoroutine(IsAction());
            _audioSourceBoardButton.Play();
        });
        _buttons[1].onClick.AddListener(() =>
        {  
            boardManager.BoardForward(false);
            StartCoroutine(IsMoving());
            StartCoroutine(IsAction());
            _audioSourceBoardButton.Play();
        });
        _buttons[2].onClick.AddListener(() =>
        {
            GameManager.Instance.OnTransfer.Invoke();
            StartCoroutine(IsMoving());
            StartCoroutine(IsAction());
            _audioSourceButton.Play();
        });
    }

    private void OnMouseDown()
    {
        _touchStart = _cam.ScreenToViewportPoint(Input.mousePosition);
    }
    private void OnMouseUp()
    {
        if (onMove || buttonPressed) return;
        _touchEnd = _cam.ScreenToViewportPoint(Input.mousePosition);
        _distance = _touchEnd.y - _touchStart.y;
        if (_distance > 0.05f)
        {
            //Toplar ileri kayar
            boardManager.Boards.ForEach(obj =>
            {
                if (obj._canMove)
                {
                    obj.GoForward();
                    StartCoroutine(IsMoving());
                    StartCoroutine(IsAction());
                }
            });
        }

        else if (_distance < -0.05f)
        {
            //Toplar geri kayar
            boardManager.Boards.ForEach(obj =>
            {
                if (obj._canMove)
                {
                    obj.GoBack();
                    StartCoroutine(IsMoving());
                    StartCoroutine(IsAction());
                }
            });
        }
    }


    public IEnumerator IsMoving()
    {
        onMove = true;
        yield return new WaitForSeconds(BoardManager._timer + 0.1f);
        onMove = false;
        StopCoroutine(IsMoving());
    }
    public IEnumerator IsAction()
    {
        foreach (var button in _buttons)
        {
            button.interactable = false;
        }
        yield return new WaitForSeconds(BoardManager._timer+0.1f);
        foreach (var button in _buttons)
        {
            if (TransferArea.transferAreaPoint) // TASINABILEN 3 TAS PRIMARY TAHTADA ISE BUTUN BUTONLARI AKTIFLESTIR
            {
                button.interactable = true;
            }
            else // DEGILSE SADECE TRANSFERI AKTIFLESTIR
            {
                if(button.name=="TransferButton")
                {        
                    button.interactable = true;
                }
            }
            if (boardManager.Boards[1].succesBoard)
            {
                if (button.name == "TransferButton")
                {

                    button.interactable = false;
                }
                button.interactable = true;
            }
        }
        StopCoroutine(IsAction());
    }

}
