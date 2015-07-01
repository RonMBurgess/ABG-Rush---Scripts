using UnityEngine;
using System.Collections;

public class Sink : OfficeObject {

	// Use this for initialization
	void Start () {
        OfficeObject_Initialize();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseOver()
    {
		//check to see if the nurse is currently busy. If the nurse is not busy...
		if (!Manager.MyNurse.IsBusy())
		{
			Manager.Manager_MouseOver(true);
			if (Input.GetMouseButtonUp(0))
			{
				Manager.MyNurse.Person_Move(location_Nurse, "Sink", false, this);
			}
		}
        
    }

    void OnMouseExit()
    {
        Manager.Manager_MouseOver(false);
    }
}
