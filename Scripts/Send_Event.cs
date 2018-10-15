using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Send_Event : MonoBehaviour {

    public bool fire = false;


	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Is_clicked()
    {
        Debug.Log("event");
        fire = true;
    }
}
