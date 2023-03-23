using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Vision))]
public class VisionEditor : Editor
{
    private void OnSceneGUI()
    {
        Vision v=(Vision)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(v.transform.position, Vector3.up, Vector3.forward, 360, v.viewRadius);
        Handles.color = Color.yellow;
        Handles.DrawWireArc(v.transform.position, Vector3.up, Vector3.forward, 360, v.viewRadius/5);
        Vector3 viewAngle1 = v.DirFromAngle(-v.viewAngle / 2, false);
        Vector3 viewAngle2 = v.DirFromAngle(v.viewAngle / 2, false);

        Handles.DrawLine(v.transform.position, v.transform.position + viewAngle1*  v.viewRadius);
        Handles.DrawLine(v.transform.position, v.transform.position + viewAngle2 * v.viewRadius);

        Handles.color= Color.red;
        
        foreach(Transform target in v.targets)
        {
            Handles.DrawLine(v.transform.position, target.position);
        }
    }
}
