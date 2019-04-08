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
		talkBoxText.text = "";

		CutsceneLine line = lineQueue.Peek();

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
