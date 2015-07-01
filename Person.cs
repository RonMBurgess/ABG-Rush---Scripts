using UnityEngine;
using System.Collections;


[RequireComponent(typeof(PolyNavAgent))]
[RequireComponent(typeof(SpriteRenderer))]

/// <summary>
/// The purpose of this class is to be the parent of both the nurse and patients. This will contain functions or variables that both will use.
/// </summary>
public class Person : MonoBehaviour {

    private PolyNavAgent agent;
    private string destinationName;
    private Manager manager;
    private bool moving, patientObject;
    private OfficeObject officeObject;
    private SpriteRenderer sr;


    public Manager Manager
    {
        get { return manager; }
    }


    /// <summary>
    /// Prepare all variables and components
    /// </summary>
    public void Person_Initialize()
    {
        Debug.Log("Initializing Person");
        agent = GetComponent<PolyNavAgent>();
        sr = GetComponent<SpriteRenderer>();
        if (CompareTag("Patient"))
        {
            sr.color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
        }
        destinationName = "";
        if(GameObject.Find("Manager")) manager = GameObject.Find("Manager").GetComponent<Manager>();
        moving = false;
    }


    /// <summary>
    /// Move to specified location. Called by Hotspot or Manager
    /// </summary>
    /// <param name="m">Location to move to</param>
    /// <param name="dName">Tag of destination</param>
    /// <param name="pObject">Is this a Patient Object?</param>
    /// <param name="officeobject">The class</param>
    public void Person_Move(Vector2 m, string dName, bool pObject = true, OfficeObject officeobject = null)
    {
		//if I am a nurse, set myself to busy
		if (gameObject.CompareTag("Nurse"))
		{
			(this as Nurse).IsBusy(-1);
		}

		//make sure that I have a reference to my polynav agent.
        if (agent == null)
        {
            agent = GetComponent<PolyNavAgent>();
        }
		//make sure that the agent is enabled, and then set the agent's destination.
        agent.enabled = true;
        agent.SetDestination(m, Person_MovementStatus);

        destinationName = dName;
        if (officeobject) { patientObject = pObject; officeObject = officeobject; }
        moving = true;
    }

    /// <summary>
    /// Called after a movement action has been taken
    /// </summary>
    /// <param name="moved">True - Reached Destination | False - Did not reach Destination</param>
    private void Person_MovementStatus(bool moved)
    {
		Debug.Log(name + " has arrived at destination " + destinationName);
        //inform self that my status has changed, so if I'm a nurse, I should now be either waiting at triage, waiting at waiting room, or waiting in patient room
        if (moved && gameObject.CompareTag("Patient"))
        {
            (this as Patient).Patient_StatusUpdate(destinationName);
            if ((this as Patient).Patient_Hotspot_Get())
            {
                (this as Patient).Patient_Hotspot_Get().OfficeObject_SetReadyState(true);
            }
                        
        }

        else if (gameObject.CompareTag("Nurse"))
        {
			string status = " ";
			//if we are dealing with a patientobject
			if (patientObject)
			{
				status = (officeObject as PatientObject).MyPatient.Status();

				if (destinationName == "WaitingChair")
				{
					//open the UI of the Waiting Chair
					(officeObject as PatientObject).PatientObject_OpenUI();
					Debug.Log("Opened the UI for " + officeObject.name);
				}
				else if (destinationName == "ExamRoom")
				{
					//if the patient is currently waiting to get their vitals checked.
					if (status == "Vitals")
					{
						//the patient is waiting for their vitals to be checked.
						//perform vital sign check, another conversation between nurse and patient, and then popup.
						(this as Nurse).Nurse_PerformAction("Vitals", officeObject, (officeObject as PatientObject).MyPatient);

					}
					//if the patient is currently waiting to get a diagnosis
					else if (status == "Diagnosis")
					{
						//the patient is waiting to be diagnosed.
						//RON COME BACK AND CHANGE THIS
						//do nothing.. or move to the computer
					}
					//else if (status == "ExamRoom")
					//{
						//Moved down below
					//}
				}
			}
			else
			{
				if (destinationName == "ExamRoomComputer")
				{
					//get the patient
					Patient p = (officeObject as ExamRoomComputer).MyExamRoom().MyPatient;
					
					//If the status is exam room, then the nurse must setup and prepare the computer.
					if (p.Status() == "ExamRoom")
					{
						//same /similar process as vitals
						(this as Nurse).Nurse_PerformAction("ExamRoomSetup", officeObject, p);
					}
					else
					{
						//Open up the Interface for the computer.
					}

					//open up the examroom computer with the proper information
					//The method should take in the patient, and open up the interface based on the patient's status.

					//if the status of the patient is VitalsComplete, display 100% of information and pose the question to get bloodwork.
					//if the status of the patient is vitals, display only the story, name and dob.
					//if the status of the patient is bloodwork, display 100% of the information.
					//if the status of the patient is diagnosis, display the abg tool
				}
				else if (destinationName == "Sink")
				{
					(this as Nurse).Nurse_PerformAction("Wash Hands");
				}
				else if (destinationName == "ReferenceDesk")
				{
					//open up the reference desk
					//pause the game?
				}
			}
            patientObject = false;
            officeObject = null;
            agent.enabled = false;

			(this as Nurse).IsBusy(1);
        }

        moving = false;
    }

    public bool Moving()
    {
        return moving;
    }

    public void Person_Update()
    {
        //if (moving)
        //{
        //Debug.Log("Person Update from " + name);
            sr.sortingOrder = -1 * (Mathf.CeilToInt(transform.position.y * 100f));

        //}
    }
}
