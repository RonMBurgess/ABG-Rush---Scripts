using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ABGWW_Help_Script : MonoBehaviour {

    public Animator anim_Loading;//This is the animator controller on the loading object.
    public List<Button> menuButtons; //all of the buttons found on the menu.
    public GameObject contentArea, navPanel;//This is the gameobject that holds the content for the different screens.
    public ABGToolManagerScript ABGTool;//the script of the ABG tool
   
    private int loadingHash = Animator.StringToHash("Loading"), curScreen = 0;

	// Use this for initialization
	void Start () {
        //ABGTool.gameObject.SetActive(false);
        //Debug.Log("ABG tool should be off");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// called by the buttons that turn the helper on/off.
    /// </summary>
    /// <param name="on"></param>
    public void OnOffABGHelper(bool on)
    {
        if (on)
        {
            //determine if GO is already on.
            if (!gameObject.activeInHierarchy)
            {
                //turn the helper on.
                gameObject.SetActive(true);
                //go to the home screen.
                ChangeScreens(0);
            }
            
        }
        else if (!on)
        {
            gameObject.SetActive(false);
        }
    }

    
    public void ChangeScreens(int screen)
    {
        StartCoroutine("ScreenSwap", screen);
        
    }

    /// <summary>
    /// Display different content based on the screen requested.
    /// </summary>
    /// <param name="screen">The screen requested.</param>
    IEnumerator ScreenSwap(int screen)
    {

        if (screen == curScreen)
        {
            //do nothing
        }
        else
        {
            //turn off all buttons
            MenuButtonsSwitcher(false);
            //turn the current page's content off
            contentArea.SetActive(false);
            //turn off the ABG tool just in case.
            ABGTool.gameObject.SetActive(false);

            //show the loading image
            anim_Loading.gameObject.SetActive(true);
            anim_Loading.SetBool(loadingHash, true);

            //wait for 2.5 seconds
            yield return new WaitForSeconds(1.5f);

            switch (screen)
            {
                    //come back and change this later.
                case 0:  //break;
                case 1:
                case 2:
                case 3: anim_Loading.gameObject.SetActive(false); anim_Loading.SetBool(loadingHash, false); contentArea.SetActive(true); navPanel.SetActive(true); break;
                case 4: anim_Loading.gameObject.SetActive(false); anim_Loading.SetBool(loadingHash, false); ABGTool.gameObject.SetActive(true); navPanel.SetActive(false); ABGTool.Reset(); break;

            }

            curScreen = screen;
            //reset menu buttons
            MenuButtonsSwitcher(true);

            //yield return new WaitForSeconds(.5f);
        }


        
    }

    /// <summary>
    /// turn all of the buttons in the overhead menu on or off.
    /// </summary>
    /// <param name="on">True will make the buttons interactable, while false will do the opposite.</param>
    private void MenuButtonsSwitcher(bool on)
    {
        foreach (Button b in menuButtons)
        {
            b.interactable = on;
        }
    }
}
