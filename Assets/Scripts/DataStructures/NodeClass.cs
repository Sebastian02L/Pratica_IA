using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Node : IComparable<Node>
{
    //Atributos de la clase
    public Node father;
    public int x;
    public int y;
    public int g;
    public int hStar;
    public int fStar;

    //Constructor
    public Node(Node fatherP,int xP, int yP, int hP) 
    {
        father = fatherP;
        x = xP;
        y = yP;

        if (father != null)
        {
            g = father.g + 1;
        }
        else
        {
            g = 0;
        }

        hStar = hP;
        fStar = g + hStar;
    }

    public int CompareTo(Node other)
    {
        if (this.fStar > other.fStar)
        {
            return -1;
        }
        else if (this.fStar < other.fStar)
        {
            return 1;
        }
        else
        {
            return 0;

        } 
    }
}
