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
    public List<Node> openList = new List<Node>();
    public List<Node> closeList = new List<Node>();
    private int level = 0;
    public List<Node> plan = new List<Node>();

    public void AStarMethod(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
    {
         

        int heuristic = abs((goals[0].ColumnId - currentPos.ColumnId)) + abs((goals[0].RowId - currentPos.RowId)); // Heuristica del nodo raiz
        openList.Add(new Node(null, currentPos.ColumnId, currentPos.RowId, heuristic));                             // Metemos la raiz en la lista abierta

        //Bucle principal
        while(openList.Count != 0){
            //Sacamos el primer elemento de la lista
            Node currentNode = openList.ElementAt(0);

            //Eliminamos el primer elemento de la lista
            openList.RemoveAt(0);

            //Comprobamos si es meta
            if (goal(currentNode, goals)) { 
                //Si es meta, lo metemos en la lista plan
                plan.Add(currentNode);
            }
            else
            {
                //Expandimos el nodo, guardamnos los hijos en la lista abierta y los ordenamos
                expand(currentNode, currentPos, boardInfo, goals);
                level++;
            }                    
        }

        int i = 0;

        while (i < level)
        {
            plan.Add(plan.ElementAt(i).father);
            i++;
        }
        plan.Reverse();
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
        //Guardamos los hijos del nodo actual en un array
        CellInfo[] childs = currentPos.WalkableNeighbours(board); 

        //Creamos los nodos correspondientes a los hijos
        for(int i = 0; i < childs.Length; i++) {

            //Calculo de la heuristica del hijo 
            int heuristic = abs((goals[0].ColumnId - childs[i].ColumnId)) + abs((goals[0].RowId - childs[i].RowId));

            //Creamos el nodo del hijo y lo insertamos a la lista abierta
            openList.Add(new Node(currentNode, childs[i].ColumnId, childs[i].RowId, heuristic));                        
        }
        // Ordenamos la lista en funcion del valor de fStar
        openList.Sort();

    }


}
