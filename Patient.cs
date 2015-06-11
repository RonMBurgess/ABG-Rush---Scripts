using UnityEngine;
using System.Collections;

public class Patient : Person {

    private float timer_Triage, timer_WaitingRoom, timer_ExamRoom, timer_Delay_Pacification, timer_Current;
    private int pacify_AmountLeft;//the amount will change if a patient is interacted with, but no action is taken. This will reduce the current timer by the pacification delay.
    private string patient_Name, patient_Story;
    private PatientObject hotspot;
    private bool timer_Halted;
 

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

        timer_Triage = 10f;
        timer_WaitingRoom = 10f;
        timer_ExamRoom = 10f;
        timer_Current = 10f;
        timer_Delay_Pacification = 10f;
        pacify_AmountLeft = 2;
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
    /// <param name="stop">True - Stop, False - Resume/Start</param>
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
        if (hotspot)
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
        if (hotspot)
        {
            if (hotspot.OfficeObject_Ready() && hotspot.OfficeObject_MousedOver())
            {
                if (Input.GetMouseButtonUp(0))
                {
                    //tell the nurse to move to location
                    Manager.MyNurse.Person_Move(hotspot.OfficeObject_LocationNurse(), tag, true, hotspot);
                }

            }
        }
        
    }
}
