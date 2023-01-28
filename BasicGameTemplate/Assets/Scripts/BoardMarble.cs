using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoardMarble : MonoBehaviour
{
    public void GoToTarget(Vector3 target)
    {
        transform.DOMove(target,0.5f);
    }
}
