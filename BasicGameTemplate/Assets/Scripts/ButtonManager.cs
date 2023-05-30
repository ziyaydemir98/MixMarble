using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler,IPointerEnterHandler
{
    [SerializeField] BoardManager boardManager;
    InputManager inputManager;
    Button button;
    private enum ButtonType { Up = 0, Down = 1, Transfer = 2 }
    [SerializeField] private ButtonType buttonType;
    private string buttonName;
    private bool _isItOn;

    private void Awake()
    {
        inputManager= boardManager.GetComponent<InputManager>();
        button = gameObject.GetComponent<Button>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        buttonName = gameObject.name;
        inputManager.ButtonPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData) // Button bırakıldı
    {
        if (!_isItOn)
        {
            inputManager.ButtonPressed = false;
        }
    }

    public void OnPointerExit(PointerEventData eventData) // Button üzerinden çıkıldı.
    {
        if (buttonName==gameObject.name)
        {
            _isItOn = false;
        }
    }
    public void OnPointerEnter(PointerEventData eventData) // Button uzerine gelindi.
    {
        if (buttonName == gameObject.name)
        {
            _isItOn = true;
        }
    }
}
