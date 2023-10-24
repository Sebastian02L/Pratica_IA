using Assets.Scripts;
using Assets.Scripts.DataStructures;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using JetBrains.Annotations;

public class AStarMind : AbstractPathMind
{
    public List<Node> openList = new List<Node>();
    public List<Node> plan = new List<Node>();

    public void AStarMethod(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
    {
        bool goalReached = false;

        int heuristic = Math.Abs((goals[0].ColumnId - currentPos.ColumnId)) + Math.Abs((goals[0].RowId - currentPos.RowId)); // Heuristica del nodo raiz
        openList.Add(new Node(null, currentPos.ColumnId, currentPos.RowId, heuristic));                             // Metemos la raiz en la lista abierta

        //Bucle A*
        while(openList.Count != 0 && !goalReached){
            //Almacenamos el primer elemento de la lista
            Node currentNode = openList.ElementAt(0);

            //Eliminamos el primer elemento de la lista
            openList.RemoveAt(0);

            //Comprobamos si es meta
            if (goal(currentNode, goals)) { 
                //Si es meta, lo metemos en la lista plan
                plan.Add(currentNode);
                goalReached = true;
                Debug.Log("Meta alcanzada");
            }
            else
            {
                //Expandimos el nodo, guardamnos los hijos en la lista abierta y los ordenamos
                expand(currentNode, boardInfo, goals);

                // Ordenamos la lista en funcion del valor de fStar
                openList.Sort();
            }

            if (openList.Count > (boardInfo.NumColumns * boardInfo.NumRows))
            {
                Debug.Log("No hay solucion");
                break;
            }
        }

        if (plan.Count != 0)
        {
            int i = 0;

            while (plan.ElementAt(i).father != null)
            {
                plan.Add(plan.ElementAt(i).father);
                i++;
            }

            plan.Reverse();
        }
        else
        {
            Debug.Log("No hay solucion");
        }
    }
    public override Locomotion.MoveDirection GetNextMove(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
    {
        if (plan.Count == 0)
        {
            AStarMethod(boardInfo, currentPos, goals);

            return Locomotion.MoveDirection.None;
        }
        else
        {

            Node move = plan.ElementAt(0);
            plan.RemoveAt(0);

            if (currentPos.ColumnId == move.x && currentPos.RowId > move.y)
            {
                return Locomotion.MoveDirection.Down;
            }

            if (currentPos.ColumnId == move.x && currentPos.RowId < move.y)
            {
                return Locomotion.MoveDirection.Up;
            }

            if (currentPos.ColumnId < move.x && currentPos.RowId == move.y)
            {
                return Locomotion.MoveDirection.Right;
            }

            return Locomotion.MoveDirection.Left;
        }
    }

    public override void Repath()
    {
        
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

    public void expand(Node currentNode, BoardInfo board, CellInfo[] goals)
    {
        CellInfo actualPosition = new CellInfo(currentNode.x, currentNode.y);

        //Guardamos los hijos del nodo actual en un array
        CellInfo[] childs = actualPosition.WalkableNeighbours(board);

        //Creamos los nodos correspondientes a los hijos
        for (int i = 0; i < childs.Length; i++) {
            if (childs[i] != null) {
                //Calculo de la heuristica del hijo 
                int heuristic = Math.Abs((goals[0].ColumnId - childs[i].ColumnId)) + Math.Abs((goals[0].RowId - childs[i].RowId));

                bool repeatedNode = false;
                Node nodeToInsert = new Node(currentNode, childs[i].ColumnId, childs[i].RowId, heuristic);

                foreach (Node node in openList)
                {
                    if(nodeToInsert.x == node.x && nodeToInsert.y == node.y)
                    {
                        repeatedNode = true;
                    }
                }

                if (!repeatedNode)
                {
                    //Creamos el nodo del hijo y lo insertamos a la lista abierta
                    openList.Add(nodeToInsert);
                }
            }
        }
    }
}
