using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiles : MonoBehaviour {

    public int CELL_SIZE_X;
    public int CELL_SIZE_Y;
    public int DENOMINATOR;
    public int MAX_TEMPERATURE;
    public int D_TEMPERETURE;
    public int DECAY;//temperature's decay rate(@DECAY = 10 , rate = 1.0)
    public int TRANS_DIR;
    public int DECAY_DIR;
    public int INIT_PROB;

    public int[,] cells;
    public int[,] cells_buf;
    public int[,] temperature;
    
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Cell_init()
    {
        cells = new int[CELL_SIZE_X, CELL_SIZE_Y];
        cells_buf = new int[CELL_SIZE_X, CELL_SIZE_Y];
        temperature = new int[CELL_SIZE_X, CELL_SIZE_Y];
        int rnd;

        for(int x = 0; x < CELL_SIZE_X; x++)
        {
            for(int y = 0; y < CELL_SIZE_Y; y++)
            {
                rnd = UnityEngine.Random.Range(0, 100);
                if(rnd < INIT_PROB)
                {
                    cells[x, y] = 1;
                }
                else
                {
                    cells[x, y] = 0;
                }
                
                
                temperature[x, y] = 0;
            }
        }
    }

    public void Next_life()
    {
        Array.Copy(cells, cells_buf, CELL_SIZE_X*CELL_SIZE_Y);
        for(int x = 0; x < CELL_SIZE_X; x++)
        {
            for (int y = 0; y < CELL_SIZE_Y; y++)
            {
                Apply_randomrule(cells, cells_buf, x, y);
            }
        }

    }

    public void Next_life1()
    {
        Array.Copy(cells, cells_buf, CELL_SIZE_X * CELL_SIZE_Y);
        for (int x = 0; x < CELL_SIZE_X; x++)
        {
            for (int y = 0; y < CELL_SIZE_Y; y++)
            {
                Apply_rule(cells, cells_buf, x, y);
            }
        }
    }

    void Apply_rule(int [,] cells_in,int [,] buf_in, int x,int y)
    {
        if (Moore_neighborhood(buf_in, x, y) == 2 && buf_in[x, y] == 1) return;
        else if(Moore_neighborhood(buf_in, x, y) == 3)
        {
            if(buf_in[x,y] == 0)
            {
                cells_in[x, y] = 1;
            }
            return;
        }
        cells_in[x, y] = 0;
        return;
    }

    int Moore_neighborhood(int [,] cells_in, int x, int y) {
        int count = 0;
        if (x-1>=0 && y-1>=0 && cells_in[x - 1, y - 1] == 1) count++;
        if (y-1>=0 && cells_in[x, y - 1] == 1) count++;
        if (x+1<CELL_SIZE_X && y-1>=0 && cells_in[x + 1, y - 1] == 1) count++;
        if (x-1>=0 && cells_in[x - 1, y] == 1) count++;
        if (x+1<CELL_SIZE_X && cells_in[x + 1, y] == 1) count++;
        if (x - 1 >= 0 && y+1<CELL_SIZE_Y && cells_in[x - 1, y + 1] == 1) count++;
        if (y+1<CELL_SIZE_Y && cells_in[x, y + 1] == 1) count++;
        if (x+1<CELL_SIZE_X && y+1<CELL_SIZE_Y && cells_in[x + 1, y + 1] == 1) count++;
        return count;
    }

    public void Add_temperature(int x, int y, int d_tmpr)
    {
        if(x >= 0 && x < CELL_SIZE_X && y >= 0 && y < CELL_SIZE_Y)
        {
            temperature[x, y] += d_tmpr;
            if (temperature[x, y] < 0) temperature[x, y] = 0;
            else if (temperature[x, y] > MAX_TEMPERATURE) temperature[x, y] = MAX_TEMPERATURE;
        }
    }

    public void Trans_tempereture(int[,] cells_in,int x, int y, int d_tmpr, int range)
    {
        for(int _x = -range;_x <= range; _x++)
        {
            for(int _y = -range;_y <= range; _y++)
            {
                Add_temperature(x + _x, y + _y, d_tmpr * (1-cells_in[x,y]));
            }
        }
    }

    void Decay_temperature(int[,] cells_in,int x, int y, int d_tmpr, int range)
    {
        for (int _x = -range; _x <= range; _x++)
        {
            for (int _y = -range; _y <= range; _y++)
            {
                Add_temperature(x + _x, y + _y, d_tmpr * cells_in[x, y]);
            }
        }
    }

    void Apply_randomrule(int[,] cells_in, int[,] buf_in, int x, int y)
    {
        if (Moore_neighborhood(buf_in, x, y) == 2 && buf_in[x, y] == 1)
        {
            Decay_temperature(buf_in,x, y, -D_TEMPERETURE * DECAY / 10, DECAY_DIR);
            Random_life(cells_in, x, y, DENOMINATOR - temperature[x, y]);
            return;
        }
        else if (Moore_neighborhood(buf_in, x, y) == 3)
        {
            Decay_temperature(buf_in,x, y, -D_TEMPERETURE * DECAY / 10, DECAY_DIR);
            Random_life(cells_in, x, y, DENOMINATOR - temperature[x, y]);
            return;
        }
        else if (buf_in[x, y] == 1)
        {
            Random_life(cells_in, x, y, temperature[x, y]);
            Trans_tempereture(cells_in,x, y, D_TEMPERETURE, TRANS_DIR);
            return;
        }

        Random_life(cells_in, x, y, temperature[x, y]);
        if(cells[x, y] == 1)
        {
            Decay_temperature(buf_in,x, y, -D_TEMPERETURE * DECAY^2, DECAY_DIR);
        }
        else
        {
            Decay_temperature(buf_in,x, y, -D_TEMPERETURE * DECAY / 10, DECAY_DIR);
        }
        return;
    }

    void Random_life(int[,] cells_in, int x, int y, int prob)
    {
        int rnd = UnityEngine.Random.Range(0, DENOMINATOR);
        if(rnd < prob)
        {
            cells_in[x, y] = 1;
        }
        else
        {
            cells_in[x, y] = 0;
        }
    }

    int Get_cell(int[,] cells_in, int x, int y) 
    {
        if (x >= 0 && x < CELL_SIZE_X && y >= 0 && y < CELL_SIZE_Y)
        {
            return cells_in[x, y];
        }
        else
        {
            return 0;
        }
    }

}
