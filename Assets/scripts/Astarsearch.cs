using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;



public class Astarsearch
{
    public const int straightcost = 5;
    public const int angluarcost = 7;
    public GenericMaze<AstarNode> maze;

    List<AstarNode> openlist;
    List<AstarNode> closelist;
    public Astarsearch(int width,int height)
    {
        maze = new GenericMaze<AstarNode>(width, height, 10, (GenericMaze<AstarNode> TemplateContainer, int x, int y) => new AstarNode(TemplateContainer, x, y));
    }

    public List<AstarNode> Find(int startx,int starty, int endx, int endy)
    {
        AstarNode startnode = maze.GetValue(startx, starty);
        AstarNode endnode = maze.GetValue(endx, endy);

        openlist= new List<AstarNode> { startnode};
        closelist= new List<AstarNode>();

        for(int x=0; x < Maze.mazegen.GetLength(0); x++)
        {
            for (int y = 0; y < Maze.mazegen.GetLength(1); y++)
            {
                AstarNode tempnode = maze.GetValue(x, y);
                tempnode.g = int.MaxValue;
                tempnode.CaluclateF();
                tempnode.clastnode = null;

            }
        }
        startnode.g = 0;
        startnode.h = distancetocost(startnode, endnode);
        startnode.CaluclateF();

        while(openlist.Count > 0)
        {
            AstarNode cNode = getLowestFCostNode(openlist);
            if (cNode == endnode)
            {
                return calculatepath(endnode);
            }
            openlist.Remove(cNode);
            closelist.Add(cNode);


            foreach(AstarNode nnode in Getneibourlist(cNode))
            {
                if (closelist.Contains(nnode)) continue;
                if (!nnode.iswalkable)
                {
                    closelist.Add(nnode);
                    continue;
                }


                int gtemp=cNode.g+distancetocost(cNode,nnode);
                if (gtemp < nnode.g)
                {
                    nnode.clastnode = cNode;
                    nnode.g= gtemp;
                    nnode.h = distancetocost(nnode, endnode);
                    nnode.CaluclateF() ;
                    if (!openlist.Contains(nnode))
                    {
                        openlist.Add(nnode);
                    }
                }



            }
        }

        return null;
    }

    List<AstarNode> Getneibourlist(AstarNode cNode)
    {
        List<AstarNode> nlist = new List<AstarNode>();
        if (cNode.x - 1 >= 0)
        {
            nlist.Add(maze.GetValue(cNode.x-1,cNode.y));
            if (cNode.y - 1 >= 0)
            {
                nlist.Add(maze.GetValue(cNode.x - 1, cNode.y - 1));
            }
            if (cNode.y + 1 < maze.height)
            {
                nlist.Add(maze.GetValue(cNode.x - 1, cNode.y + 1));
            }
        }
        if (cNode.x + 1 < maze.width)
        {
            nlist.Add(maze.GetValue(cNode.x + 1, cNode.y));
            if (cNode.y - 1 >= 0)
            {
                nlist.Add(maze.GetValue(cNode.x + 1, cNode.y - 1));
            }
            if (cNode.y + 1 < maze.height)
            {
                nlist.Add(maze.GetValue(cNode.x + 1, cNode.y + 1));
            }
        }
        if (cNode.y - 1 >= 0)
        {
            nlist.Add(maze.GetValue(cNode.x, cNode.y-1));
        }
        if (cNode.y + 1 < maze.height)
        {
            nlist.Add(maze.GetValue(cNode.x, cNode.y + 1));
        }
        return nlist;
    }
    List<AstarNode> calculatepath(AstarNode endnode)
    {
        List<AstarNode> plist = new List<AstarNode>();
        plist.Add(endnode);
        AstarNode cn = endnode;
        while (cn.clastnode != null)
        {
            plist.Add(cn.clastnode);
            cn= cn.clastnode;
        }
        plist.Reverse();
        return plist;
    }
    int distancetocost(AstarNode a,AstarNode b)
    {
        int dx = Mathf.Abs(a.x - b.x);
        int dy = Mathf.Abs(a.y - b.y);
        int remain=Mathf.Abs(dx-dy);
        return angluarcost*Mathf.Min(dx,dy)+straightcost*remain;
    }
    AstarNode getLowestFCostNode(List<AstarNode> nodelist)
    {
        AstarNode lowest = nodelist[0];
        for (int i = 1; i < nodelist.Count; i++) { 
            if (nodelist[i].f < lowest.f)
            {
                lowest = nodelist[i];
            } }
        return lowest;
    }
}

public class AstarNode
{
    GenericMaze<AstarNode> astar;
    public int x;
    public int y;

    public int g;
    public int h;
    public int f;

    public AstarNode clastnode;
    public bool iswalkable;
    public string textcolor;

    public AstarNode(GenericMaze<AstarNode> astar, int x, int y)
    {
        this.astar = astar;
        this.x = x;
        this.y = y;
        iswalkable = true;
        textcolor = "white";
    }

    public void CaluclateF()
    {
        f=g+h;
    }
    public void changecolor(string color)
    {
        textcolor = color;
        astar.TriggerMazeObjectChanged(x, y);
    }
    public override string ToString()
    {
        return "<color="+textcolor+">"+x+" "+y+"</color>";
        
    }
}