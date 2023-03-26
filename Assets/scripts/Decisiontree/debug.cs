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
        //Debug.Log("ongui");
        EvaderAI ai = (EvaderAI)target;
        if(ai == null)
        {
            return;
        }
        Hide movement;
        if (ai.root != null)
        {
             movement = (Hide)ai.root.children[0].children[1];
            getvisiontarget v = (getvisiontarget)ai.root.children[0].children[0];
            if (v.none)
            {
                return;
            }
        }
        else
        {
            return ;
        }
        
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
[CustomEditor(typeof(ChaserAI))]
[CanEditMultipleObjects]
public class EnemyEditor : Editor
{
    private GUIStyle PlayerFontStyle;
    private GUIStyle ProjectionFontStyle;
    private void OnEnable()
    {
        PlayerFontStyle = new GUIStyle()
        {
            normal = new GUIStyleState()
            {
                textColor = Color.black
            },
            fontSize = 24
        };
        ProjectionFontStyle = new GUIStyle()
        {
            normal = new GUIStyleState()
            {
                textColor = Color.yellow
            },
            fontSize = 24
        };
    }

    private void OnSceneGUI()
    {
        ChaserAI x = (ChaserAI)target;
        
        chase movement;
        if(x.root == null)
        {
            return;
        }
        else
        {
            movement = (chase)x.root.children[0].children[1];
        }
        

        if (movement != null)
        {
            Debug.Log("gui");
            if (movement.Agent.hasPath)
            {
                Handles.color = Color.green;
                Handles.DrawSolidDisc(movement.Agent.destination, Vector3.up, 0.25f);
                Handles.Label(movement.Agent.destination, "Destination");
            }

            Handles.color = Color.black;
            Handles.DrawLine(movement.transform.position, movement.target.position);
            Handles.Label(Vector3.Lerp(movement.target.position, movement.transform.position, 0.5f), "Player Position", PlayerFontStyle);
            Handles.DrawSolidDisc(movement.target.position, Vector3.up, 0.25f);
            Vector3 targetPosition = movement.target.position + movement.AverageVelocity * movement.movementprodictiontime;

            Handles.color = Color.yellow;
            Handles.DrawLine(movement.transform.position, targetPosition);
            Handles.DrawSolidDisc(targetPosition, Vector3.up, 0.25f);

            //Handles.Label(Vector3.Lerp(targetPosition, movement.transform.position, 0.5f), "Projected Position", ProjectionFontStyle);
            Handles.Label(targetPosition, "Predicted Position", ProjectionFontStyle);
            Vector3 directionToTarget = (targetPosition - movement.transform.position).normalized;
            Vector3 directionToPlayer = (movement.target.position - movement.transform.position).normalized;

            Handles.ArrowHandleCap(EditorGUIUtility.GetControlID(FocusType.Passive), movement.transform.position, Quaternion.LookRotation(directionToTarget), 2f, EventType.Repaint);
            Handles.color = Color.black;
            Handles.ArrowHandleCap(EditorGUIUtility.GetControlID(FocusType.Passive), movement.transform.position, Quaternion.LookRotation(directionToPlayer), 2f, EventType.Repaint);

            float dot = Vector3.Dot(directionToPlayer, directionToTarget);
            Handles.Label(movement.transform.position - (movement.Agent.velocity.normalized), $"Dot: {dot:N2}", PlayerFontStyle);

            Vector3[] corners = movement.Agent.path.corners;
            for (int i = 1; i < corners.Length; i++)
            {
                Handles.color = Color.cyan;
                Handles.DrawLine(corners[i - 1], corners[i], 3);
            }
        }
    }
}
//[CustomEditor(typeof(ChaserAI))]
//public class ChaserMovementEditor : Editor
//{
//    private Collider[] Colliders = new Collider[10];

//    private void OnSceneGUI()
//    {
//        Debug.Log("ongui");
//        ChaserAI ai = (ChaserAI)target;
//        if (ai == null)
//        {
//            return;
//        }
//        checkotherchaser movement;
//        if (ai.root != null)
//        {
//            movement = (checkotherchaser)ai.root.children[0].children[1];
//            getvisiontarget v = (getvisiontarget)ai.root.children[0].children[0];
//            if (v.none)
//            {
//                return;
//            }
//        }
//        else
//        {
//            return;
//        }

//        if (movement == null || movement.Player == null)
//        {
//            Debug.Log("movement is null");
//            return;
//        }

//        int Hits = Physics.OverlapSphereNonAlloc(movement.Agent.transform.position, movement.transform.GetComponent<Vision>().viewRadius, Colliders, movement.layer);
//        if (Hits > 0)
//        {
//            int HitReduction = 0;
//            for (int i = 0; i < Hits; i++)
//            {
//                if (Vector3.Distance(Colliders[i].transform.position, movement.Player.position) < movement.MinEnenydistance)
//                {
//                    Handles.color = Color.red;
//                    Handles.SphereHandleCap(GUIUtility.GetControlID(FocusType.Passive), Colliders[i].transform.position, Quaternion.identity, 0.25f, EventType.Repaint);
//                    Handles.Label(Colliders[i].transform.position, $"{i} too close to target");
//                    Colliders[i] = null;
//                    HitReduction++;
//                }


//            }
//            Hits -= HitReduction;

//            System.Array.Sort(Colliders, movement.SortCollider);

//            bool FoundTarget = false;

//            for (int i = 0; i < Hits; i++)
//            {
//                if (NavMesh.SamplePosition(Colliders[i].transform.position, out NavMeshHit hit, 20f, movement.Agent.areaMask))
//                {
//                    if (!NavMesh.FindClosestEdge(hit.position, out hit, movement.Agent.areaMask))
//                    {
//                        Handles.color = Color.red;
//                        Handles.SphereHandleCap(GUIUtility.GetControlID(FocusType.Passive), hit.position, Quaternion.identity, 0.25f, EventType.Repaint);
//                        Handles.Label(hit.position, $"{i} (hit1) no edge found");
//                    }

//                    if (Vector3.Dot(hit.normal, (movement.Player.position - hit.position).normalized) < movement.chaseSensitivity)
//                    {
//                        Handles.color = FoundTarget ? Color.yellow : Color.green;
//                        FoundTarget = true;
//                        Handles.SphereHandleCap(GUIUtility.GetControlID(FocusType.Passive), hit.position, Quaternion.identity, 0.25f, EventType.Repaint);
//                        Handles.Label(hit.position, $"{i} (hit1) dot: {Vector3.Dot(hit.normal, (movement.Player.position - hit.position).normalized)}");
//                    }
//                    else
//                    {
//                        Handles.color = Color.red;
//                        Handles.SphereHandleCap(GUIUtility.GetControlID(FocusType.Passive), hit.position, Quaternion.identity, 0.25f, EventType.Repaint);
//                        Handles.Label(hit.position, $"{i} (hit1) dot: {Vector3.Dot(hit.normal, (movement.Player.position - hit.position).normalized)}");

//                        if (NavMesh.SamplePosition(Colliders[i].transform.position - (movement.Player.position - hit.position).normalized * 15, out NavMeshHit hit2, 20f, movement.Agent.areaMask))
//                        {
//                            if (!NavMesh.FindClosestEdge(hit2.position, out hit2, movement.Agent.areaMask))
//                            {
//                                Handles.color = Color.red;
//                                Handles.SphereHandleCap(GUIUtility.GetControlID(FocusType.Passive), hit2.position, Quaternion.identity, 0.25f, EventType.Repaint);
//                                Handles.Label(hit.position, $"{i} (hit2) no edge found");
//                            }

//                            if (Vector3.Dot(hit2.normal, (movement.Player.position - hit2.position).normalized) < movement.chaseSensitivity)
//                            {
//                                Handles.color = FoundTarget ? Color.yellow : Color.green;
//                                FoundTarget = true;
//                                Handles.SphereHandleCap(GUIUtility.GetControlID(FocusType.Passive), hit2.position, Quaternion.identity, 0.25f, EventType.Repaint);
//                                Handles.Label(hit2.position, $"{i} (hit2) dot: {Vector3.Dot(hit2.normal, (movement.Player.position - hit2.position).normalized)}");
//                            }
//                            else
//                            {
//                                Handles.color = Color.red;
//                                Handles.SphereHandleCap(GUIUtility.GetControlID(FocusType.Passive), hit2.position, Quaternion.identity, 0.25f, EventType.Repaint);
//                                Handles.Label(hit2.position, $"{i} (hit2) dot: {Vector3.Dot(hit2.normal, (movement.Player.position - hit2.position).normalized)}");
//                            }
//                        }
//                        else
//                        {
//                            Handles.color = Color.red;
//                            Handles.SphereHandleCap(GUIUtility.GetControlID(FocusType.Passive), hit2.position, Quaternion.identity, 0.25f, EventType.Repaint);
//                            Handles.Label(hit.position, $"{i} Hit 2 could not sampleposition");
//                        }
//                    }
//                }
//            }
//        }
//    }
//}