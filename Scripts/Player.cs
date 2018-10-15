using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private enum Direction { up, down, right, left};

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Get_Key();
	}

    void Move(Direction direction)
    {
        Vector3 tmp_vec;
        switch (direction)
        {
            case Direction.up:
                tmp_vec = transform.position;
                tmp_vec.y += 1;
                transform.position = tmp_vec;
                break;

            case Direction.down:
                tmp_vec = transform.position;
                tmp_vec.y -= 1;
                transform.position = tmp_vec;
                break;

            case Direction.right:
                tmp_vec = transform.position;
                tmp_vec.x += 1;
                transform.position = tmp_vec;
                break;

            case Direction.left:
                tmp_vec = transform.position;
                tmp_vec.x -= 1;
                transform.position = tmp_vec;
                break;
        }

            
    }

    void Get_Key()
    {
        if (Input.GetKeyDown(KeyCode.W)){
            Move(Direction.up);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Move(Direction.down);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Move(Direction.right);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            Move(Direction.left);
        }
    }
}
