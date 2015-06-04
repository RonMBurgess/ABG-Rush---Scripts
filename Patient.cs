using UnityEngine;
using System.Collections;

public class Patient : Person {

    private float timer_Triage, timer_WaitingRoom, timer_ExamRoom, timer_Delay_Pacification, timer_Current;
    private int pacifiy_AmountLeft;//the amount will change if a patient is interacted with, but no action is taken. This will reduce the current timer by the pacification delay.
    private string patient_Name;
    private PatientObject hotspot;
 

	// Use this for initialization
	void Start () {
        Person_Initialize();
        Patient_Initalize();
        tag = "Patient";
	}
	
	// Update is called once per frame
	void Update () {
        Patient_PatienceCountdown();
	}

    /// <summary>
    /// Inform the patient of it's new location
    /// </summary>
    /// <param name="location">Triage, WaitingRoom, ExamRoom</param>
    public void Patient_LocationChange(string location)
    {
        switch (location)
        {
            case "Triage": timer_Current = timer_Triage; break;
            case "WaitingChair": timer_Current = timer_WaitingRoom; break;
            case "ExamRoom": timer_Current = timer_ExamRoom; break;
            case "Exit": Destroy(gameObject); break;
        }
    }

    public void Patient_Hotspot(PatientObject po)
    {
        hotspot = po;
    }


    /// <summary>
    /// Initialize Patient variables
    /// </summary>
    private void Patient_Initalize(){
        //This may be changed later depending on when/where we would like to generate all the patient's information. 
        //It could be done here either immediately on startup or after the manager has given input, and each patient can be self inclusive
        //This information could be created/generated entirely through the manager as well, and this patient just holds the data.

        timer_Triage = 10f;
        timer_WaitingRoom = 10f;
        timer_ExamRoom = 10f;
        timer_Current = 10f;
    }

    /// <summary>
    /// Pacifiy the patient, and add more time to the timer
    /// </summary>
    public void Patient_Pacify()
    {
        if (pacifiy_AmountLeft > 0)
        {
            timer_Current += timer_Delay_Pacification;
        }
    }


    /// <summary>
    /// Called each update to tick down the patience timers.
    /// </summary>
    private void Patient_PatienceCountdown()
    {
        if (timer_Current > 0)
        {
            timer_Current -= Time.deltaTime;
            if (timer_Current <= 0)
            {
                //inform current hotspot
                hotspot.PatientObject_Patient_Remove();
                //inform manager
                Manager.Manager_Patient_StormOut(this);
                //storm out
                //manager should inform me of where the exit is.
            }
        }
        
    }
}
