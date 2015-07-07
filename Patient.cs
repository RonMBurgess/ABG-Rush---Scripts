using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Patient : Person {

	public SpriteRenderer highlightRenderer = null;

    private float timer_Triage, timer_WaitingRoom, timer_ExamRoom, timer_Vitals, timer_Bloodwork, timer_Diagnosis, timer_Delay_Pacification, timer_Current, timer_CurrentSet;
    private int pacify_AmountLeft;//the amount will change if a patient is interacted with, but no action is taken. This will reduce the current timer by the pacification delay.
    private string patient_Name, status;//The current status of the patient: (Triage, Waiting, Exam Room, Vitals, etc...)
    private Diagnosis diagnosis;
    private PatientObject hotspot;
    private bool timer_Halted;
    private Collider2D collider;

	//Animator/animation stuff
	private Animator anim;
	private List<int> animationPositions;
	private int hash_Talking = Animator.StringToHash("Talking");
	private int hash_Walking = Animator.StringToHash("Walking");
	private int hash_Sitting = Animator.StringToHash("Sitting");
	private int hash_Turned = Animator.StringToHash("Turned");
	private int hash_Patience = Animator.StringToHash("Patience");
	private int hash_Highlight = Animator.StringToHash("Highlight");
	private int hash_Waiting = Animator.StringToHash("Waiting");
 

	// Use this for initialization
	void Start () {
        //Person_Initialize();
        //Patient_Initalize();
        //tag = "Patient";
	}
	
	// Update is called once per frame
	void Update () {
        Person_Update(highlightRenderer);
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
			case "Triage": Patient_ToggleCountdown(true); collider.enabled = false; Patient_Animation("Turned", false, true); break;
			case "WaitingChair": timer_Current = timer_WaitingRoom; timer_CurrentSet = timer_WaitingRoom; Patient_ToggleCountdown(false); collider.enabled = true; Patient_Animation("Waiting", false, true); Patient_Animation("Highlight", false, true); break;
			case "ExamRoom": timer_Current = timer_ExamRoom; timer_CurrentSet = timer_ExamRoom; collider.enabled = false; Patient_Animation("Sitting", false, true); break;
			case "Vitals": timer_Current = timer_Vitals; timer_CurrentSet = timer_Vitals; collider.enabled = true; Patient_Animation("Sitting", false, true); Patient_Animation("Highlight", false, true); break;
			case "VitalsComplete": timer_Current = timer_Vitals; timer_CurrentSet = timer_Vitals; Patient_ToggleCountdown(true); collider.enabled = false; Patient_Animation("Sitting", false, true); break;//countdown will re-enable after a decision is made in UI
			//case "Bloodwork": timer_Current = timer_Bloodwork; collider.enabled = false; break;
			case "BloodworkWaiting": timer_Current = timer_Bloodwork; timer_CurrentSet = timer_Bloodwork; collider.enabled = false; Patient_Animation("Sitting", false, true); Patient_Animation("Highlight", false, false); break;
			case "Diagnosis": timer_Current = timer_Diagnosis; timer_CurrentSet = timer_Diagnosis; collider.enabled = true; Patient_Animation("Sitting", false, true); Patient_Animation("Highlight", false, true); break;
			case "DiagnosisComplete": timer_Current = 999f; collider.enabled = false; Patient_Animation("Highlight", false, false); Patient_Leave(); break;
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
        timer_WaitingRoom = 30f;//how long I will wait in the waiting room before leaving
        timer_ExamRoom = 30f;//how long I will wait in the exam room before leaving
		timer_Vitals = 30f;
		timer_Bloodwork = 30f;
		timer_Diagnosis = 60f;
        timer_Current = 10f;
        timer_Delay_Pacification = 20f;
        pacify_AmountLeft = 2;
        anim = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
        if (collider)
        {
            //turn collider off
            collider.enabled = false;
        }

		if (anim)
		{
			anim.SetBool(hash_Highlight, false);
			anim.SetBool(hash_Sitting, false);
			anim.SetBool(hash_Talking, false);
			anim.SetBool(hash_Waiting, false);
			anim.SetBool(hash_Walking, false);
			anim.SetFloat(hash_Patience, 1f);

			animationPositions = new List<int>();
			animationPositions.Add(hash_Sitting); animationPositions.Add(hash_Walking); animationPositions.Add(hash_Waiting); animationPositions.Add(hash_Turned);
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
			if (anim)
			{
				anim.SetFloat(hash_Patience, timer_Current / timer_CurrentSet);
			}
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
		Debug.Log("Patient_Leave");
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
            //Manager.Manager_MouseOver(true);
            //hotspot.OfficeObject_MouseEnter();
        }
    }

    void OnMouseExit()
    {
        if (hotspot)
        {
            //Manager.Manager_MouseOver(false);
            //hotspot.OfficeObject_MouseExit();
        }
        
        
    }

    void OnMouseOver()
    {
        if (hotspot && !Manager.MyNurse.IsBusy())
        {
            if (hotspot.OfficeObject_Ready()/* && hotspot.OfficeObject_MousedOver()*/ && hotspot.tag != "Triage" && !Moving())
            {
                if (Input.GetMouseButtonUp(0))
                {
					//bool nextStep = false;
					if (status == "ExamRoom" || status == "Vitals")
					{
						//tell the nurse to move to the proper location
						//exam room should no longer be a status since it's automated now, but it currently remains since this is not final.
						Manager.MyNurse.Person_Move(hotspot.OfficeObject_LocationNurse(), "ExamRoom", true, hotspot);
						//nextStep = true;
					}
					else if (status == "BloodworkWaiting" || status == "Diagnosis" || status == "VitalsComplete")
					{
						//tell the nurse to move to the exam room computer
						Manager.MyNurse.Person_Move((hotspot as ExamRoom).Computer().OfficeObject_LocationNurse(), "ExamRoomComputer", false, (hotspot as ExamRoom).Computer());
						//nextStep = true;
					}
					else if(status == "WaitingChair")
					{
						Manager.MyNurse.Person_Move(hotspot.OfficeObject_LocationNurse(), hotspot.tag, true, hotspot);
						//nextStep = true;
					}
					//if (nextStep && !timer_Halted)
					//{
					//	if (timer_Current / timer_CurrentSet > .6) { Manager.UpdateSatisfactionScore(2); }
					//	else if (timer_Current / timer_CurrentSet < .3) { Manager.UpdateSatisfactionScore(-1); }
					//}
                }

            }
        }
        
    }

	/// <summary>
	/// Update the score based on how long you waited.
	/// </summary>
	public void Patient_PatienceScore()
	{
		if (status != "ExamRoom" && status != "Triage")
		{
			if (timer_Current / timer_CurrentSet > .6) { Manager.UpdateSatisfactionScore(2); }
			else if (timer_Current / timer_CurrentSet < .3) { Manager.UpdateSatisfactionScore(-1); }
		}
	}

    /// <summary>
    /// Called outside of the class.
    /// </summary>
    /// <param name="animationName">Name of the Animation: Talking</param>
    /// <param name="trigger">Is this animation a trigger?</param>
    /// <param name="on">On or Off</param>
    public void Patient_Animation(string animationName, bool trigger, bool on)
    {
		//if (trigger)
		//{
		//	anim.SetTrigger(animationName);
		//}
		//else
		//{
		//	anim.SetBool(animationName, tru);
		//}
		if (anim)
		{
			Debug.Log("Setting " + animationName + " to " + on);
			if (animationName == "Waiting" || animationName == "Sitting" || animationName == "Walking" || animationName == "Turned")
			{
				foreach (int i in animationPositions)
				{
					anim.SetBool(i, false);
				}
				
				anim.SetBool(animationName, on);
			}
			if (animationName == "Talking")
			{
				anim.SetBool(hash_Talking, on);
			}
			
			else if (animationName == "Highlight")
			{
				anim.SetBool(hash_Highlight, on);
			}
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


	public void Patient_Setup(string n, string dob, Diagnosis d){
		Person_Initialize();
		Patient_Initalize();
		tag = "Patient";

		name = n;
		patient_Name = n;

		//COME BACK AND SET DOB

		diagnosis = d;
		Debug.Log(name + "'s Diagnosis is: " + d.Answer_Respiratory_Metabolic + " " + d.Answer_Acidosis_Alkalosis + " " + d.Answer_Compensation);
		Debug.Log(name + "'s Story is: " + d.Story("S"));
	}
}
