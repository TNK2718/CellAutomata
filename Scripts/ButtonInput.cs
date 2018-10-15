using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInput : MonoBehaviour {

    private int selected_state = 0;

    public void Select01()
    {
        selected_state = 1;//TODO
    }

    public void Select02()
    {
        selected_state = 2;
    }

    public int GetSelectedState()
    {
        return selected_state;
    }
}
