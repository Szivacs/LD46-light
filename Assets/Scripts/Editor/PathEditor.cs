using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Path))]
public class PathEditor : Editor
{
    Path path;

    private void OnSceneGUI()
    {
        Event guiEvent = Event.current;
        Vector2 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;

        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
        {
            Undo.RecordObject(path, "Add point");
            path.Add(mousePos);
        }

        for(int i = 0; i < path.points.Count-1; i++)
        {
            for (float t = 0.0f; t <= 1.0f; t += 0.1f)
            {
                Handles.color = Color.grey;
                Handles.CubeHandleCap(0, path.Evaluate(i, i+1, t), Quaternion.identity, 0.5f, EventType.Repaint);
            }
            Handles.DrawBezier(path.points[i].position, path.points[i + 1].position, path.points[i].right, path.points[i + 1].left, Color.red, null, 2.0f);
        }

        for(int i = 0; i < path.points.Count; i++)
        {
            if (guiEvent.type == EventType.MouseDown && guiEvent.button == 1 && Vector3.Distance(path.points[i].position, mousePos) < 1.0f)
            {
                path.points.RemoveAt(i);
                break;
            }

            Handles.color = Color.white;
            Handles.DrawLine(path.points[i].position, path.points[i].left);
            Handles.DrawLine(path.points[i].position, path.points[i].right);

            Handles.color = Color.red;
            Vector2 pos = Handles.FreeMoveHandle(path.points[i].position, Quaternion.identity, 1.0f, Vector3.zero, Handles.CylinderHandleCap);
            if(path.points[i].position != pos)
            {
                Undo.RecordObject(path, "Move point");
                path.Move(i, pos);
            }

            Handles.color = Color.cyan;
            pos = Handles.FreeMoveHandle(path.points[i].left, Quaternion.identity, 0.5f, Vector3.zero, Handles.CylinderHandleCap);
            if (path.points[i].left != pos)
            {
                Undo.RecordObject(path, "Move tangent point");
                path.points[i].left = pos;
                if (!guiEvent.shift)
                    path.points[i].right = path.points[i].position + (path.points[i].position - path.points[i].left);
            }

            Handles.color = Color.yellow;
            pos = Handles.FreeMoveHandle(path.points[i].right, Quaternion.identity, 0.5f, Vector3.zero, Handles.CylinderHandleCap);
            if (path.points[i].right != pos)
            {
                Undo.RecordObject(path, "Move tangent point");
                path.points[i].right = pos;
                if (!guiEvent.shift)
                    path.points[i].left = path.points[i].position + (path.points[i].position - path.points[i].right);
            }
        }
    }

    private void OnEnable()
    {
        path = (Path)target;
    }
}
