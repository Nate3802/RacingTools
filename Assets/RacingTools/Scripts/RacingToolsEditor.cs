using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RacingTools))]
public class RacingToolsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RacingTools racingTools = (RacingTools)target;

        if (!racingTools.TestObstruction())
        {
            GUI.backgroundColor = Color.green;
        }
        else
        {
            GUI.backgroundColor = Color.gray;
        }

        foreach(Object obj in racingTools.trackPrefabs)
        {
            if(GUILayout.Button("Add " + racingTools.ConvertName(obj.name)))
            {
                racingTools.AddPiece(racingTools.trackPrefabs.IndexOf(obj));
            }
        }

        if (racingTools.TrackPiecesNum() > 0)
        {
            GUI.backgroundColor = Color.red;
        }
        else
        {
            GUI.backgroundColor = Color.gray;
        }
        
        if(GUILayout.Button("Delete Last Track"))
        {
            racingTools.DeleteLastPiece();
        }

        if (racingTools.CheckCircuit())
        {
            EditorGUILayout.LabelField("Circuit Complete");
        }
    }
}
