using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    public GameObject player_obj;

    public Tiles tl;
    public Draw draw;
    public Player player;
    public Send_Event[,] sndev;
    public SpriteRenderer[,] sprrnd;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        Click_Event();
	}

    void Click_Event()
    {
        for (int _x = 0; _x < tl.CELL_SIZE_X; _x++)
        {
            for (int _y = 0; _y < tl.CELL_SIZE_Y; _y++)
            {
                if(sndev[_x, _y].fire == true)
                {
                    Debug.Log("addedtmpr");
                    tl.Trans_tempereture(tl.cells_buf, _x, _y, -tl.D_TEMPERETURE * 40, tl.DECAY_DIR);
                    sndev[_x, _y].fire = false;
                }
            }
        }
    }
}
