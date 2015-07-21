﻿using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.XPath;
using System.Text;


public class LanguageManager : MonoBehaviour {

	public static LanguageManager _LanguageManager;
	public string languageFileLocation;
	
	private string language = "English";


	private XmlDocument xmlDoc = null;

	// Use this for initialization
	void Start () {
		Initialize();
		//Debug.Log(TextTranslation("ExamRoom", 3));

		//move to the start screen.
		Application.LoadLevel(1);
	}

	private void Initialize()
	{
		//make sure we don't get destroyed when the scene changes.
		DontDestroyOnLoad(gameObject);

		//verify that we have a location for the file.
		if (languageFileLocation != null || languageFileLocation != "")
		{
			//create an instance of an xml doc
			xmlDoc = new XmlDocument();

			TextAsset xml = Resources.Load(languageFileLocation) as TextAsset;
			//load the xml file.
			xmlDoc.LoadXml(xml.text);
		}

		_LanguageManager = this;

	}

	/// <summary>
	/// Return a string based on the given Input
	/// </summary>
	/// <param name="section">DiagnosisTool, ExamRoom, Feedback, GameOver, WaitingRoom</param>
	/// <param name="id">The id of the text in it's section</param>
	/// <returns></returns>
	public string TextTranslation(string section = "", int id = -5)
	{
		Debug.Log("TextTranslation for: " + section + " " + id);

		//verfiy that we have an xml doc.
		if (xmlDoc != null)
		{
			//Debug.Log(xmlDoc.FirstChild.InnerText);
			//Debug.Log(xmlDoc.GetElementsByTagName("Translations")[0].InnerText);
			//create the path we will use to find the translation we need.
			string path = "/Translations/" + language + "/" + section + "/e" + id;

			//create a variable to hold the value we find.
			string translation ;

			//make sure we can find something.
			if (xmlDoc.SelectSingleNode(path) != null)
			{
				translation = xmlDoc.SelectSingleNode(path).InnerText;

				//return what we found
				return translation;
			}
			

			//Debug.Log(xmlDoc.SelectSingleNode(path).InnerText);
			
			
		}
		

		return null;
	}

	/// <summary>
	/// Find a Direct Translation of a specific term. Only available for specific ones.
	/// </summary>
	/// <param name="section">ABG</param>
	/// <param name="term">Respiratory, Acidosis, RespiratoryOrMetabolic</param>
	/// <returns>The term translated for the current language</returns>
	public string DirectTranslation(string section, string term)
	{
		Debug.Log("DirectTranslation for: " + section + " " + term);
		if (xmlDoc != null)
		{
			//create the path we will be following to find our term.
			string path = "/Translations/" + language + "/" + section + "/" + term;

			//create a variable to hold the value we find.
			string translation;
			//make sure we find something
			if (xmlDoc.SelectSingleNode(path) != null)
			{
				translation = xmlDoc.SelectSingleNode(path).InnerText;

				//return what we found
				return translation;
			}
			
		}

		return null;
	}

	/// <summary>
	/// Return the language the game is currently using.
	/// </summary>
	/// <returns></returns>
	public string Language()
	{
		return language;
	}

	/// <summary>
	/// Change the language the game is currently set to.
	/// </summary>
	/// <param name="l">English, Spanish</param>
	public void SetLanguage(string l)
	{
		if (l == "English" || l == "Spanish")
		{
			language = l;
		}
		
	}
}
