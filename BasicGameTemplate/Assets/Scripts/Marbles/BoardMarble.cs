using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoardMarble : MonoBehaviour
{
    #region Variables
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

    #endregion

    #region Fuctions
    public void MarbleDye(Material color) // Painting
    {
        this.gameObject.GetComponent<MeshRenderer>().material = color;
    }

    public void GoToTarget(Vector3 target) // Which position will the bead go to during marble scrolling?
    {
        transform.DOMove(target, BoardManager._timer);

    }
    #endregion

}
