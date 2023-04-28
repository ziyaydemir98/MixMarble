using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoardMarble : MonoBehaviour
{
    private enum MarbleColor { White = 0, Red = 1, Blue = 2, Black = 3, Green = 4}
    [SerializeField] private MarbleColor marbleColors;

    public int ColorType => (int)marbleColors;
    private void OnEnable()
    {
        Color color;
        color = gameObject.GetComponent<MeshRenderer>().material.color;
        switch (ColorType)
        {
            case 0:
                color = Color.white;
                break;
            case 1:
                color = Color.red;
                break;
            case 2:
                color = Color.blue;
                break;
            case 3:
                color = Color.black;
                break;
            case 4:
                color = Color.green;
                break;
        }
        gameObject.GetComponent<MeshRenderer>().material.color = color;
    }
    public void GoToTarget(Vector3 target)
    {
        transform.DOMove(target,0.5f);
    }
}
