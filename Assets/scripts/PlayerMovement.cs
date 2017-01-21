using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { up, down, left, right, none };

public class PlayerMovement : MonoBehaviour {

    [Header("Control Keys")]
    public KeyCode moveLeft;
    public KeyCode moveRight;
    public KeyCode moveUp;
    public KeyCode moveDown;

    [Header("Move Settings")]
    [Range(0f, 1f)]
    public float moveSpeed;
    [Range(0f, 1f)]
    public float rotationSpeed;

    // does not show on inspector
    public Map map;

    private Vector3 nextPos;
    private Quaternion nextRot;

    private Direction moveDir;
    private float moveTimer;
    private float rotationTimer;


    // Use this for initialization
    void Start () {
        moveDir = Direction.none;
    }
	
	// Update is called once per frame
	void Update () {

        if(moveDir != Direction.none)
        {
            return;
        }

        if(Input.GetKey(moveLeft))
        {
            Move(Direction.left);
        }

        if (Input.GetKey(moveRight))
        {
            Move(Direction.right);
        }

        if (Input.GetKey(moveUp))
        {
            Move(Direction.up);
        }

        if (Input.GetKey(moveDown))
        {
            Move(Direction.down);
        }
    }

    public void FixedUpdate()
    {
        if(moveTimer > 0f)
        {
            var pos = Vector3.Lerp(nextPos, transform.position, moveTimer);
            transform.position = pos;
            moveTimer -= Time.fixedDeltaTime;
        }
        else
        {
            moveDir = Direction.none;
        }

        if(rotationTimer > 0f)
        {
            var rot = Quaternion.Lerp(nextRot, transform.rotation, rotationTimer);
            transform.rotation = rot;
            rotationTimer -= Time.fixedDeltaTime;
        }
    }

    public void Move(Direction dir)
    {
        var pos = transform.position;
        nextPos = pos;
        switch (dir)
        {
            case Direction.up:
                nextPos.y += 1;
                nextRot = Quaternion.Euler(0, 0, 0);
                break;
            case Direction.down:
                nextPos.y -= 1;
                nextRot = Quaternion.Euler(0, 0, 180);
                break;
            case Direction.left:
                nextPos.x -= 1;
                nextRot = Quaternion.Euler(0, 0, 90);
                break;
            case Direction.right:
                nextPos.x += 1;
                nextRot = Quaternion.Euler(0, 0, 270);
                break;
        }

        if (map.CanMove(transform.position, dir))
        {
            moveTimer = moveSpeed;
        }

        rotationTimer = rotationSpeed;

        moveDir = dir;
    }

    public bool CanMove(Direction dir)
    {
        return false;
    }
}
