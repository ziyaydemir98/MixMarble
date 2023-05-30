using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    private enum BoardType { Primary = 0, Secondary = 1 }
    [SerializeField] private BoardType boardTypes;
    public Material Color;
    public int BoardTypeInt
    {
        get
        {
            return (int)boardTypes;
        }
        private set { }
    }


    private enum BoardColor { White = 0, Red = 1, Blue = 2, Green = 3, Orange = 4}
    [SerializeField] private BoardColor boardColors;

    public int BoardColors
    {
        get
        {
            return (int)boardColors;
        }
        private set { }
    }


    [Tooltip("List of marbles currently on the board")]
    [Header("Marbles in board")]
    [SerializeField] private List<BoardMarble> marbles = new();
    public List<BoardMarble> Marbles
    {
        get
        {
            return marbles;
        }
        set
        {
            marbles = value;
        }
    }

    [Tooltip("Marbles in this index can be transferred to different boards.")]
    [Header("Transfer areas")]
    [SerializeField] private List<ConnectedTransferArea> changeAreas = new();

    [SerializeField] Canvas succesCanvas;
    public bool succesBoard = false;
    Color color;
    public bool _canMove; // Boncuklari hareket ettirilebilir Board bu mu?
    private TextMeshProUGUI textMeshProUGUI;





    private void Awake()
    {
        _canMove = CheckCanMove();
        BoardDye();
    }


    public bool CheckAllMarbles()
    {
        foreach (var marble in marbles)
        {
            if (marble != null && marble.MarbleColorValue != this.BoardColors)
            {
                return false;
            }
        }
        succesBoard = true;
        succesCanvas.gameObject.SetActive(true);
        return true;
        
    }





    #region Transfer Scripts



    public void GetMarbles(TransferArea shipper) // marbles into transporter
    {
        _canMove = false;
        
        foreach (var area in changeAreas)
        {
            if (shipper == area.relatedArea)
            {
                var loadIndex = 0;
            
                foreach (var curIndex in area.boardConnectionIndex)
                {
                    shipper.TakeMarble(marbles[curIndex]);
                    shipper.CarriedMarbles[loadIndex] = marbles[curIndex];
                    shipper.CarriedMarbles[loadIndex].transform.Translate(Vector3.zero);
                    marbles[curIndex] = null;

                    loadIndex++;
                }
            }
        }
    }





    public void SetMarbles(TransferArea shipper) // marbles into board
    {
        foreach (var area in changeAreas)
        {
            if (shipper == area.relatedArea)
            {
                var loadIndex = 0;

                for (int i = area.boardConnectionIndex.Count - 1; i >= 0; i--)
                {
                    var tempIndex = area.boardConnectionIndex[i];
                    marbles[tempIndex] = shipper.CarriedMarbles[loadIndex];
                    marbles[tempIndex].transform.parent = gameObject.transform;
                    shipper.CarriedMarbles[loadIndex] = null;
                    
                    loadIndex++;
                }
            }
        }
        
        _canMove = true;
    }




    #endregion













    #region Marbles Movements
    


    public void GoForward() 
    {
        if (!_canMove) return;
        else
        {
            var temp = marbles[0];
            var tempPos = marbles[^1].transform.position;

            for (int i = marbles.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    marbles[i].GoToTarget(tempPos);
                }
                else
                {
                    marbles[i].GoToTarget(marbles[i - 1].gameObject.transform.position);
                }
            }
            for (int i = 0; i < marbles.Count; i++)
            {
                if (i == marbles.Count - 1)
                {
                    marbles[i] = temp;
                }
                else
                {
                    marbles[i] = marbles[i + 1];
                }
            }
        }
        
    }





    public void GoBack()
    {
        if (!_canMove) return;
        else
        {
            var temp = marbles[^1];
            var tempPos = marbles[0].transform.position;

            for (int i = 0; i < marbles.Count; i++)
            {
                if (i == marbles.Count - 1)
                {
                    marbles[i].GoToTarget(tempPos);
                }
                else
                {
                    marbles[i].GoToTarget(marbles[i + 1].gameObject.transform.position);
                }
            }
            for (int i = marbles.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    marbles[i] = temp;
                }
                else
                {
                    marbles[i] = marbles[i - 1];
                }
            }
        }
        
    }




    private bool CheckCanMove() // SADECE BUTUN BONCUKLARIN OLDUGU TAHTA HAREKET EDEBILECEGI ICIN TAHTANIN ICINDE NULL DONDUREN BIR BONCUK VAR MI YOK MU?
    {
        foreach (var marble in marbles) 
        {
            if (marble == null)
            {
                return false;
            }
        }
        return true;
    }

    #endregion

    private void BoardDye()
    {
        foreach (Transform child in this.gameObject.transform)
        {
            if(child.name=="Background")
            {
                foreach (Transform backimages in child.transform)
                {                 
                    switch (BoardColors)
                    {
                        case 0:
                            color = new Color(1, 1, 1, 0.25f); // BEYAZ
                            break;
                        case 1:
                            color = new Color(1, 0, 0, 0.25f); // KIRMIZ
                            break;
                        case 2:
                            color = new Color(0.5f, 0.5f, 1f, 0.25f); // MAVI
                            break;
                        case 3:
                            color = new Color(0, 1, 0, 0.25f); // YESIL
                            break;
                        case 4:
                            color = new Color(1, 0.5f, 0, 0.25f); // TURUNCU
                            break;
                    }
                    backimages.GetComponent<Image>().color = color;

                    if (backimages.name == "TitleBackground")
                    {
                        textMeshProUGUI = backimages.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                    }
                }
            }
        }
        textMeshProUGUI.text = boardColors.ToString();
    }
}

//private void SuccesBoard()
//{
    
//}



[System.Serializable]
public class ConnectedTransferArea
{
    public List<int> boardConnectionIndex = new();
    public TransferArea relatedArea;
}