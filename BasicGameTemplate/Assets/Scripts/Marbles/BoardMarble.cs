using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoardMarble : MonoBehaviour
{
    private int marbleColorValue;
    public int MarbleColorValue
    {
        get
        {
            return marbleColorValue;
        }
        set
        {
            marbleColorValue = value;
        }
    }
    public bool dyed
    {
        get 
        {
            return dyed;
        }
        set
        {
            dyed = value;
        }
    }
    public void MarbleDye(Material color)
    {
        this.gameObject.GetComponent<MeshRenderer>().material = color;
    }
    public void GoToTarget(Vector3 target)
    {
        transform.DOMove(target, BoardManager._timer);
        
    }
}
