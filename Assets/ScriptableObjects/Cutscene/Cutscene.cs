using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A scriptable object that contains a list of cutscene lines 
[CreateAssetMenu(fileName = "New Cutscene", menuName = "Cutscene")]
public class Cutscene : ScriptableObject
{
	public List<CutsceneLine> lineList = new List<CutsceneLine>();
}
