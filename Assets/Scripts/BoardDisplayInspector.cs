using UnityEngine;
using UnityEditor;
using System.Collections;


[CustomEditor(typeof(BoardDisplay))]
public class BoardDisplayInspector : Editor
{

    private static bool backgroundLayoutFoldout = false;

    SerializedProperty backgroundLayoutEditor;
    SerializedProperty layoutHeight;
    SerializedProperty layoutWidth;

    void OnEnable()
    {
        backgroundLayoutEditor = serializedObject.FindProperty("backgroundLayoutEditor");
        layoutHeight = serializedObject.FindProperty("layoutHeight");
        layoutWidth = serializedObject.FindProperty("layoutWidth");
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        serializedObject.Update();

        backgroundLayoutFoldout = EditorGUILayout.Foldout(backgroundLayoutFoldout, "Board Background");
        if (backgroundLayoutFoldout)
        {
            DrawBackground();
        }

        serializedObject.ApplyModifiedProperties();
    }

    void DrawBackground()
    {
        for (int i = layoutHeight.intValue - 1; i >= 0; i--)
        {
            int spaceType;
            GUILayout.BeginHorizontal();
            for (int j = 0; j < layoutWidth.intValue; j++)
            {
                spaceType = backgroundLayoutEditor.GetArrayElementAtIndex(j + i * layoutWidth.intValue).enumValueIndex;
                switch (spaceType)
                {
                    case 0:
                        GUI.color = GUI.contentColor;
                        break;
                    case 1:
                        GUI.color = Color.black;
                        break;
                    case 2:
                        GUI.color = Color.red;
                        break;

                }
                if (GUILayout.Button(""))
                {
                    if (spaceType < 2)
                    {
                        backgroundLayoutEditor.GetArrayElementAtIndex(j + i * layoutWidth.intValue).enumValueIndex++;
                    }
                    else
                        backgroundLayoutEditor.GetArrayElementAtIndex(j + i * layoutWidth.intValue).enumValueIndex = 0;
                }
            }
            GUILayout.EndHorizontal();
        }

        GUI.color = GUI.contentColor;
        if (GUILayout.Button("Add Row"))
        {
            layoutHeight.intValue++;
            for (int i = 0; i < layoutWidth.intValue; i++)
            {
                backgroundLayoutEditor.InsertArrayElementAtIndex(0);
            }
        }
        if (GUILayout.Button("Add Column"))
        {
            layoutWidth.intValue++;
            for (int i = 0; i < layoutHeight.intValue; i++)
            {
                backgroundLayoutEditor.InsertArrayElementAtIndex(layoutWidth.intValue * i);
            }
        }
        if (GUILayout.Button("DeleteRow"))
        {
            for (int i = 0; i < layoutWidth.intValue; i++)
            {
                backgroundLayoutEditor.DeleteArrayElementAtIndex(0);
            }
            layoutHeight.intValue--;
        }
        if (GUILayout.Button("DeleteColumn"))
        {
            int maxSize = backgroundLayoutEditor.arraySize;
            for (int i = 0; i < layoutHeight.intValue; i++)
            {
                backgroundLayoutEditor.DeleteArrayElementAtIndex(maxSize - layoutWidth.intValue * (i+1) );
            }
            layoutWidth.intValue--;
        }
    }

}
