using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Maze: MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] blocks;
    public int planeoffset;
    List<GameObject> mazeList = new List<GameObject>();
    GenericMaze<stringmazeobj> maze;
    [HideInInspector]
    public Astarsearch Astarsearch;

    public static int[,] mazegen ={ 
       //1,2,3,4,5,6,7,8,9,
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        {1,0,0,0,1,0,0,0,1,0,0,0,0,0,0,0,1},
        {1,0,0,0,1,0,1,0,1,0,1,1,1,0,0,0,1},
        {1,0,0,0,0,0,1,0,0,0,0,0,1,1,1,0,1},
        {1,1,0,1,1,0,1,1,1,1,1,0,0,0,0,0,1},
        {1,0,0,0,1,0,1,0,0,0,0,0,1,0,0,0,1},
        {1,0,0,0,1,0,0,0,0,0,1,0,1,1,1,1,1},
        {0,0,1,0,0,0,1,0,1,0,1,0,0,0,0,0,0},
        {1,1,1,1,1,0,1,0,1,1,1,1,1,0,0,0,1},
        {1,0,0,0,1,0,1,0,0,0,1,0,1,0,0,0,1},
        {1,0,0,0,1,0,0,0,1,0,0,0,1,0,0,0,1},
        {1,0,0,0,0,0,1,0,1,1,0,0,0,0,0,0,1},
        {1,0,1,1,0,1,1,1,1,1,0,1,1,1,0,1,1},
        {1,0,1,0,0,0,1,0,0,0,0,0,1,0,0,0,1},
        {1,0,0,0,1,0,0,0,0,0,1,0,0,0,0,0,1},
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
    };


    private TextMesh[,] debugtextarray = new TextMesh[mazegen.GetLength(0), mazegen.GetLength(1)];
    private void Start()
    {


        // maze = new GenericMaze<stringmazeobj>(mazegen.GetLength(0), mazegen.GetLength(1), planeoffset, (GenericMaze<stringmazeobj> TM, int x, int y) => new stringmazeobj(TM, x, y));
        Astarsearch = new Astarsearch(Maze.mazegen.GetLength(0), Maze.mazegen.GetLength(1));


        for (int i = 0; i < Maze.mazegen.GetLength(0); i++)
        {
            for (int j = 0; j < Maze.mazegen.GetLength(1); j++)
            {

                if (Maze.mazegen[i, j] == 0)
                {
                    Astarsearch.maze.GetValue(i, j).iswalkable = true;

                }
                else
                if (Maze.mazegen[i, j] == 1)
                {
                    Astarsearch.maze.GetValue(i, j).iswalkable = false;
                    Astarsearch.maze.GetValue(i, j).changecolor("green");
                }
            }
        }
    }
    private void Update()
    {
       // Vector3 position=textmaze.GetMouseWorldPosition();
       // if (Input.GetKeyDown(KeyCode.K))
       // {
       //     maze.GetValue(position).AddLetter("A");
       // }
    }



    [ContextMenu("gen-maze")]
    public void genmaze()
    {
        
        for (int i = 0; i < mazegen.GetLength(0); i++)
        {
            for (int j = 0; j < mazegen.GetLength(1); j++)
            {
                debugtextarray[i,j]= textmaze.CreateMazeText(mazegen[i, j].ToString(), transform, getWorldPosition(i, j), 20, Color.white, TextAnchor.MiddleCenter);
                Debug.DrawLine(getWorldPosition(i, j), getWorldPosition(i, j + 1), Color.white, 100f);
                Debug.DrawLine(getWorldPosition(i, j), getWorldPosition(i + 1, j), Color.white, 100f);
                if (mazegen[i, j] == 0)
                {
                    GameObject temp = Instantiate(blocks[0], transform);

                    temp.transform.position = new Vector3(i * planeoffset, 0, j * planeoffset);
                    mazeList.Add(temp);
                } else
                if (mazegen[i, j] == 1)
                {
                    GameObject temp = Instantiate(blocks[1], transform);
                    temp.transform.position = new Vector3(i * planeoffset, 0, j* planeoffset);
                    mazeList.Add(temp);
                }
            }
        }
        Debug.DrawLine(getWorldPosition(0, mazegen.GetLength(0)+1), getWorldPosition(mazegen.GetLength(0), mazegen.GetLength(1)), Color.white, 100f);
        Debug.DrawLine(getWorldPosition(mazegen.GetLength(1)-1, 0), getWorldPosition(mazegen.GetLength(0), mazegen.GetLength(1)), Color.white, 100f);
    }
    private Vector3 getWorldPosition(int x, int y)
    {
        return new Vector3(y, 0, x) * planeoffset;
    }
    [ContextMenu("remove-maze")]
    public void removemaze()
    {
        foreach (GameObject temp in mazeList)
        {
            DestroyImmediate(temp);
        }

        foreach (TextMesh temp in GetComponentsInChildren<TextMesh>())
        {
            DestroyImmediate(temp.gameObject);
        }
    }

    public void getXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition.z+(planeoffset/2)) / planeoffset);
        y = Mathf.FloorToInt((worldPosition.x+(planeoffset/2)) / planeoffset);
    }
    public void SetValue(int x, int y, int value)
    {
        if (x >= 0 && y >= 0 && x < mazegen.GetLength(0) && y < mazegen.GetLength(1))
        {
            mazegen[x, y] = value;
            debugtextarray[x, y].text = mazegen[x, y].ToString();
        }
    }
    public void SetValue(Vector3 worldposition, int value)
    {
        int x, y;
        getXY(worldposition, out x, out y);
        print(x + " " + y);
        
        SetValue(x, y, value);
    }
    public int GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < mazegen.GetLength(0) && y < mazegen.GetLength(1))
        {
            return mazegen[x, y];
        }
        else
        {
            return -1;
        }
    }
    public int GetValue(Vector3 worldPosition)
    {
        int x, y;
        getXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }
}
public class stringmazeobj
{
    private GenericMaze<stringmazeobj> maze;
    int x;
    int y;

    string letters;
    string numbers;
    public stringmazeobj(GenericMaze<stringmazeobj> maze, int x, int y)
    {
        this.maze = maze;
        this.x = x;
        this.y = y;
        this.letters = "1";
        this.numbers = "2";
    }
    public void AddLetter(string letter)
    {
        letters += letter;
        maze.TriggerMazeObjectChanged(x, y);

    }
    public override string ToString()
    {
        return letters+"\n"+numbers;
    }
}


public static class textmaze
{
    public static TextMesh CreateMazeText(string text,Transform parent=null, Vector3 localPosition=default(Vector3), int fontSize=40, Color? color=null, TextAnchor textAnchor=TextAnchor.UpperLeft, TextAlignment textAlignment=TextAlignment.Left, int sortingOrder=5000)
    {
        if(color==null) color=Color.white;
        return CreateMazeText(parent,text,localPosition,fontSize,(Color)color,textAnchor,textAlignment,sortingOrder);
    }
    public static TextMesh CreateMazeText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color,TextAnchor textAnchor,TextAlignment textAlignment,int sortingOrder)
    {
        GameObject gameObject = new GameObject("World_Text",typeof(TextMesh));
        Transform transform= gameObject.transform;
        transform.SetParent(parent,false);
        transform.localPosition = localPosition;
        
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.color = color;
        textMesh.fontSize= fontSize;
        textMesh.GetComponent<MeshRenderer>().sortingOrder=sortingOrder;
        return textMesh;
    }

    public static Vector3 GetMouseWorldPosition()
    {
        Plane plane = new Plane(Vector3.down, 0);
        Vector3 worldposition = new Vector3();
          Ray ray=   Camera.main.ScreenPointToRay(Input.mousePosition);
        if(plane.Raycast(ray,out float distance))
        {
           worldposition = ray.GetPoint(distance);
        }
        else
        {
            worldposition = new Vector3(-1,-1,-1);
        }
        
        return worldposition;
    }

}
