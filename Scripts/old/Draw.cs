using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw : MonoBehaviour {

    float time;
    public float time_out;

    GameObject[,] tiles;
    public GameObject cells_prefab;

    Manager manager;


	// Use this for initialization
	void Start () {
        manager = GetComponent(typeof(Manager)) as Manager;
        manager.tl = GetComponent(typeof(Tiles)) as Tiles;
        init();
        manager.tl.Cell_init();
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        if(time >= time_out)
        {
            draw();
            time = 0f;
        }
	}

    void init()
    {
        tiles = new GameObject[manager.tl.CELL_SIZE_X, manager.tl.CELL_SIZE_Y];
        manager.sprrnd = new SpriteRenderer[manager.tl.CELL_SIZE_X, manager.tl.CELL_SIZE_Y];
        manager.sndev = new Send_Event[manager.tl.CELL_SIZE_X, manager.tl.CELL_SIZE_Y];

        Vector3 tmp_vec = Vector3.back;//initialization
        for(int y = 0; y < manager.tl.CELL_SIZE_Y; y++)
        {
            for(int x = 0; x < manager.tl.CELL_SIZE_X; x++)
            {
                tiles[x, y] = Instantiate(cells_prefab);
                manager.sprrnd[x, y] = tiles[x, y].gameObject.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;
                manager.sndev[x, y] = tiles[x, y].gameObject.GetComponent(typeof(Send_Event)) as Send_Event;

                tmp_vec.x = x + transform.position.x;
                tmp_vec.y = y + transform.position.y;
                tiles[x, y].transform.position = tmp_vec;

            }
        }
    }

    void draw()
    {
        manager.tl.Next_life();
        for(int y = 0;y < manager.tl.CELL_SIZE_Y; y++)
        {
            for(int x = 0;x < manager.tl.CELL_SIZE_X; x++)
            {
                if(manager.tl.cells[x, y] == 1)
                {
                    manager.sprrnd[x, y].color = new Color(0, 1f, 0);
                }
                else
                {
                    manager.sprrnd[x, y].color = new Color(5 * manager.tl.temperature[x, y]/manager.tl.MAX_TEMPERATURE, manager.tl.temperature[x, y]/manager.tl.MAX_TEMPERATURE, manager.tl.temperature[x, y]/manager.tl.MAX_TEMPERATURE);
                }
                
            }
        }
    }
}
