using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    public Transform wallsContainer;
    public Tile[,] map;

    [HideInInspector]
    public int width;

    [HideInInspector]
    public int height;

    void Start()
    {
        var maxX = 0;
        var maxY = 0;

        foreach (Transform child in wallsContainer)
        {
            var pos = WorldToMap(child.position);

            if (pos.x > maxX)
            {
                maxX = (int)pos.x;
            }

            if (pos.y > maxY)
            {
                maxY = (int)pos.y;
            }
        }

        width = maxX;
        height = maxY;

        map = new Tile[width + 1, height + 1];

        foreach (Transform child in wallsContainer)
        {
            var name = child.name;

            var pos = WorldToMap(child.position);

            child.name = string.Format("{0}|{1} : {2}", (int)pos.x, (int)pos.y, Tile.CleanName(name));

            try
            {
                map[(int)pos.x, (int)pos.y] = Tile.FromName(name);
            }
            catch (System.Exception)
            {
                Debug.Log(string.Format("ERROR: {0}|{1}", (int)pos.x, (int)pos.y));
            }

        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool CanMove(Vector2 curPosition, Direction moveDir)
    {

        var pos = WorldToMap(curPosition);

        var x = (int)pos.x;
        var y = (int)pos.y;

        switch (moveDir)
        {
            case Direction.up:
                if (map[x, y].top)
                {
                    return false;
                }

                if (y >= height)
                {
                    return false;
                }

                if (map[x, y + 1].bottom)
                {
                    return false;
                }
                break;
            case Direction.down:
                if (map[x, y].bottom)
                {
                    return false;
                }

                if (y == 0)
                {
                    return false;
                }

                if (map[x, y - 1].top)
                {
                    return false;
                }
                break;
            case Direction.left:
                if (map[x, y].left)
                {
                    return false;
                }

                if (x == 0)
                {
                    return false;
                }

                if (map[x - 1, y].right)
                {
                    return false;
                }

                break;
            case Direction.right:
                if (map[x, y].right)
                {
                    return false;
                }

                if (x >= width)
                {
                    return false;
                }

                if (map[x + 1, y].left)
                {
                    return false;
                }

                break;

        }

        return true;
    }



    public static float snapValue = 1;
    private static Vector2 gridPos;
    public static Vector2 WorldToMap(Vector2 pos)
    {
        float snapInverse = 1 / snapValue;

        // if snapValue = .5, x = 1.45 -> snapInverse = 2 -> x*2 => 2.90 -> round 2.90 => 3 -> 3/2 => 1.5
        // so 1.45 to nearest .5 is 1.5
        gridPos.x = Mathf.Round(pos.x * snapInverse) / snapInverse;
        gridPos.y = Mathf.Round(pos.y * snapInverse) / snapInverse;

        return gridPos;
    }
}
