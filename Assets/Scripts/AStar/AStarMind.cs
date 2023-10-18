using Assets.Scripts;
using Assets.Scripts.DataStructures;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class AStarMind : AbstractPathMind
{
    public List<Node> openList;
    public List<Node> closeList;
    public List<Node> plan;

    public void AStarMethod(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
    {
        int heuristic = abs((goals[0].ColumnId - currentPos.ColumnId)) + abs((goals[0].RowId - currentPos.RowId)); // Heuristica del nodo raiz
        openList.Add(new Node(null, currentPos.ColumnId, currentPos.RowId, heuristic));                             // Metemos la raiz en la lista abierta

        //Bucle principal
        while(openList.Count != 0){
            Node currentNode = openList.ElementAt(0);       //Sacamos el primer elemento de la lista
            openList.RemoveAt(0);                           //Eliminamos el primer elemento de la lista

            if (goal(currentNode, goals)) {                 //Comprobamos si es meta
                plan.Add(currentNode);
            }
            else
            {

            }                    


        }
    }
    public override Locomotion.MoveDirection GetNextMove(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
    {
  
    }

    public override void Repath()
    {
        throw new System.NotImplementedException();
    }

    public int abs(int a)
    {
        if (a < 0)
        {
            a = -a;
        }

        return a;
    }

    public bool goal(Node node, CellInfo[] goal)
    {
        if (node.x == goal[0].ColumnId && node.y == goal[0].RowId)
        {
            return true;
        }
        else { 
            return false;
        }
    }

    public void expand(Node currentNode, CellInfo currentPos, BoardInfo board, CellInfo[] goals)
    {
        CellInfo[] childs = currentPos.WalkableNeighbours(board); //Guardamos los hijos del nodo actual
        //creamos los nodos correspondientes
        for(int i = 0; i < childs.Length; i++) {

             int heuristic = abs((goals[0].ColumnId - childs[i].ColumnId)) + abs((goals[0].RowId - childs[i].RowId));    //Heuristica del hijo
             openList.Add(new Node(currentNode, childs[i].ColumnId, childs[i].RowId, heuristic));                        //Metemos los hijos a la lista abierta
        }
        // Ordenamos la lista en funcion del valor de fStar
        //openList

    }
}
