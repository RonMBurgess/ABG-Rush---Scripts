using UnityEngine;
using System.Collections;

public class Triage : PatientObject {

    public UI_Triage ui_Triage;//Change this later to be accessed from the gameplay UI script instead of a public variable.

	// Use this for initialization
	void Start () {
        tag = "Triage";
        OfficeObject_Initialize();
        MyUI = ui_Triage;

	}


	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseOver()
    {
        if (OfficeObject_Ready() && OfficeObject_MousedOver())
        {
            if (Input.GetMouseButtonDown(0))
            {
                //tell the nurse to move to location
                Manager.MyNurse.Person_Move(location_Nurse,tag,true,this);
            }

        }
    }

    
}
