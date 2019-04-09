using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneController : MonoBehaviour
{
	[SerializeField]
	private List<Cutscene> cutscenes = new List<Cutscene>();

	private Queue<CutsceneLine> lineQueue = new Queue<CutsceneLine>();

	private GameObject dialoguePanel;
	private Text talkBoxText;
	private Text talkNameText;
	private Image talkPortrait;

	private bool lineInProgress = false;
	 

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

		talkBoxText = talkBoxTransform.GetComponent<Text>();

		var talkNameTransform = dialoguePanel.transform.Find("TalkName");

		if (talkNameTransform == null)
		{
			Debug.Log("TalkName not found");
			return;
		}

		talkNameText = talkNameTransform.GetComponent<Text>();

		var talkPortraitTransform = dialoguePanel.transform.Find("TalkPortrait");

		if (talkPortraitTransform == null)
		{
			Debug.Log("TalkPortrait not found");
			return;
		}

		talkPortrait = talkPortraitTransform.GetComponent<Image>();

		talkPortrait.enabled = false;

	}
	 
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Space))
		{ 
			if(lineInProgress)
			{
				FinishLine();
			}
			else
			{
				if(lineQueue.Count == 0)
				{ 
					dialoguePanel.SetActive(false);
					gameObject.SendMessage("SwapToPrevious");
					return;
				}

				StartCoroutine("DisplayNextLine");
			}
		}
	}

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
	 
	public IEnumerator DisplayNextLine()
	{  
		lineInProgress = true;
		 
		talkBoxText.text = String.Empty;

		CutsceneLine line = lineQueue.Peek();

		foreach (var data in line.saveDataUpdate)
		{
			gameObject.SendMessage("UpdateSaveData", data); 
		};


		if (line.name != null)
		{
			talkNameText.text = line.name;
		}
		else talkNameText.text = String.Empty;

		if (line.portrait != null)
		{
			talkPortrait.enabled = true;
			talkPortrait.sprite = line.portrait;
		}
		else
		{
			talkPortrait.enabled = false;
		}
		 
		var letters = line.text.ToCharArray();

		for (int i = 0; i < letters.Length; i++)
		{ 
			talkBoxText.text += letters[i]; 

			yield return new WaitForSeconds(0.05f);
		}

		lineQueue.Dequeue();
		lineInProgress = false; 
	}

	public void FinishLine()
	{
		StopCoroutine("DisplayNextLine");
		lineQueue.Dequeue();
		lineInProgress = false;
	}

}
