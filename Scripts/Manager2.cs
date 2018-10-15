using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager2 : MonoBehaviour
{
    //
    public Tiles2 tiles;
    [SerializeField]
    private Draw2 draw;
    [SerializeField]
    private Player player;
    public Send_Event[,] sendevents;
    public SpriteRenderer[,] sprrnd;
    [SerializeField]
    private ButtonInput input;
    [SerializeField]
    private Text time_indicator;
    [SerializeField]
    private Text parret_indicator;
    [SerializeField]
    private Text turn_indicator;
    [SerializeField]
    private Text resourceL_indicator;
    [SerializeField]
    private Text resourceR_indicator;

    //states
    public States state;
    private bool action_enabled;

    //time
    private float time;
    [SerializeField]
    private float process_time;
    [SerializeField]
    private float select_time;
    [SerializeField]
    private float playerLs_time;
    [SerializeField]
    private float playerRs_time;
    [SerializeField]
    private float time_gain_rate;

    //
    private int draw_state;

    public enum States
    {
        Wait, PlayerL, PlayerR
    }


    // Use this for initialization
    void Start()
    {
        draw = GetComponent(typeof(Draw2)) as Draw2;
        state = States.Wait;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        Process();
        SetText();
    }

    void CellClicked()
    {
        if (state == States.PlayerL || state == States.PlayerR)
        {

            draw_state = input.GetSelectedState();

            for (int _x = 0; _x < tiles.CELL_SIZE_X; _x++)
            {
                for (int _y = 0; _y < tiles.CELL_SIZE_Y; _y++)
                {
                    if(state == States.PlayerL)
                    {
                        if (sendevents[_x, _y].fire == true && action_enabled == true)
                        {
                            tiles.Consume(0, tiles.cells[_x, _y], draw_state);
                            tiles.Set_Cell(tiles.cells, _x, _y, draw_state);
                            sendevents[_x, _y].fire = false;
                            action_enabled = false;
                        }
                    } else if (state == States.PlayerR)
                    {
                        if (sendevents[_x, _y].fire == true && action_enabled == true)
                        {
                            tiles.Consume(1, tiles.cells[_x, _y], draw_state);
                            tiles.Set_Cell(tiles.cells_another, _x, _y, draw_state);
                            sendevents[_x, _y].fire = false;
                            action_enabled = false;
                        }
                    } 
                }
            }
            draw.Draw();
        }
    }

    public IEnumerator Wait(float sec)
    {
        yield return new WaitForSeconds(sec);
    }

    public void SetState(States state_in)
    {
        state = state_in;
        time = 0;
        ClearClickEvent();
        action_enabled = true;
    }

    public void Process()
    {
        switch (state)
        {
            case States.Wait:

                playerLs_time += Time.deltaTime * time_gain_rate;
                playerRs_time += Time.deltaTime * time_gain_rate;
                GetKey();
                if (time >= process_time)
                {
                    tiles.Next_life1();
                    draw.Draw();
                    time = 0f;
                }
                break;

            case States.PlayerL:
                if (time <= select_time && action_enabled == true && playerLs_time > 0)
                {
                    playerLs_time += -Time.deltaTime;
                    CellClicked();
                } else
                {
                    playerLs_time += -5;
                    SetState(States.Wait);
                }
                break;

            case States.PlayerR:
                if (time <= select_time && action_enabled == true && playerRs_time > 0)
                {
                    playerRs_time += -Time.deltaTime;
                    CellClicked();
                } else
                {
                    playerRs_time += -5;
                    SetState(States.Wait);
                }
                break;
        }
    }

    public void GetKey()
    {
        if (Input.GetKeyDown(KeyCode.Z) && playerLs_time > 0)
        {
            SetState(States.PlayerL);
        } else if (Input.GetKeyDown(KeyCode.Backslash) && playerRs_time > 0)
        {
            SetState(States.PlayerR);
        }
    }

    public void ClearClickEvent()
    {
        for (int _x = 0; _x < tiles.CELL_SIZE_X; _x++)
        {
            for (int _y = 0; _y < tiles.CELL_SIZE_Y; _y++)
            {
                sendevents[_x, _y].fire = false;
            }
        }
    }

    public void SetText()
    {
        turn_indicator.text = state.ToString() + "'sTrun";
        parret_indicator.text = "State" + draw_state + "is selected";
        if (state == States.PlayerL) time_indicator.text = playerLs_time.ToString();
        else if (state == States.PlayerR) time_indicator.text = playerRs_time.ToString();
        resourceL_indicator.text = "L's resource is " + tiles.resources[0];
        resourceR_indicator.text = "R's resource is " + tiles.resources[1];
    }
}
