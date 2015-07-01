using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ExamRoomComputer : OfficeObject {

	private ExamRoom er;

	// Use this for initialization
	void Start () {
		tag = "ExamRoomComputer";
        OfficeObject_Initialize();
	}
	
	// Update is called once per frame
	void Update () {
	   
	}

    void OnMouseOver()
    {
		//verify that nurse is not currently busy. Verify that I have a reference to a patient
		if (!Manager.MyNurse.IsBusy() && er.MyPatient)
		{
			Manager.Manager_MouseOver(true);
			if (Input.GetMouseButtonUp(0))
			{
				Manager.MyNurse.Person_Move(location_Nurse, tag, false);
			}
		}
        
    }

    void OnMouseExit()
    {
        Manager.Manager_MouseOver(false);
    }

	/// <summary>
	/// Return the Exam Room I am part of.
	/// <param name="e"> Exam Room to set or nothing to retrieve.</param>
	/// </summary>
	public ExamRoom MyExamRoom(ExamRoom e = null)
	{
		if (e)
		{
			er = e;
		}
		return er;
	}
}
