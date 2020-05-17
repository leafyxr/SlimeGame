using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGenerator : MonoBehaviour
{
    //World Size
    [SerializeField]
    int size_X = 4;
    [SerializeField]
    int size_Y = 4;

    Vector2 worldSize;

    //List of potential nodes
    Node[,] nodes;
    //List of used positions
    List<Vector2> usedPos = new List<Vector2>();

    //Size of Grid
    int sizeX, sizeY;

    //Number of nodes
    [SerializeField]
    int NodeNo = 10;

    //Size of sprites(Seperation)
    public Vector2 spriteSize = new Vector2(10, 10);
    public GameObject Object;

    private void Start()
    {
        //Set size
        worldSize = new Vector2(size_X, size_Y);

        //Reset node number if too large
        if (NodeNo >= (worldSize.x * 2) * (worldSize.y * 2))
        {
            NodeNo = Mathf.RoundToInt((worldSize.x * 2) * (worldSize.y * 2));
        }
        sizeX = Mathf.RoundToInt(worldSize.x);
        sizeY = Mathf.RoundToInt(worldSize.y);
        //Generate Nodes
        Generate();
        //Set Connections
        SetDoors();
        //Render
        Draw();
    }

    //Generate Nodes
    void Generate()
    {
        //Create empty nodes
        nodes = new Node[sizeX * 2, sizeY * 2];

        //Create starting node
        Vector2 curPos = Vector2.zero;
        nodes[sizeX, sizeY] = new Node(curPos, 1);
        usedPos.Insert(0, curPos);

        //Create rest of nodes
        for(int i = 0; i < NodeNo-1; i++)
        {
            curPos = selectPos();
            Debug.Log("Connections : " + checkAdjacenciesofAdjacencies(curPos));
            //Regen if more than 1 other connection
            if (checkAdjacenciesofAdjacencies(curPos) > 1)
            {
                int a = 0;
                int r = (int)Random.Range(1, 3);
                if (r == 3) r = (int)Random.Range(1, 3);
                Debug.Log("Retry #1 - Target : " + r);
                //Regen Node until correct number of adjacenies
                while (checkAdjacenciesofAdjacencies(curPos) != r && a < 200)
                {
                    curPos = selectPos();
                    a++;
                }
                Debug.Log("Connections : " + checkAdjacenciesofAdjacencies(curPos) + " Target : " + r + " | Attempts :" + a);
                
            }
            //Create new node
            nodes[(int)curPos.x + sizeX, (int)curPos.y + sizeY] = new Node(curPos, 0);
            //Add to Used
            usedPos.Insert(0, curPos);
        }
    }

    enum direction { Up, Down, Left, Right };


    Vector2 selectPos()
    {
        int x = 0, y = 0;
        Vector2 currPos = Vector2.zero;

        //Randomly selects a node based on the last one generated
        do
        {
            int index = Mathf.RoundToInt(Random.value * (usedPos.Count - 1));
            x = (int)usedPos[index].x;
            y = (int)usedPos[index].y;

            //randomly selects a direction
            direction dir = (direction)Random.Range(0, 3);
            switch (dir)
            {
                case direction.Up:
                    y++;
                    break;
                case direction.Down:
                    y--;
                    break;
                case direction.Left:
                    x++;
                    break;
                case direction.Right:
                    x--;
                    break;
            }
            currPos = new Vector2(x, y);
        }
        //Continues until an empty position is found
        while (usedPos.Contains(currPos) || x>=sizeX || x<-sizeX || y >=sizeY || y<-sizeY);
        return currPos;
    }

    //Returns Number of Adjaciencies
    int checkAdjacencies(Vector2 pos)
    {
        int i = 0;
        if (usedPos.Contains(pos + Vector2.right)) i++;
        if (usedPos.Contains(pos + Vector2.left)) i++;
        if (usedPos.Contains(pos + Vector2.up)) i++;
        if (usedPos.Contains(pos + Vector2.down)) i++;
        return i;
    }

    //Returns highest Number of Adjaciencies on adjacent nodes
    int checkAdjacenciesofAdjacencies(Vector2 pos)
    {
        int a = 0;
        if (usedPos.Contains(pos + Vector2.right))
        {
            Vector2 TempPos = pos + Vector2.right;
            int i = 0;
            if (usedPos.Contains(TempPos + Vector2.right)) i++;
            if (usedPos.Contains(TempPos + Vector2.left)) i++;
            if (usedPos.Contains(TempPos + Vector2.up)) i++;
            if (usedPos.Contains(TempPos + Vector2.down)) i++;

            if (i > a) a = i;
        }
        if (usedPos.Contains(pos + Vector2.left))
        {
            Vector2 TempPos = pos + Vector2.left;
            int i = 0;
            if (usedPos.Contains(TempPos + Vector2.right)) i++;
            if (usedPos.Contains(TempPos + Vector2.left)) i++;
            if (usedPos.Contains(TempPos + Vector2.up)) i++;
            if (usedPos.Contains(TempPos + Vector2.down)) i++;

            if (i > a) a = i;
        }
        if (usedPos.Contains(pos + Vector2.up))
        {
            Vector2 TempPos = pos + Vector2.up;
            int i = 0;
            if (usedPos.Contains(TempPos + Vector2.right)) i++;
            if (usedPos.Contains(TempPos + Vector2.left)) i++;
            if (usedPos.Contains(TempPos + Vector2.up)) i++;
            if (usedPos.Contains(TempPos + Vector2.down)) i++;

            if (i > a) a = i;
        }
        if (usedPos.Contains(pos + Vector2.down))
        {
            Vector2 TempPos = pos + Vector2.down;
            int i = 0;
            if (usedPos.Contains(TempPos + Vector2.right)) i++;
            if (usedPos.Contains(TempPos + Vector2.left)) i++;
            if (usedPos.Contains(TempPos + Vector2.up)) i++;
            if (usedPos.Contains(TempPos + Vector2.down)) i++;

            if (i > a) a = i;
        }
        return a;
    }

    //Sets nodes connections
    void SetDoors()
    {
        for (int x = 0; x < sizeX*2; x++)
        {
            for (int y = 0; y < sizeX * 2; y++)
            {
                Debug.Log("X: " + x + ", Y: " + y + ", SizeX : " + nodes.GetLength(0) + ", SizeY : " + nodes.GetLength(1));
                if (nodes[x, y] == null) continue;

                //Bottom
                if (y - 1 < 0) nodes[x, y].doorBottom = false;
                else nodes[x, y].doorBottom = nodes[x, y - 1] != null;

                //Top
                if (y + 1 >= sizeY * 2) nodes[x, y].doorTop = false;
                else nodes[x, y].doorTop = nodes[x, y + 1] != null;

                //Left
                if (x - 1 < 0) nodes[x, y].doorLeft = false;
                else nodes[x, y].doorLeft = nodes[x - 1, y] != null;

                //Right
                if (x + 1 >= sizeX * 2) nodes[x, y].doorRight = false;
                else nodes[x, y].doorRight = nodes[x + 1, y] != null;
            }
        }
    }
    
    //Draw nodes
    void Draw()
    {
        foreach (Node node in nodes)
        {
            //Set Position
            if (node == null) continue;
            Vector2 pos = node.posistion;
            pos.x *= spriteSize.x;
            pos.y *= spriteSize.y;

            //Generate sprite
            SpriteSelector spriteSelector = Instantiate(Object as GameObject, pos, Quaternion.identity).GetComponent<SpriteSelector>();
            spriteSelector.id = node.ID;
            spriteSelector.up = node.doorTop;
            spriteSelector.down = node.doorBottom;
            spriteSelector.left = node.doorLeft;
            spriteSelector.right = node.doorRight;
        }
    }
}
