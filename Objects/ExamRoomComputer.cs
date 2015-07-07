using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ExamRoomComputer : OfficeObject {

	private ExamRoom er;
	public float bloodworkTimeAlloted;
	private float bloodworkTimeUsed;
	private bool bloodworkSent;

	// Use this for initialization
	void Start () {
		
		
        OfficeObject_Initialize();
		InitializeExamRoomComputer();
	}

	private void InitializeExamRoomComputer()
	{
		tag = "ExamRoomComputer";
		bloodworkTimeUsed = 0;
		bloodworkSent = false;
	}

	// Update is called once per frame
	void Update () {
		if (bloodworkSent)
		{
			bloodworkTimeUsed += Time.deltaTime;
			if (bloodworkTimeUsed >= bloodworkTimeAlloted)
			{
				//reset bloodwork sent
				bloodworkSent = false;

				//inform the patient of status change/update.
				
				MyExamRoom().MyPatient.Patient_StatusUpdate("Diagnosis");
			}
		}
	}

    void OnMouseOver()
    {
		//verify that nurse is not currently busy. Verify that I have a reference to a patient
		if (!Manager.MyNurse.IsBusy() && er.MyPatient)
		{
			//Highlight this object.
			Highlight(true);

			//Manager.Manager_MouseOver(true);
			if (Input.GetMouseButtonUp(0))
			{
				Debug.Log("Telling the Nurse to move to me: " + this);
				Manager.MyNurse.Person_Move(location_Nurse, tag, false, this);
			}
		}
        
    }

    void OnMouseExit()
    {
        //Manager.Manager_MouseOver(false);
		Highlight(false);
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

	/// <summary>
	/// Begin the countdown for bloodwork
	/// </summary>
	public void SendBloodwork()
	{
		bloodworkTimeUsed = 0;
		bloodworkSent = true;
		MyExamRoom().MyPatient.Patient_StatusUpdate("BloodWorkWaiting");
	}
}
