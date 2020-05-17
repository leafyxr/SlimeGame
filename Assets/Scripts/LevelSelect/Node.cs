using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Node Class for procedural generation
public class Node
{
    public Vector2 posistion;
    public int ID;
    public bool doorTop, doorBottom, doorLeft, doorRight;
    public Node(Vector2 pos, int id)
    {
        posistion = pos;
        ID = id;
    }
}
