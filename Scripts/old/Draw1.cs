using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw1 : MonoBehaviour {

    float time;
    public float time_out;

    GameObject[,] tiles;
    public GameObject cells_prefab;

    SpriteRenderer[,] sprrnd;
    Tiles tl;

    // Use this for initialization
    void Start()
    {
        tl = GetComponent(typeof(Tiles)) as Tiles;
        init();
        tl.Cell_init();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= time_out)
        {
            draw();
            time = 0f;
        }
    }

    void init()
    {
        tiles = new GameObject[tl.CELL_SIZE_X, tl.CELL_SIZE_Y];
        sprrnd = new SpriteRenderer[tl.CELL_SIZE_X, tl.CELL_SIZE_Y];
        Vector3 tmp_vec = Vector3.back;
        for (int y = 0; y < tl.CELL_SIZE_Y; y++)
        {
            for (int x = 0; x < tl.CELL_SIZE_X; x++)
            {
                tiles[x, y] = Instantiate(cells_prefab);
                sprrnd[x, y] = tiles[x, y].gameObject.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;
                tmp_vec.x = x + transform.position.x;
                tmp_vec.y = y + transform.position.y;
                tiles[x, y].transform.position = tmp_vec;
            }
        }
    }

    void draw()
    {
        tl.Next_life1();//
        for (int y = 0; y < tl.CELL_SIZE_Y; y++)
        {
            for (int x = 0; x < tl.CELL_SIZE_X; x++)
            {
                if (tl.cells[x, y] == 1)
                {
                    sprrnd[x, y].color = new Color(0, 1f, 0);
                }
                else
                {
                    sprrnd[x, y].color = new Color(1f,1f,1f);
                }

            }
        }
    }
}
