using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiles2 : MonoBehaviour
{
    //member
    public int CELL_SIZE_X;
    public int CELL_SIZE_Y;
    public int DENOMINATOR;//used for random initialization
    public int MAX_TEMPERATURE;//not in use
    public int D_TEMPERETURE;// not in use
    public int TRANS_DIR;//not in use
    public int DECAY_DIR;//not in use
    public int INIT_PROB;
    public int INIT_PROB2;
    public int NUMBER_OF_STATES;//number of states cells can be
    public int INUSE_STATES;
    public int INUSE_STATES2;
    public int TURRET_STATE;//decide which state be turret

    //int resource;
    //int resource2;
    public int[] resources;//[0]=resource,[1]=resource2

    int STATES_DATA_NUM;

    public int range;//kari

    public int[,] cells;
    public int[,] cells_buf;
    public int[,] cells_another;
    public int[,] cells_buf_another;

    public List<int[,]> cells_list;//[0]=cells,[1]=cells_another
    public List<int[,]> buf_list;//[0]=cells_buf,[1]=cells_buf_another

    public int[,] temperature;//not in use

    string[,] rule_parameter_tmp;//states number, target, evaluate, eval_value, return_state
    string[,] rule_parameter_tmp2;

    string[,] states_parameter_tmp;
    string[,] states_parameter_tmp2;



    List<Rule>[] rules_list;
    List<Rule>[] rules_list_another;

    State_data[] states_list;
    State_data[] states_list2;


    Resources_Loader loader;

    enum Evaluation
    {
        equals, more, less, not_equals, always//more, less includes equals
    };

    enum Cell_function
    {
        none, turretU, turretD, turretR, turretL
    };

    struct Rule
    {
        public int target;  // 6  7  8  moore_neighborhood = 9
                            // 3  4  5
                            // 0  1  2
        public Evaluation evaluate;//strucutre of evaluation like =, <, !=
        public int option;//argument for evalation
        public int eval_value;//
        public int return_state;//
    }

    struct State_data
    {
        public int cost;
        public int strength;
        public Cell_function func;
    }


    // methods
    void Start()
    {
        loader = GetComponent(typeof(Resources_Loader)) as Resources_Loader;//string -> enum no hennkann
        Cell_init();
        Init_rules();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(resources[0]);
        Debug.Log(resources[1]);
    }

    public void Cell_init()//initialization
    {
        cells = new int[CELL_SIZE_X, CELL_SIZE_Y];
        cells_buf = new int[CELL_SIZE_X, CELL_SIZE_Y];
        cells_another = new int[CELL_SIZE_X, CELL_SIZE_Y];
        cells_buf_another = new int[CELL_SIZE_X, CELL_SIZE_Y];

        cells_list = new List<int[,]>();
        buf_list = new List<int[,]>();
        cells_list.Add(cells);
        cells_list.Add(cells_another);
        buf_list.Add(cells_buf);
        buf_list.Add(cells_buf_another);


        temperature = new int[CELL_SIZE_X, CELL_SIZE_Y];

        for (int x = 0; x < CELL_SIZE_X; x++)
        {
            for (int y = 0; y < CELL_SIZE_Y; y++)
            {
                Random_life1(cells, x, y, INIT_PROB, UnityEngine.Random.Range(1, INUSE_STATES));
                Random_life1(cells_another, x, y, INIT_PROB2, UnityEngine.Random.Range(1, INUSE_STATES2));
                temperature[x, y] = 0;
                Cell_collision(cells, cells_another, x, y);
            }
        }
    }

    void Init_rules() //initialization
    {
        rule_parameter_tmp = loader.rule_parameters;
        rule_parameter_tmp2 = loader.rule_parameters2;
        states_parameter_tmp = loader.states_parameters;
        states_parameter_tmp2 = loader.states_parameters2;

        rules_list = new List<Rule>[NUMBER_OF_STATES];
        rules_list_another = new List<Rule>[NUMBER_OF_STATES];
        states_list = new State_data[NUMBER_OF_STATES];//
        states_list2 = new State_data[NUMBER_OF_STATES];



        for(int i = 0;i < NUMBER_OF_STATES;i++)
        {
            rules_list[i] = new List<Rule>();
            rules_list_another[i] = new List<Rule>();

            states_list[i] = new State_data {cost = Int32.Parse(states_parameter_tmp[i, 0]), strength = Int32.Parse(states_parameter_tmp[i, 1]), func = (Cell_function)Enum.Parse(typeof(Cell_function), states_parameter_tmp[i, 2])};
            states_list2[i] = new State_data { cost = Int32.Parse(states_parameter_tmp2[i, 0]), strength = Int32.Parse(states_parameter_tmp2[i, 1]), func = (Cell_function)Enum.Parse(typeof(Cell_function), states_parameter_tmp2[i, 2]) };

        }

        for (int i = 0; i < rule_parameter_tmp.GetLength(0); i++)
        {
            rules_list[Int32.Parse(rule_parameter_tmp[i, 0])].Add(new Rule {target = Int32.Parse(rule_parameter_tmp[i, 1]), evaluate = (Evaluation)Enum.Parse(typeof(Evaluation), rule_parameter_tmp[i, 2]), option = Int32.Parse(rule_parameter_tmp[i, 3]), eval_value = Int32.Parse(rule_parameter_tmp[i, 4]), return_state = Int32.Parse(rule_parameter_tmp[i, 5]) });
        }

        for(int i = 0; i < rule_parameter_tmp2.GetLength(0); i++)
        {
            rules_list_another[Int32.Parse(rule_parameter_tmp2[i, 0])].Add(new Rule { target = Int32.Parse(rule_parameter_tmp2[i, 1]), evaluate = (Evaluation)Enum.Parse(typeof(Evaluation), rule_parameter_tmp2[i, 2]), option = Int32.Parse(rule_parameter_tmp2[i, 3]), eval_value = Int32.Parse(rule_parameter_tmp2[i, 4]), return_state = Int32.Parse(rule_parameter_tmp2[i, 5]) });

        }
    }

    public void Next_life1()
    {
        Array.Copy(cells, cells_buf, CELL_SIZE_X * CELL_SIZE_Y);
        Array.Copy(cells_another, cells_buf_another, CELL_SIZE_X * CELL_SIZE_Y);

        //Turret(cells, cells_buf, cells_another, cells_buf_another, TURRET_STATE);

        for (int x = 0; x < CELL_SIZE_X; x++)
        {
            for (int y = 0; y < CELL_SIZE_Y; y++)
            {
                Apply_rule(0, cells, cells_buf, x, y, rules_list);
                Apply_rule(1, cells_another, cells_buf_another, x, y, rules_list_another);
                Cell_collision(cells, cells_another, x, y);
            }
        }
    }

    void Apply_rule(int player,int[,] cells_in, int[,] buf_in, int x, int y, List<Rule>[] states_in)
    {
        for(int state = 0; state < NUMBER_OF_STATES; state++)
        {
            if(buf_in[x, y] == state)
            {                
                foreach(Rule rule in states_in[state])//
                {
                    switch(rule.evaluate)
                    {
                        case Evaluation.equals:
                            if(rule.target == 9 && Moore_neighborhood1(buf_in, x, y, rule.option) == rule.eval_value)
                            {
                                Generate_cell(player, buf_in[x, y], rule.return_state, x, y);
                            }
                            else if(rule.target <= 8 && rule.target >= 0 && Get_neighbor(buf_in, x, y, rule.target%3 -1, rule.target/3 -1) == rule.eval_value)//
                            {
                                Generate_cell(player, buf_in[x, y], rule.return_state, x, y);
                            }
                            break;

                        case Evaluation.more:
                            if(rule.target == 9 && Moore_neighborhood1(buf_in, x ,y, rule.option) >= rule.eval_value)
                            {
                                Generate_cell(player, buf_in[x, y], rule.return_state, x, y);
                            } else if(rule.target <= 8 && rule.target >= 0 && Get_neighbor(buf_in, x, y, rule.target % 3 - 1, rule.target / 3 - 1) >= rule.eval_value) {
                                Generate_cell(player, buf_in[x, y], rule.return_state, x, y);
                            }
                            break;

                        case Evaluation.less:
                            if (rule.target == 9 && Moore_neighborhood1(buf_in, x, y, rule.option) <= rule.eval_value)
                            {
                                Generate_cell(player, buf_in[x, y], rule.return_state, x, y);
                            } else if (rule.target <= 8 && rule.target >= 0 && Get_neighbor(buf_in, x, y, rule.target % 3 - 1, rule.target / 3 - 1) <= rule.eval_value)
                            {
                                Generate_cell(player, buf_in[x, y], rule.return_state, x, y);
                            }
                            break;
                        case Evaluation.not_equals:
                            if(rule.target == 9 && Moore_neighborhood1(buf_in, x, y, rule.option) != rule.eval_value)
                            {
                                Generate_cell(player, buf_in[x, y], rule.return_state, x, y);
                            } else if(rule.target <= 8 && rule.target >= 0 && Get_neighbor(buf_in, x, y, rule.target % 3 - 1, rule.target / 3 - 1) != rule.eval_value)
                            {
                                Generate_cell(player, buf_in[x, y], rule.return_state, x, y);
                            }
                            break;

                        case Evaluation.always:
                            Generate_cell(player, buf_in[x, y], rule.return_state, x, y);
                            break;
                    }
                }
            }
        }

        Consume(player, buf_in[x, y], cells_in[x, y]);
    }

    int Moore_neighborhood(int[,] cells_in, int x, int y)//count cells its state is not 0
    {
        int count = 0;
        if (x - 1 >= 0 && y - 1 >= 0 && cells_in[x - 1, y - 1] != 0) count++;
        if (y - 1 >= 0 && cells_in[x, y - 1] != 0) count++;
        if (x + 1 < CELL_SIZE_X && y - 1 >= 0 && cells_in[x + 1, y - 1] != 0) count++;
        if (x - 1 >= 0 && cells_in[x - 1, y] != 0) count++;
        if (x + 1 < CELL_SIZE_X && cells_in[x + 1, y] != 0) count++;
        if (x - 1 >= 0 && y + 1 < CELL_SIZE_Y && cells_in[x - 1, y + 1] != 0) count++;
        if (y + 1 < CELL_SIZE_Y && cells_in[x, y + 1] != 0) count++;
        if (x + 1 < CELL_SIZE_X && y + 1 < CELL_SIZE_Y && cells_in[x + 1, y + 1] != 0) count++;
        return count;
    }

    int Moore_neighborhood1(int[,] cells_in, int x, int y, int target)//count target state
    {
        int count = 0;
        if (x - 1 >= 0 && y - 1 >= 0 && cells_in[x - 1, y - 1] == target) count++;
        if (y - 1 >= 0 && cells_in[x, y - 1] == target) count++;
        if (x + 1 < CELL_SIZE_X && y - 1 >= 0 && cells_in[x + 1, y - 1] == target) count++;
        if (x - 1 >= 0 && cells_in[x - 1, y] == target) count++;
        if (x + 1 < CELL_SIZE_X && cells_in[x + 1, y] == target) count++;
        if (x - 1 >= 0 && y + 1 < CELL_SIZE_Y && cells_in[x - 1, y + 1] == target) count++;
        if (y + 1 < CELL_SIZE_Y && cells_in[x, y + 1] == target) count++;
        if (x + 1 < CELL_SIZE_X && y + 1 < CELL_SIZE_Y && cells_in[x + 1, y + 1] == target) count++;
        return count;
    }

    int Get_neighbor(int [,] cells_in, int x, int y,int dir_x, int dir_y)
    {
        if(x+dir_x>=0 && y+dir_y>=0 && x+dir_x<CELL_SIZE_X && y + dir_y < CELL_SIZE_Y)
        {
            return cells_in[x + dir_x, y + dir_y];
        }
        return -1;
    }

    public void Add_temperature(int x, int y, int d_tmpr)//not in use
    {
        if (x >= 0 && x < CELL_SIZE_X && y >= 0 && y < CELL_SIZE_Y)
        {
            temperature[x, y] += d_tmpr;
            if (temperature[x, y] < 0) temperature[x, y] = 0;
            else if (temperature[x, y] > MAX_TEMPERATURE) temperature[x, y] = MAX_TEMPERATURE;
        }
    }

    public void Trans_tempereture(int[,] cells_in, int x, int y, int d_tmpr, int range)//not in use
    {
        for (int _x = -range; _x <= range; _x++)
        {
            for (int _y = -range; _y <= range; _y++)
            {
                Add_temperature(x + _x, y + _y, d_tmpr * (1 - cells_in[x, y]));
            }
        }
    }

    void Decay_temperature(int[,] cells_in, int x, int y, int d_tmpr, int range)//not in use
    {
        for (int _x = -range; _x <= range; _x++)
        {
            for (int _y = -range; _y <= range; _y++)
            {
                Add_temperature(x + _x, y + _y, d_tmpr * cells_in[x, y]);
            }
        }
    }

    void Random_life1(int[,] cells_in, int x, int y, int prob,int state)//initialization
    {
        int rnd = UnityEngine.Random.Range(0, DENOMINATOR);
        if (rnd < prob)
        {
            cells_in[x, y] = state;
        }
        else
        {
            cells_in[x, y] = 0;
        }
    }

    public int Get_cell(int[,] cells_in, int x, int y)//read
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

    public void Set_Cell(int[,] cells_in, int x, int y, int state)//write
    {
        if (x >= 0 && x < CELL_SIZE_X && y >= 0 && y < CELL_SIZE_Y)
        {
            cells_in[x, y] = state;
        }
    }

    void Cell_collision(int[,] cells_in1, int[,] cells_in2, int x, int y)
    {
        if(cells_in1[x, y] != 0 && cells_in2[x, y] != 0)
        {
            cells_in1[x, y] = 0;
            cells_in2[x, y] = 0;
        }
    }

    void Turret(int[,] cells_in1, int[,] buf_in1,int[,] cells_in2, int[,] buf_in2, State_data[] states_list_in, State_data[] states_list_in2)//kari
    {
        for(int x = 0; x <= CELL_SIZE_X; x++)
        {
            for(int y = 0; y <= CELL_SIZE_Y; y++)
            {
                switch (states_list_in[cells_in1[x, y]].func)
                {
                    case Cell_function.turretU:

                    case Cell_function.turretD:

                    case Cell_function.turretR:

                    case Cell_function.turretL:
                        break;

                }
            }
        }
    }

    int Get_resource(int player)
    {
        if(player == 0 || player == 1)
        {
            return resources[player];
        } else
        {
            return -1;
        }
    }

    void Add_resource(int player, int resource_in)
    {
        resources[player] += resource_in;
        if(resources[player] < 0)
        {
            resources[player] = 0;
        }
    }

    void Consume(int player, int from, int to)//
    {
        switch (player)
        {
            case 0:
                Add_resource(player, states_list[from].cost - states_list[to].cost);
                break;

            case 1:
                Add_resource(player, states_list2[from].cost - states_list2[to].cost);
                break;
        }
    }

    void Generate_cell(int player, int from, int to, int x, int y)
    {
        if (from == 0 && to != 0)
        {
            if (resources[player] >= states_list[to].cost - states_list[from].cost)
            {
                Set_Cell(cells_list[player], x, y, to);
            }
        } else
        {
            Set_Cell(cells_list[player], x, y, to);
        }
    }
}
