using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { up, down, left, right };

public class PlayerMovement : MonoBehaviour {

    public KeyCode moveLeft;
    public KeyCode moveRight;
    public KeyCode moveUp;
    public KeyCode moveDown;

    public float speed;

    public Map map;


    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {

        if(Input.GetKeyUp(moveLeft))
        {
            Move(Direction.left);
        }

        if (Input.GetKeyUp(moveRight))
        {
            Move(Direction.right);
        }

        if (Input.GetKeyUp(moveUp))
        {
            Move(Direction.up);
        }

        if (Input.GetKeyUp(moveDown))
        {
            Move(Direction.down);
        }
    }

    public void Move(Direction dir)
    {
        var pos = transform.position;
        var rot = transform.rotation;

        switch (dir)
        {
            case Direction.up:
                pos.y += 1;
                rot = Quaternion.Euler(0, 0, 0);
                break;
            case Direction.down:
                pos.y -= 1;
                rot = Quaternion.Euler(0, 0, 180);
                break;
            case Direction.left:
                rot = Quaternion.Euler(0, 0, 90);
                pos.x -= 1;
                break;
            case Direction.right:
                pos.x += 1;
                rot = Quaternion.Euler(0, 0, 270);
                break;
        }

        if (map.CanMove(transform.position, dir))
        {
            transform.position = pos;
        }

        transform.rotation = rot;
    }

    public bool CanMove(Direction dir)
    {
        return false;
    }
}
