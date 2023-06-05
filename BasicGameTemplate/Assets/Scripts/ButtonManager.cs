using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler,IPointerEnterHandler
{
    #region Variables
    [SerializeField] BoardManager boardManager;
    InputManager inputManager;
    private string _buttonName;
    private bool _isItOn;
    #endregion


    private void OnEnable()
    {
        inputManager = boardManager.GetComponent<InputManager>();
    }

    #region Functions
    public void OnPointerDown(PointerEventData eventData) // button down
    {
        _buttonName = gameObject.name;
        inputManager.ButtonPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData) // button press finished
    {
        if (!_isItOn)
        {
            inputManager.ButtonPressed = false;
        }
    }

    public void OnPointerExit(PointerEventData eventData) // cursor entered button
    {
        if (_buttonName == gameObject.name)
        {
            _isItOn = false;
        }
    }
    public void OnPointerEnter(PointerEventData eventData) // cursor left button
    {
        if (_buttonName == gameObject.name)
        {
            _isItOn = true;
        }
    }
    #endregion

}
