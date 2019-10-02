using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CutsceneEditorWindow : EditorWindow
{
	private Cutscene editedCutscene;

	[MenuItem("MyMenu/CutsceneEditorWindow")]
	static void Init()
	{
		CutsceneEditorWindow window = (CutsceneEditorWindow)EditorWindow.GetWindow(typeof(CutsceneEditorWindow));
	}

	void OnGUI()
	{
		Object o = EditorGUILayout.ObjectField("Edited cutscene ", editedCutscene, typeof(ScriptableObject), false);
		editedCutscene = (Cutscene)o;

		if(editedCutscene != null)
		{
			for(int i = 0; i < editedCutscene.lineList.Count; i++)
			{
				var currentLine = editedCutscene.lineList[i];

				EditorGUILayout.BeginVertical("box");
				EditorGUILayout.LabelField("Line #" + i);

				EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Name: ");
					currentLine.name = EditorGUILayout.TextArea(currentLine.name);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Text: ");
					currentLine.text = EditorGUILayout.TextArea(currentLine.text);
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.EndVertical(); 
			}

			EditorUtility.SetDirty(editedCutscene);
		}
	}
}