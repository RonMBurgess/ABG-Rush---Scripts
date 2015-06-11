using UnityEngine;
using System.Collections;

public class Chair_WaitingRoom : InteractableObject{

	// Use this for initialization
	void Start () {
        Initialize();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.A)) { State_Active = !State_Active; }
	}

    public void MouseEnter()
    {
        Debug.Log("mouse has entered " + gameObject.name);
        Hover(true);
    }

    public void MouseExit()
    {
        Debug.Log("mouse has exited " + gameObject.name);
        Hover(false);
    }
}
