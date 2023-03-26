using BehaviorTree;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[CustomEditor(typeof(EvaderAI))]
public class EnemyMovementEditor : Editor
{
    private Collider[] Colliders = new Collider[10];

    private void OnSceneGUI()
    {
        Debug.Log("ongui");
        EvaderAI ai = (EvaderAI)target;
        if(ai == null)
        {
            return;
        }
        Hide movement= (Hide)ai.root.children[0].children[1];
        if (movement == null || movement.Player == null)
        {
            Debug.Log("movement is null");
            return;
        }

        int Hits = Physics.OverlapSphereNonAlloc(movement.Agent.transform.position, movement.transform.GetComponent<Vision>().viewRadius, Colliders, movement.HidableLayers);
        if (Hits > 0)
        {
            int HitReduction = 0;
            for (int i = 0; i < Hits; i++)
            {
                if (Vector3.Distance(Colliders[i].transform.position, movement.Player.position) < movement.MinEnenydistance)
                {
                    Handles.color = Color.red;
                    Handles.SphereHandleCap(GUIUtility.GetControlID(FocusType.Passive), Colliders[i].transform.position, Quaternion.identity, 0.25f, EventType.Repaint);
                    Handles.Label(Colliders[i].transform.position, $"{i} too close to target");
                    Colliders[i] = null;
                    HitReduction++;
                }
               

            }
            Hits -= HitReduction;

            System.Array.Sort(Colliders, movement.SortCollider);

            bool FoundTarget = false;

            for (int i = 0; i < Hits; i++)
            {
                if (NavMesh.SamplePosition(Colliders[i].transform.position, out NavMeshHit hit, 20f, movement.Agent.areaMask))
                {
                    if (!NavMesh.FindClosestEdge(hit.position, out hit, movement.Agent.areaMask))
                    {
                        Handles.color = Color.red;
                        Handles.SphereHandleCap(GUIUtility.GetControlID(FocusType.Passive), hit.position, Quaternion.identity, 0.25f, EventType.Repaint);
                        Handles.Label(hit.position, $"{i} (hit1) no edge found");
                    }

                    if (Vector3.Dot(hit.normal, (movement.Player.position - hit.position).normalized) < movement.HideSensitivity)
                    {
                        Handles.color = FoundTarget ? Color.yellow : Color.green;
                        FoundTarget = true;
                        Handles.SphereHandleCap(GUIUtility.GetControlID(FocusType.Passive), hit.position, Quaternion.identity, 0.25f, EventType.Repaint);
                        Handles.Label(hit.position, $"{i} (hit1) dot: {Vector3.Dot(hit.normal, (movement.Player.position - hit.position).normalized)}");
                    }
                    else
                    {
                        Handles.color = Color.red;
                        Handles.SphereHandleCap(GUIUtility.GetControlID(FocusType.Passive), hit.position, Quaternion.identity, 0.25f, EventType.Repaint);
                        Handles.Label(hit.position, $"{i} (hit1) dot: {Vector3.Dot(hit.normal, (movement.Player.position - hit.position).normalized)}");

                        if (NavMesh.SamplePosition(Colliders[i].transform.position - (movement.Player.position - hit.position).normalized * 15, out NavMeshHit hit2, 20f, movement.Agent.areaMask))
                        {
                            if (!NavMesh.FindClosestEdge(hit2.position, out hit2, movement.Agent.areaMask))
                            {
                                Handles.color = Color.red;
                                Handles.SphereHandleCap(GUIUtility.GetControlID(FocusType.Passive), hit2.position, Quaternion.identity, 0.25f, EventType.Repaint);
                                Handles.Label(hit.position, $"{i} (hit2) no edge found");
                            }

                            if (Vector3.Dot(hit2.normal, (movement.Player.position - hit2.position).normalized) < movement.HideSensitivity)
                            {
                                Handles.color = FoundTarget ? Color.yellow : Color.green;
                                FoundTarget = true;
                                Handles.SphereHandleCap(GUIUtility.GetControlID(FocusType.Passive), hit2.position, Quaternion.identity, 0.25f, EventType.Repaint);
                                Handles.Label(hit2.position, $"{i} (hit2) dot: {Vector3.Dot(hit2.normal, (movement.Player.position - hit2.position).normalized)}");
                            }
                            else
                            {
                                Handles.color = Color.red;
                                Handles.SphereHandleCap(GUIUtility.GetControlID(FocusType.Passive), hit2.position, Quaternion.identity, 0.25f, EventType.Repaint);
                                Handles.Label(hit2.position, $"{i} (hit2) dot: {Vector3.Dot(hit2.normal, (movement.Player.position - hit2.position).normalized)}");
                            }
                        }
                        else
                        {
                            Handles.color = Color.red;
                            Handles.SphereHandleCap(GUIUtility.GetControlID(FocusType.Passive), hit2.position, Quaternion.identity, 0.25f, EventType.Repaint);
                            Handles.Label(hit.position, $"{i} Hit 2 could not sampleposition");
                        }
                    }
                }
            }
        }
    }
}