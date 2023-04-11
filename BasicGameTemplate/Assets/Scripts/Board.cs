using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [Tooltip("List of marbles currently on the board")]
    [Header("Marbles in board")]
    [SerializeField] private List<BoardMarble> marbles = new();

    [Tooltip("Marbles in this index can be transferred to different boards.")]
    [Header("Transfer areas")]
    [SerializeField] private List<ConnectedTransferArea> changeAreas = new();

    //Movement values
    private Camera _cam;
    private Vector3 _touchStart;
    
    private bool _touched;
    private bool _canMove;

    private void Awake()
    {
        _cam = Camera.main;

        _canMove = CheckCanMove();
    }

    private void OnMouseDown()
    {
        if (!_canMove) return;
        
        _touchStart = _cam.ScreenToViewportPoint(Input.mousePosition);
        _touched = true;
    }

    private void OnMouseUp()
    {
        if (!_touched && !_canMove) return;

        _touched = false;
        
        var touchEnd = _cam.ScreenToViewportPoint(Input.mousePosition);
        var distance = touchEnd.y - _touchStart.y;

        if (distance > 0.05f)
        {
            //Toplar ileri kayar
            GoForward();
        }
        else if (distance < -0.05f)
        {
            //Toplar geri kayar
            GoBack();
        }
    }
    public bool CheckAllMarbles()
    {
        var tempColor = marbles[0].Color;
        
        foreach (var marble in marbles)
        {
            if (marble != null)
            {
                tempColor = marble.Color;
                break;
            }    
        }
        
        foreach (var marble in marbles)
        {
            if (marble != null && marble.Color != tempColor)
                return false;
        }

        return true;
    }
    #region Transfer Scripts

    private bool CheckCanMove()
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
            
                // foreach (var curIndex in area.boardConnectionIndex)
                // {
                //     marbles[curIndex] = shipper.CarriedMarbles[loadIndex];
                //     marbles[curIndex].transform.parent = gameObject.transform;
                //     shipper.CarriedMarbles[loadIndex] = null;
                //
                //     loadIndex++;
                // }

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
    
    private void GoForward()
    {
        var temp = marbles[0];
        var tempPos = marbles[^1].transform.position;
        
        for (int i = marbles.Count-1; i >= 0; i--) 
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
    private void GoBack()
    {
        var temp = marbles[^1];
        var tempPos = marbles[0].transform.position;
        
        for (int i = 0; i < marbles.Count; i++)
        {
            if (i == marbles.Count - 1)
            {
                marbles[i].GoToTarget(tempPos); ;
            }
            else
            {
                marbles[i].GoToTarget(marbles[i + 1].gameObject.transform.position);
            }
        }
        for (int i = marbles.Count-1 ; i >= 0; i--)
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
        
    #endregion
}

[System.Serializable]
public class ConnectedTransferArea
{
    public List<int> boardConnectionIndex = new();
    public TransferArea relatedArea;
}