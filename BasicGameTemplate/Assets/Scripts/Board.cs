using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    #region Variables

    private enum BoardType { Primary = 0, Secondary = 1 }
    [SerializeField] private BoardType boardTypes;
    public int BoardTypeInt => (int)boardTypes;



    private enum BoardColor { White = 0, Red = 1, Blue = 2, Green = 3, Orange = 4 }
    [SerializeField] private BoardColor boardColors;
    public int BoardColors => (int)boardColors;


    [Tooltip("List of marbles currently on the board")]
    [Header("Marbles in board")]
    [SerializeField] public List<BoardMarble> Marbles = new List<BoardMarble>();
    [Tooltip("Marbles in this index can be transferred to different boards.")]
    [Header("Transfer areas")]
    [SerializeField] private List<ConnectedTransferArea> changeAreas = new();


    [SerializeField] Canvas succesCanvas;
    public Material Color; // The material to be given to the beads that should belong to the board
    public bool SuccesBoard = false; //Is the board complete?
    public bool CanMove; // Boncuklari hareket ettirilebilir Board bu mu?
    private TextMeshProUGUI _textMeshProUGUI;
    Color _color;
    #endregion



    private void OnEnable()
    {
        CanMove = CheckCanMove();
        BoardDye();
    }

    #region Functions

    public bool CheckAllMarbles() //Does the color of the board match the beads on the board? 
    {
        foreach (var marble in Marbles)
        {
            if (marble != null && marble.MarbleColorValue != this.BoardColors)
            {
                return false;
            }
        }
        if ((int)boardTypes != 0)
        {
            SuccesBoard = true;
            succesCanvas.gameObject.SetActive(true);
        }
        return true;

    }

    private void BoardDye() //paint your board background 
    {
        foreach (Transform child in this.gameObject.transform)
        {
            if (child.name == "Background")
            {
                foreach (Transform backimages in child.transform)
                {
                    switch (BoardColors)
                    {
                        case 0:
                            _color = new Color(1, 1, 1, 0.25f); // White
                            break;
                        case 1:
                            _color = new Color(1, 0, 0, 0.25f); // Red
                            break;
                        case 2:
                            _color = new Color(0.5f, 0.5f, 1f, 0.25f); // Blue
                            break;
                        case 3:
                            _color = new Color(0, 1, 0, 0.25f); // Green
                            break;
                        case 4:
                            _color = new Color(1, 0.5f, 0, 0.25f); // Orange
                            break;
                    }
                    backimages.GetComponent<Image>().color = _color;

                    if (backimages.name == "TitleBackground")
                    {
                        _textMeshProUGUI = backimages.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                    }
                }
            }
        }
        _textMeshProUGUI.text = boardColors.ToString();
    }

    #endregion


    #region Transfer Scripts

    public void GetMarbles(TransferArea shipper) // marbles into transporter
    {
        CanMove = false;
        
        foreach (var area in changeAreas)
        {
            if (shipper == area.RelatedArea)
            {
                var loadIndex = 0;
            
                foreach (var curIndex in area.BoardConnectionIndex)
                {
                    shipper.TakeMarble(Marbles[curIndex]);
                    shipper.CarriedMarbles[loadIndex] = Marbles[curIndex];
                    shipper.CarriedMarbles[loadIndex].transform.Translate(Vector3.zero);
                    Marbles[curIndex] = null;

                    loadIndex++;
                }
            }
        }
    }

    public void SetMarbles(TransferArea shipper) // marbles into board
    {
        foreach (var area in changeAreas)
        {
            if (shipper == area.RelatedArea)
            {
                var loadIndex = 0;

                for (int i = area.BoardConnectionIndex.Count - 1; i >= 0; i--)
                {
                    var tempIndex = area.BoardConnectionIndex[i];
                    Marbles[tempIndex] = shipper.CarriedMarbles[loadIndex];
                    Marbles[tempIndex].transform.parent = gameObject.transform;
                    shipper.CarriedMarbles[loadIndex] = null;
                    
                    loadIndex++;
                }
            }
        }
        
        CanMove = true;
    }

    #endregion


    #region Marbles Movements

    private bool CheckCanMove() //Who is the movable board?
    {
        foreach (var marble in Marbles)
        {
            if (marble == null)
            {
                return false;
            }
        }
        return true;
    }

    public void GoForward() 
    {
        if (!CanMove) return;
        else
        {
            var temp = Marbles[0];
            var tempPos = Marbles[^1].transform.position;

            for (int i = Marbles.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    Marbles[i].GoToTarget(tempPos);
                }
                else
                {
                    Marbles[i].GoToTarget(Marbles[i - 1].gameObject.transform.position);
                }
            }
            for (int i = 0; i < Marbles.Count; i++)
            {
                if (i == Marbles.Count - 1)
                {
                    Marbles[i] = temp;
                }
                else
                {
                    Marbles[i] = Marbles[i + 1];
                }
            }
        }
        
    }

    public void GoBack()
    {
        if (!CanMove) return;
        else
        {
            var temp = Marbles[^1];
            var tempPos = Marbles[0].transform.position;

            for (int i = 0; i < Marbles.Count; i++)
            {
                if (i == Marbles.Count - 1)
                {
                    Marbles[i].GoToTarget(tempPos);
                }
                else
                {
                    Marbles[i].GoToTarget(Marbles[i + 1].gameObject.transform.position);
                }
            }
            for (int i = Marbles.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    Marbles[i] = temp;
                }
                else
                {
                    Marbles[i] = Marbles[i - 1];
                }
            }
        }
        
    }

   

    #endregion

    
}




[System.Serializable]
public class ConnectedTransferArea
{
    public List<int> BoardConnectionIndex = new();
    public TransferArea RelatedArea;
}