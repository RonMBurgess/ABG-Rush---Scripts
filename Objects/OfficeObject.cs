using UnityEngine;
using System.Collections;

public class OfficeObject : MonoBehaviour {

    public Vector2 location_Nurse;
    private Animator anim;
    private int hash_Ready = Animator.StringToHash("Ready");
    private int hash_MouseOver = Animator.StringToHash("MouseOver");
    //not sure if state_ready is needed at the moment, but ill keep it for now
    private bool state_Ready, state_MouseOver;//am I idle, or am I ready. Am I currently being moused over?
    private Manager manager;


    /// <summary>
    /// Return the manager
    /// </summary>
    public Manager Manager
    {
        get { return manager; }
    }

    public void OfficeObject_Initialize()
    {
        anim = GetComponent<Animator>();
        if(GameObject.Find("Manager")) manager = GameObject.Find("Manager").GetComponent<Manager>();
    }

    

    public Vector2 OfficeObject_LocationNurse()
    {
        return location_Nurse;
    }

    public bool OfficeObject_MousedOver()
    {
        return state_MouseOver;
    }

    public bool OfficeObject_Ready()
    {
        return state_Ready;
    }

    /// <summary>
    /// Set the object as ready or idle
    /// </summary>
    /// <param name="ready">True = ready, false = idle</param>
    public void OfficeObject_SetReadyState(bool ready)
    {
        state_Ready = ready;
        if (anim != null)
        {
            anim.SetBool(hash_Ready, ready);
        }
        
    }

    private void OnMouseEnter()
    {
        state_MouseOver = true;
        if (anim != null)
        {
            anim.SetBool(hash_MouseOver, true);
        }
        
    }

    private void OnMouseExit()
    {
        state_MouseOver = false;
        if (anim != null)
        {
            anim.SetBool(hash_MouseOver, false);
        }
    }
}
