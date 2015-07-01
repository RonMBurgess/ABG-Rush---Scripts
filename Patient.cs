using UnityEngine;
using System.Collections;

public class Patient : Person {

    private float timer_Triage, timer_WaitingRoom, timer_ExamRoom, timer_Vitals, timer_Bloodwork, timer_Diagnosis, timer_Delay_Pacification, timer_Current;
    private int pacify_AmountLeft;//the amount will change if a patient is interacted with, but no action is taken. This will reduce the current timer by the pacification delay.
    private string patient_Name, status;//The current status of the patient: (Triage, Waiting, Exam Room, Vitals, etc...)
    private Diagnosis diagnosis;
    private PatientObject hotspot;
    private bool timer_Halted;
    private Animator anim;
    private Collider2D collider;
 

	// Use this for initialization
	void Start () {
        Person_Initialize();
        Patient_Initalize();
        tag = "Patient";
	}
	
	// Update is called once per frame
	void Update () {
        Person_Update();
        if (!Moving() /*and not currently in UI*/ && !timer_Halted)
        {
            Patient_PatienceCountdown();
        }
	}

    /// <summary>
    /// Inform the patient of it's new Status
    /// </summary>
    /// <param name="location">Triage, WaitingRoom, ExamRoom, Vitals, Bloodwork, Diagnosis, Leave</param>
    public void Patient_StatusUpdate(string stat)
    {
        switch (stat)
        {
			case "Triage": Patient_ToggleCountdown(true); collider.enabled = false; break;
            case "WaitingChair": timer_Current = timer_WaitingRoom; collider.enabled = true; break;
            case "ExamRoom": timer_Current = timer_ExamRoom; collider.enabled = false; break;
			case "Vitals": timer_Current = timer_Vitals; collider.enabled = true; break;
			case "VitalsComplete": timer_Current = timer_Vitals; break;//countdown will re-enable after a decision is made in UI
			case "Bloodwork": timer_Current = timer_Bloodwork; collider.enabled = false; break;
			case "Diagnosis": timer_Current = timer_Diagnosis; collider.enabled = true; break;
            case "Exit": Destroy(gameObject); break;
        }
		status = stat;
    }

    /// <summary>
    /// Set this patient's current Hotspot
    /// </summary>
    /// <param name="po"></param>
    public void Patient_Hotspot(PatientObject po)
    {
        hotspot = po;
    }

    /// <summary>
    /// Return this patient's hotspot
    /// </summary>
    public PatientObject Patient_Hotspot_Get()
    {
        return hotspot;
    }


    /// <summary>
    /// Initialize Patient variables
    /// </summary>
    private void Patient_Initalize(){
        //This may be changed later depending on when/where we would like to generate all the patient's information. 
        //It could be done here either immediately on startup or after the manager has given input, and each patient can be self inclusive
        //This information could be created/generated entirely through the manager as well, and this patient just holds the data.

        timer_Triage = 10f;//how long I will wait at the triage before leaving
        timer_WaitingRoom = 10f;//how long I will wait in the waiting room before leaving
        timer_ExamRoom = 10f;//how long I will wait in the exam room before leaving
		timer_Vitals = 10f;
		timer_Bloodwork = 15f;
		timer_Diagnosis = 20f;
        timer_Current = 10f;
        timer_Delay_Pacification = 10f;
        pacify_AmountLeft = 2;
        anim = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
        if (collider)
        {
            //turn collider off
            collider.enabled = false;
        }
    }

    /// <summary>
    /// Pacifiy the patient, and add more time to the timer
    /// </summary>
    public void Patient_Pacify()
    {
        if (pacify_AmountLeft > 0)
        {
            timer_Current += timer_Delay_Pacification;
            pacify_AmountLeft--;
        }
    }

    /// <summary>
    /// Return the amount of times a patient can be pacified.
    /// </summary>
    /// <returns></returns>
    public int Patient_Pacify_AmountLeft()
    {
        return pacify_AmountLeft;
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
                Patient_StormOut();
            }
        }
        
    }

    /// <summary>
    /// Called to stop the patient's current countdown clock.
    /// </summary>
    /// <param name="halt">True - Stop, False - Resume/Start</param>
    public void Patient_ToggleCountdown(bool halt)
    {
        timer_Halted = halt;
    }


    /// <summary>
    /// Leave angrily
    /// </summary>
    private void Patient_StormOut()
    {
        //inform current hotspot
        hotspot.PatientObject_Patient_Remove();
        //inform manager
        Manager.Manager_Patient_StormOut(this);
        //storm out
        //manager should inform me of where the exit is.
    }

    /// <summary>
    /// Leave after either finishing treatment or being told I cannot be helped here.
    /// </summary>
    public void Patient_Leave()
    {
        //inform current hotspot
        hotspot.PatientObject_Patient_Remove();
        //inform manager
        Manager.Manager_Patient_Leave(this);
        //leave
    }

    void OnMouseEnter()
    {
		//if I am on a hotspot, and the nurse is not busy.
        if (hotspot && !Manager.MyNurse.IsBusy())
        {
            Manager.Manager_MouseOver(true);
            hotspot.OfficeObject_MouseEnter();
        }
    }

    void OnMouseExit()
    {
        if (hotspot)
        {
            Manager.Manager_MouseOver(false);
            hotspot.OfficeObject_MouseExit();
        }
        
        
    }

    void OnMouseOver()
    {
        if (hotspot && !Manager.MyNurse.IsBusy())
        {
            if (hotspot.OfficeObject_Ready() && hotspot.OfficeObject_MousedOver() && hotspot.tag != "Triage" && !Moving())
            {
                if (Input.GetMouseButtonUp(0))
                {
					if (status == "ExamRoom" || status == "Vitals")
					{
						//tell the nurse to move to the proper location
						//exam room should no longer be a status since it's automated now, but it currently remains since this is not final.
						Manager.MyNurse.Person_Move(hotspot.OfficeObject_LocationNurse(), "ExamRoom", true, hotspot);
					}
					else if (status == "Bloodwork" || status == "Diagnosis" || status == "VitalsComplete")
					{
						//tell the nurse to move to the exam room computer
						Manager.MyNurse.Person_Move((hotspot as ExamRoom).Computer().OfficeObject_LocationNurse(), "ExamRoomComputer", false, hotspot);
					}
					else if(status == "WaitingChair")
					{
						Manager.MyNurse.Person_Move(hotspot.OfficeObject_LocationNurse(), hotspot.tag, true, hotspot);
					}
                }

            }
        }
        
    }

    /// <summary>
    /// Called outside of the class.
    /// </summary>
    /// <param name="animationName">Name of the Animation: Talking</param>
    /// <param name="trigger">Is this animation a trigger?</param>
    /// <param name="tru">On or Off</param>
    public void Patient_Animation(string animationName, bool trigger, bool tru)
    {
        if (trigger)
        {
            anim.SetTrigger(animationName);
        }
        else
        {
            anim.SetBool(animationName, tru);
        }
    }

	/// <summary>
	/// The status of the patient.
	/// </summary>
	/// <returns>Triage, WaitingChair, ExamRoom, Vitals, Bloodwork, Diagnosis, Exit</returns>
	public string Status()
	{
		return status;
	}

	/// <summary>
	/// Return the Diagnosis
	/// </summary>
	/// <returns></returns>
	public Diagnosis MyDiagnosis()
	{
		return diagnosis;
	}
}
