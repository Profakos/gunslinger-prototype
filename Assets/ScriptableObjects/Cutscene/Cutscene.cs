using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Cutscene", menuName = "Cutscene")]
public class Cutscene : ScriptableObject
{
	public List<CutsceneLine> lineList = new List<CutsceneLine>();
}
