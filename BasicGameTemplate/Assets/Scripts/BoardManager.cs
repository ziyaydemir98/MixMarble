using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
     [SerializeField] private List<Board> boards = new();


     public bool CheckBoards()
     {
          foreach (var board in boards)
          {
               if (board.CheckAllMarbles() == false)
                    return false;
          }
          return true;
     }
}
