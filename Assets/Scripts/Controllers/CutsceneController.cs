using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;  

public class CutsceneController : MonoBehaviour
{
	[SerializeField]
	private List<Cutscene> cutscenes = new List<Cutscene>();

	private Queue<CutsceneLine> lineQueue = new Queue<CutsceneLine>();

	private List<CutsceneLine.Choice> choiceOptions = new List<CutsceneLine.Choice>();
	private int selectIndex = 0;

	private GameObject dialoguePanel;
	private TextMeshProUGUI talkBoxText;
	private TextMeshProUGUI talkNameText;

	private GameObject portraitPanel;
	private Image talkPortrait;

	private GameObject choicePanel;
	private GameObject point0;
	private TextMeshProUGUI choice0;
	private GameObject point1;
	private TextMeshProUGUI choice1;
	private GameObject point2;
	private TextMeshProUGUI choice2;

	private int maxChoiceNumber = 3;


	private bool typeWriterActive = false;
	 
	//Finds the various components required to display dialogues
	void Awake()
	{ 
		dialoguePanel = GameObject.Find("DialoguePanel");

		if(dialoguePanel == null)
		{
			Debug.Log("DialoguePanel not found");
			return;
		}

		dialoguePanel.SetActive(false);
		 
		var talkBoxTransform = dialoguePanel.transform.Find("TalkBox");

		if(talkBoxTransform == null)
		{
			Debug.Log("TalkBox not found");
			return;
		}

		talkBoxText = talkBoxTransform.GetComponent<TextMeshProUGUI>();

		if (talkBoxText == null)
		{
			Debug.Log("TalkBoxText not found");
			return;
		}

		var talkNameTransform = dialoguePanel.transform.Find("TalkName");

		if (talkNameTransform == null)
		{
			Debug.Log("TalkName not found");
			return;
		}

		talkNameText = talkNameTransform.GetComponent<TextMeshProUGUI>();

		var portraitPanelTransform = dialoguePanel.transform.Find("PortraitPanel");

		if (portraitPanelTransform == null)
		{
			Debug.Log("Portrait Panel not found");
			return;
		}

		portraitPanel = portraitPanelTransform.gameObject;

		var portraitTransform = portraitPanelTransform.Find("Portrait");

		talkPortrait = portraitTransform.GetComponent<Image>();
		 
		portraitPanel.SetActive(false);


		var choicePanelTransform = dialoguePanel.transform.Find("ChoicePanel");

		if (choicePanelTransform == null)
		{
			Debug.Log("Choice panel not found");
			return;
		}

		choicePanel = choicePanelTransform.gameObject;
		choicePanel.SetActive(false);
		 
		var point0Transform = choicePanelTransform.Find("Point0");
		var point1Transform = choicePanelTransform.Find("Point1");
		var point2Transform = choicePanelTransform.Find("Point2");

		if (point0Transform == null || point1Transform == null || point2Transform == null)
		{
			Debug.Log("Choice panel pointers not found");
			return;
		}

		point0 = point0Transform.gameObject; 
		point1 = point1Transform.gameObject; 
		point2 = point2Transform.gameObject;
		 
		var choice0Transform = choicePanelTransform.Find("Choice0");
		var choice1Transform = choicePanelTransform.Find("Choice1");
		var choice2Transform = choicePanelTransform.Find("Choice2");

		if (choice0Transform == null || choice1Transform == null || choice2Transform == null)
		{
			Debug.Log("Choice panel text not found");
			return;
		}

		choice0 = choice0Transform.gameObject.GetComponent<TextMeshProUGUI>();
		choice1 = choice1Transform.gameObject.GetComponent<TextMeshProUGUI>();
		choice2 = choice2Transform.gameObject.GetComponent<TextMeshProUGUI>();

	}
	 
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		//Cycles through the input menu, if there is one
		if(Input.GetKeyDown(KeyCode.W))
		{
			selectIndex -= 1;

			if(selectIndex < 0)
			{
				selectIndex = choiceOptions.Count - 1;
			}

			point0.SetActive(selectIndex == 0 ? true : false);
			point1.SetActive(selectIndex == 1 ? true : false);
			point2.SetActive(selectIndex == 2 ? true : false);

			 
			return;
		}

		//Cycles through the input menu, if there is one
		if (Input.GetKeyDown(KeyCode.S))
		{
			selectIndex += 1;

			if (selectIndex >= choiceOptions.Count)
			{
				selectIndex = 0;
			}

			point0.SetActive(selectIndex == 0 ? true : false);
			point1.SetActive(selectIndex == 1 ? true : false);
			point2.SetActive(selectIndex == 2 ? true : false);

			return;
		}

		//Skips displaying the line character by character, or advances the text
		if (Input.GetKeyDown(KeyCode.Space))
		{ 
			if(typeWriterActive)
			{
				FinishLineEarly();
			}
			else
			{  
				if(choiceOptions.Count > 0)
				{
					var choice = choiceOptions[selectIndex];

					if (String.IsNullOrEmpty(choice.cutsceneId) || !TryStartCutscene(choice.cutsceneId))
					{
						StartCoroutine("DisplayNextLine");
					};

					choiceOptions.Clear();
					choicePanel.SetActive(false);
					return;

				}
				else if (lineQueue.Count == 0)
				{ 
					dialoguePanel.SetActive(false);
					gameObject.SendMessage("SwapToPrevious");
					return;
				}

				StartCoroutine("DisplayNextLine");
			}
		}
	}

	//If a cutscene with the specified name exists, play it 
	public bool TryStartCutscene(string message)
	{
		Cutscene cutscene = cutscenes.Find(c => c.name.Equals(message));

		if (cutscene == null)
		{
			Debug.Log(String.Format("Cutscene named {0} not found", message));
			return false;
		}
		 
		lineQueue = new Queue<CutsceneLine>(cutscene.lineList);

		dialoguePanel.SetActive(true);

		StartCoroutine("DisplayNextLine");

		return true;
	}
	 
	//Sets up the various message components, and makes letters appear one by one in the message box
	public IEnumerator DisplayNextLine()
	{  
		typeWriterActive = true;
		 
		talkBoxText.SetText(String.Empty);

		CutsceneLine line = lineQueue.Peek();

		//updates the worldstate
		foreach (var data in line.worldStateUpdate)
		{
			gameObject.SendMessage("UpdateWorldState", data); 
		};

		//Sets the name of the speaker
		if (line.name != null)
		{
			talkNameText.SetText(line.name);
		}
		else talkNameText.SetText(String.Empty);

		//Sets the portrait of the speaker
		if (line.portrait != null)
		{ 
			portraitPanel.SetActive(true);
			talkPortrait.sprite = line.portrait;
		}
		else
		{
			portraitPanel.SetActive(false); 
		}
		  
		talkBoxText.maxVisibleCharacters = 0; 
		talkBoxText.SetText(line.text); 
		talkBoxText.ForceMeshUpdate();
		 
		var characterCount = talkBoxText.textInfo.characterCount; //().characterCount;
		  
		var waitTimer = new WaitForSeconds(0.05f);
		for (int i = 1; i <= characterCount; i++)
		{
			talkBoxText.maxVisibleCharacters = i;  
			yield return waitTimer;
		}
		 
		TryShowOptions(line);

		lineQueue.Dequeue();
		typeWriterActive = false; 
	}

	/*
	 * Skip displaying the letters one by one 
	 */
	public void FinishLineEarly()
	{
		StopCoroutine("DisplayNextLine");

		CutsceneLine line = lineQueue.Peek();
		talkBoxText.SetText(line.text);

		talkBoxText.maxVisibleCharacters = talkBoxText.textInfo.characterCount;
		 
		TryShowOptions(line);

		lineQueue.Dequeue();
		typeWriterActive = false;

	}

	/*
	 * If there are dialogue options, try display them
	 */
	public void TryShowOptions(CutsceneLine line)
	{
		if (line.multipleChoice.Count < 1) return;

		selectIndex = 0;
		choiceOptions.AddRange(line.multipleChoice);

		point0.SetActive(true);
		point1.SetActive(false);
		point2.SetActive(false);

		choice0.SetText(String.Empty);
		choice1.SetText(String.Empty);
		choice2.SetText(String.Empty);

		for (int i = 0; i < maxChoiceNumber; i++)
		{
			if(i >= choiceOptions.Count)
			{
				break;
			}

			switch(i)
			{
				case (0): 
					choice0.SetText(choiceOptions[i].text);
					break;
				case (1):
					choice1.SetText(choiceOptions[i].text);
					break;
				case (2):
					choice2.SetText(choiceOptions[i].text);
					break;
			} 
		}

		choicePanel.SetActive(true);

	}

}
