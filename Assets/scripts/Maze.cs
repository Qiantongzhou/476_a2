using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Maze: MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] blocks;
    public static int planeoffset=10;
    public List<GameObject> mazeList = new List<GameObject>();
    GenericMaze<stringmazeobj> maze;
    [HideInInspector]
    public Astarsearch Astarsearch;
    private List<NavMeshSurface> navMeshSurfaces= new List<NavMeshSurface>();

    public static int[,] mazegen ={ 
       //0 ,1 ,2 ,3 ,4 ,5 ,6 ,7 ,8 ,9 ,10,11,12
        {18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18},//0
        {18,2 ,7 ,7 ,7 ,7 ,7 ,7 ,7 ,7 ,7 ,7 ,6 ,7 ,7 ,7 ,7 ,7 ,7 ,7 ,7 ,7 ,7 ,7 ,7 ,7 ,7 ,3 ,18},//1
        {18,8, 0 ,0 ,0 ,0 ,0 ,17,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,17,0 ,0 ,0 ,0 ,0 ,1 ,0 ,0 ,8 ,18},//2
        {18,8, 0 ,0 ,0 ,17,0 ,0 ,0 ,17,0 ,0 ,0 ,17,0 ,17,17,0 ,0 ,0 ,17,0 ,0 ,0 ,0 ,0 ,0 ,8 ,18},//3
        {18,8, 17,0 ,2 ,7 ,7 ,0 ,7 ,7 ,7 ,0 ,7 ,7 ,0 ,7 ,7 ,0 ,7 ,7 ,7 ,7 ,0 ,7 ,0 ,2 ,3 ,8 ,18},//4
        {18,8, 0 ,0 ,8 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,1 ,0 ,0 ,0 ,0 ,0 ,0 ,5 ,4 ,8 ,18},//5
        {18,8, 0 ,17,8 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,15,0 ,17,0 ,1 ,1 ,1 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,8 ,18},//6
        {18,8, 0 ,17,5 ,7 ,7 ,7 ,7 ,7 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,7 ,7 ,7 ,3 ,15,0 ,0 ,8 ,18},//7
        {18,8, 0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,17,0 ,17,17,0 ,15,0 ,0 ,0 ,0 ,0 ,0 ,8 ,0 ,0 ,0 ,8 ,18},//8
        {18,8, 0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,8 ,0 ,0 ,8 ,0 ,0 ,0 ,8 ,18},//9
        {18,13,7 ,7 ,7 ,7 ,9 ,7 ,7 ,7 ,7 ,9 ,7 ,7 ,7 ,7 ,9 ,7 ,7 ,7 ,14,9 ,0 ,5 ,7 ,0 ,7 ,14,18},//10
        {18,8, 0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,8 ,0 ,0 ,0 ,0 ,0 ,0 ,8 ,18},//11
        {18,8, 0 ,0 ,1 ,1 ,1 ,1 ,1 ,1 ,0 ,1 ,0 ,1 ,1 ,1 ,1 ,1 ,1 ,0 ,8 ,0 ,17,0 ,0 ,0 ,0 ,8 ,18},//12
        {18,8, 0 ,0 ,0 ,0 ,0 ,0 ,0 ,1 ,0 ,0 ,0 ,0 ,1 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,1 ,0 ,1 ,1 ,0 ,8 ,18},//13
        {18,8, 0 ,0 ,0 ,1 ,0 ,0 ,0 ,1 ,0 ,0 ,1 ,1 ,1 ,0 ,0 ,0 ,1 ,0 ,8 ,0 ,1 ,0 ,0 ,0 ,0 ,8 ,18},//14
        {18,8, 0 ,0 ,0 ,0 ,1 ,0 ,0 ,0 ,0 ,0 ,1 ,0 ,1 ,0 ,1 ,0 ,0 ,0 ,8 ,0 ,0 ,0 ,16,0 ,0 ,8 ,18},//15
        {18,8, 0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,8 ,0 ,1 ,1 ,1 ,1 ,1 ,8 ,18},//16
        {18,8, 0 ,2 ,7 ,0 ,7 ,7 ,0 ,7 ,7 ,7 ,0 ,7 ,0 ,7 ,7 ,7 ,1 ,0 ,0 ,0 ,0 ,1 ,0 ,0 ,1 ,8 ,18},//17
        {18,8, 0 ,8 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,8 ,0 ,0 ,0 ,0 ,0 ,0 ,8 ,18},//18
        {18,8, 0 ,8 ,0 ,0 ,0 ,1 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,1 ,0 ,1 ,0 ,17,15,0 ,8 ,18},//19
        {18,8, 0 ,5 ,7 ,0 ,0 ,7 ,0 ,7 ,7 ,0 ,7 ,0 ,7 ,7 ,7 ,7 ,3 ,0 ,0 ,0 ,1 ,1 ,0 ,0 ,0 ,8 ,18},//20
        {18,8, 0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,8 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,8 ,18},//21
        {18,13,7 ,7 ,7 ,7 ,7 ,0 ,7 ,7 ,7 ,7 ,0 ,7 ,0 ,7 ,6 ,7 ,4 ,0 ,2 ,9 ,7 ,9 ,7 ,7 ,7 ,14,18},//22
        {18,8, 0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,8 ,18},//23
        {18,8, 0 ,1 ,0 ,1 ,1 ,0 ,1 ,1 ,1 ,0 ,0 ,0 ,1 ,0 ,0 ,1 ,0 ,0 ,8 ,0 ,1 ,1 ,1 ,1 ,1 ,8 ,18},//24
        {18,8, 0 ,1 ,0 ,1 ,0 ,0 ,0 ,1 ,0 ,0 ,0 ,1 ,1 ,0 ,1 ,1 ,1 ,0 ,8 ,0 ,0 ,1 ,0 ,0 ,0 ,8 ,18},//25
        {18,8, 0 ,1 ,0 ,0 ,0 ,1 ,0 ,0 ,0 ,0 ,1 ,1 ,1 ,0 ,0 ,1 ,0 ,0 ,8 ,0 ,0 ,1 ,0 ,1 ,1 ,8 ,18},//26
        {18,8, 0 ,1 ,1 ,1 ,1 ,1 ,1 ,0 ,1 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,1 ,0 ,0 ,0 ,0 ,1 ,0 ,8 ,18},//27
        {18,8, 0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,8 ,18},//28
        {18,5, 7 ,7 ,7 ,7 ,7 ,7 ,7 ,7 ,7 ,7 ,7 ,7 ,7 ,7 ,7 ,7 ,7 ,7 ,7 ,7 ,7 ,7 ,7 ,7 ,7 ,4 ,18},//29
        {18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18,18} //30
    };              


    private TextMesh[,] debugtextarray = new TextMesh[mazegen.GetLength(0), mazegen.GetLength(1)];
    private void Start()
    {
        //genmaze();

        //maze = new GenericMaze<stringmazeobj>(mazegen.GetLength(0), mazegen.GetLength(1), planeoffset, (GenericMaze<stringmazeobj> tm, int x, int y) => new stringmazeobj(tm, x, y));
        Astarsearch = new Astarsearch(Maze.mazegen.GetLength(0), Maze.mazegen.GetLength(1));


        for (int i = 0; i < Maze.mazegen.GetLength(0); i++)
        {
            for (int j = 0; j < Maze.mazegen.GetLength(1); j++)
            {

                if (walkablepath().Contains(Maze.mazegen[i, j]))
                {
                    Astarsearch.maze.GetValue(i, j).iswalkable = true;

                }
                else
                if (!walkablepath().Contains(Maze.mazegen[i, j]))
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

    public int[] walkablepath()
    {
        int[] temp = {0,6,9};
        return temp;
    }

    [ContextMenu("gen-maze")]
    public void genmaze()
    {
        
        
        for (int i = 0; i < mazegen.GetLength(0); i++)
        {
            for (int j = 0; j < mazegen.GetLength(1); j++)
            {
                //debugtextarray[i,j]= textmaze.CreateMazeText(mazegen[i, j].ToString(), transform, getWorldPosition(i, j), 20, Color.white, TextAnchor.MiddleCenter);
                Debug.DrawLine(getWorldPosition(i, j), getWorldPosition(i, j + 1), Color.white, 100f);
                Debug.DrawLine(getWorldPosition(i, j), getWorldPosition(i + 1, j), Color.white, 100f);
                for (int k = 0; k < blocks.Length; k++)
                {
                    if (mazegen[i, j] == k)
                    {
                        GameObject temp = Instantiate(blocks[k], transform);
                        temp.AddComponent<NavMeshSurface>();
                        //temp.transform.position = new Vector3(j * planeoffset, 0, (mazegen.GetLength(0)-1) * planeoffset - i * planeoffset);
                        temp.transform.position = new Vector3(i * planeoffset, 0, j * planeoffset);
                        if(
                            i==mazegen.GetLength(0)-1
                            || i == mazegen.GetLength(0) - 2
                            || i == mazegen.GetLength(0) - 3
                            || i == 0
                            || i == 1
                            || i == 2
                            || j == mazegen.GetLength(1) - 1
                            || j == mazegen.GetLength(1) - 2
                            || j == mazegen.GetLength(1) - 3
                            || j == 0
                            || j == 1
                            || j == 2
                            )
                        {
                            SetLayer(temp, 0);
                        }
                        mazeList.Add(temp);
                    }

                }
                
            }
        }
        Debug.DrawLine(getWorldPosition(0, mazegen.GetLength(1)), getWorldPosition(mazegen.GetLength(0), mazegen.GetLength(1)), Color.white, 100f);
        Debug.DrawLine(getWorldPosition(mazegen.GetLength(0), 0), getWorldPosition(mazegen.GetLength(0), mazegen.GetLength(1)), Color.white, 100f);
        
            
            navMeshSurfaces.Add(mazeList.Last().GetComponent<NavMeshSurface>());
            
            
            navMeshSurfaces.Last().BuildNavMesh();
        
        GenerateNavMesh();
    }
    void SetLayer(GameObject obj, int layer)
    {
        obj.layer = layer;

        // Iterate through the child objects and set their layers recursively
        foreach (Transform child in obj.transform)
        {
            SetLayer(child.gameObject, layer);
        }
    }
    private Vector3 getWorldPosition(int x, int y)
    {
        return new Vector3(y * planeoffset, 0, (mazegen.GetLength(0) - 1) * planeoffset - x * planeoffset) ;
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
    private void GenerateNavMesh()
    {
        
    }

    

    public static void getXY(Vector3 worldPosition, out int x, out int y)
    {

        x = Mathf.FloorToInt((worldPosition.x+(planeoffset/2)) / planeoffset);
        y = Mathf.FloorToInt((worldPosition.z+(planeoffset/2)) / planeoffset);
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
