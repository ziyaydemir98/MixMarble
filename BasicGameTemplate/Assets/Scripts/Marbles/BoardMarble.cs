using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoardMarble : MonoBehaviour
{
    private enum MarbleColor { White = 0, Red = 1, Blue = 2, Black = 3, Green = 4}
    [SerializeField] private MarbleColor marbleColors;

    public int Color => (int)marbleColors;

    public void GoToTarget(Vector3 target)
    {
        transform.DOMove(target,0.5f);
    }
}
