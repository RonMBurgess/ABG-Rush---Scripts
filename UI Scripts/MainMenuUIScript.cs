using UnityEngine;
using System.Collections;

public class MainMenuUIScript : MonoBehaviour {



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Practice()
    {
        Application.LoadLevel("Scn_Practice");

    }

    public void Help()
    {
        Application.LoadLevel("Scn_Help");
    }

    public void Play()
    {
        Application.LoadLevel("Scn_Play");
    }

	public void QuitOut()
	{
		Application.Quit();
	}
}
