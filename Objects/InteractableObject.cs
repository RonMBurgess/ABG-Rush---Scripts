using UnityEngine;
using System.Collections;

public class InteractableObject : MonoBehaviour
{

    #region variables

    public Vector2 position_Nurse, position_Patient;

    private bool state_active; //Active or Idle
    private Animator anim; // the animator controller
    private Manager manager;
    private int hash_Hover = Animator.StringToHash("Hover");
    private int hash_Active = Animator.StringToHash("Active");
    private Patient patient;

    #endregion


    /// <summary>
    /// Return True for Active, false for idle.
    /// </summary>
    public bool State_Active
    {
        get { return state_active; }
        set { state_active = value; anim.SetBool(hash_Active, state_active); }
    }

    public Vector2 Location_Nurse
    {
        get { return position_Nurse; }
    }

    public Vector2 Location_Patient
    {
        get { return position_Patient; }
    }


    /// <summary>
    /// Prepare the object for use.
    /// </summary>
    public void Initialize()
    {
        //gain access to necessary components and game manager
        if(GetComponent<Animator>() != null) anim = GetComponent<Animator>();

        GameObject m = GameObject.Find("Manager");
        if (m != null) manager = m.GetComponent<Manager>();

        //set the status of all variables
        state_active = false;

        //inform manager that I have been initialized
    }

    /// <summary>
    /// Set the Interactable Object's Hover State
    /// </summary>
    /// <param name="isHovering">Hovering = true</param>
    public void Hover(bool isHovering)
    {
        anim.SetBool(hash_Hover, isHovering);
    }


    
}
