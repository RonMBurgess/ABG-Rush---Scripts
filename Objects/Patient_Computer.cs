using UnityEngine;
using System.Collections;

public class Patient_Computer : OfficeObject {

	// Use this for initialization
	void Start () {
        OfficeObject_Initialize();
	}
	
	// Update is called once per frame
	void Update () {
	   
	}

    void OnMouseOver()
    {
        Manager.Manager_MouseOver(true);
        if (Input.GetMouseButtonUp(0))
        {
            Manager.MyNurse.Person_Move(location_Nurse, "Sink", false);
        }
    }

    void OnMouseExit()
    {
        Manager.Manager_MouseOver(false);
    }
}
