using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    [Header("Grid Size")]
    public int gridX;
    public int gridY;

    [Header("Grid Settings")]
    public float updateGridRate = 1.0f;
    [Range(0f,1f)]
    public float heatTransfer;

    private Cell[,] grid;
    private Cell[,] next;

    private float north, south, east, west;
    private float updateGridTimer;

	// Use this for initialization
	void Start () {

        InitGrids();
        updateGridTimer = 0f;
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        if(updateGridTimer <= 0)
        {
            updateGridTimer = updateGridRate;
            UpdateGrid();
        }

        updateGridTimer -= Time.fixedDeltaTime;
    }

    public void InitGrids()
    {
        grid = new Cell[gridX, gridY];
        next = new Cell[gridX, gridY];

        for (int x = 0; x < gridX; x++)
        {
            for (int y = 0; y < gridY; y++)
            {
                grid[x, y] = new Cell() { heat = 0f };
                next[x, y] = new Cell() { heat = 0f };
            }
        }
    }

    public void UpdateGrid()
    {
        //calculate next grid heat values
        for(int x = 0; x < gridX; x++)
        {
            for (int y = 0; y < gridY; y++)
            {
                next[x, y].heat = AverageHeat(x, y);
            } 
        }

        // update grid heat values
        for (int x = 0; x < gridX; x++)
        {
            for (int y = 0; y < gridY; y++)
            {
                grid[x,y].heat = next[x, y].heat;
            }
        }
    }

    public float AverageHeat(int x, int y)
    {
        north = 0;
        south = 0;
        west = 0;
        east = 0;

        //heat north
        if(y > 0)
        {
            north = grid[x, y - 1].heat;
        }

        //heat south
        if(y < gridY)
        {
            south = grid[x, y + 1].heat;
        }

        //heat west
        if(x > 0)
        {
            west = grid[x - 1, y].heat;
        }

        //heat east
        if(x < gridX)
        {
            east = grid[x + 1, y].heat;
        }

        return heatTransfer * (north + south + east + west);
    }
}

public class Cell
{
    public float heat;
}
