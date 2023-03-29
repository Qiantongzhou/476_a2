using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class GenericMaze<TmazeObject>
{
    public event EventHandler<OnMazeValueChangedEventArgs> onMazeValueChanged;
    public class OnMazeValueChangedEventArgs : EventArgs {
        public int x;
        public int y;
    }


    public int width;
    public int height;
    public GameObject[] blocks;
    public int planeoffset;
    public TmazeObject[,] genericmaze;
    private TextMesh[,] debugtextarray;

    public GenericMaze(int width,int height, int planeoffset,Func<GenericMaze<TmazeObject>,int ,int,TmazeObject >createMazeObject)
    {
        this.width = width;
        this.height = height;
        this.planeoffset = planeoffset;
        genericmaze = new TmazeObject[width, height];
        for (int i = 0; i < genericmaze.GetLength(0); i++)
        {
            for (int j = 0; j < genericmaze.GetLength(1); j++)
            {
                genericmaze[i, j] = createMazeObject(this, i, j);
                
            }
        }
        debugtextarray=new TextMesh[width,height];
        for (int i = 0; i < genericmaze.GetLength(0); i++)
        {
            for (int j = 0; j < genericmaze.GetLength(1); j++)
            {
                debugtextarray[i, j] = textmaze.CreateMazeText(genericmaze[i, j]?.ToString(), null, getWorldPosition(i, j), 20, Color.white, TextAnchor.MiddleCenter);
                Debug.DrawLine(getWorldPosition(i, j), getWorldPosition(i, j + 1), Color.white, 100f);
                Debug.DrawLine(getWorldPosition(i, j), getWorldPosition(i + 1, j), Color.white, 100f);
               
            }
        }
        Debug.DrawLine(getWorldPosition(0, genericmaze.GetLength(1)), getWorldPosition(genericmaze.GetLength(0), genericmaze.GetLength(1)), Color.white, 100f);
        Debug.DrawLine(getWorldPosition(genericmaze.GetLength(0), 0), getWorldPosition(genericmaze.GetLength(0), genericmaze.GetLength(1)), Color.white, 100f);
        onMazeValueChanged += (object sender, OnMazeValueChangedEventArgs eventArgs) =>
        {
            debugtextarray[eventArgs.x, eventArgs.y].text = genericmaze[eventArgs.x, eventArgs.y]?.ToString();
        };
    }
    private Vector3 getWorldPosition(int x, int y)
    {
        return new Vector3(x, 0, y) * planeoffset;
    }


    public void getXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition.x + (planeoffset / 2)) / planeoffset);
        y = Mathf.FloorToInt((worldPosition.z + (planeoffset / 2)) / planeoffset);
    }
    public void SetValue(int x, int y, TmazeObject value)
    {
        if (x >= 0 && y >= 0 && x < genericmaze.GetLength(0) && y < genericmaze.GetLength(1))
        {
            genericmaze[x, y] = value;
            if(onMazeValueChanged != null) onMazeValueChanged(this,new OnMazeValueChangedEventArgs { x=x, y=y });
        }
    }
    public void SetValue(Vector3 worldposition, TmazeObject value)
    {
        int x, y;
        getXY(worldposition, out x, out y);
        

        SetValue(x, y, value);
    }
    public void TriggerMazeObjectChanged(int x, int y)
    {
        if(onMazeValueChanged!=null) onMazeValueChanged(this,new OnMazeValueChangedEventArgs { x=x, y=y });
    }
    public TmazeObject GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < genericmaze.GetLength(0) && y < genericmaze.GetLength(1))
        {
            return genericmaze[x, y];
        }
        else
        {
            return default(TmazeObject);
        }
    }
    public TmazeObject GetValue(Vector3 worldPosition)
    {
        int x, y;
        getXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }
}
