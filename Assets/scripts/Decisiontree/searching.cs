using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorTree
{
    public class searching : Node
    {
        private Transform _transform;
        private Animator _animator;
        private Vector3 _waypoints;

        

        private float _waitTime = 1f; // in seconds
        private float _waitCounter = 0f;
        private bool _waiting = false;
        private bool _setdestination=false;
        

        public searching(Transform transform)
        {
            _transform = transform;
            _animator = transform.GetComponent<Animator>();
            while (true)
            {
                Vector3 mp = new Vector3(UnityEngine.Random.value * Maze.mazegen.GetLength(0) * 10, 0, UnityEngine.Random.value * Maze.mazegen.GetLength(1) * 10);
                Maze.getXY(mp, out int x, out int y);

                if (Maze.mazegen[x, y] == 0&&x< Maze.mazegen.GetLength(0)&&y<Maze.mazegen.GetLength(1))
                {
                    Debug.Log(x + "" + y);
                    _waypoints = new Vector3(x*10, 0, y*10);
                    break;
                }
            }
            CoroutineRunner.Instance.Run(cleardestination());

            _setdestination = true;
        }
        IEnumerator cleardestination()
        {
            while (true)
            {
               
                yield return new WaitForSeconds(20f);
                _waitCounter = 0f;
                _waiting = true;
                _setdestination = true;
                while (true)
                {
                    Vector3 mp = new Vector3(UnityEngine.Random.value * (Maze.mazegen.GetLength(0)-2) * 10, 0, UnityEngine.Random.value * (Maze.mazegen.GetLength(1)-2) * 10);
                    Maze.getXY(mp, out int x, out int y);
                    if (Maze.mazegen[x, y] == 0)
                    {
                        _waypoints = new Vector3(x * 10, 0, y * 10);
                        break;
                    }
                    
                }
                Debug.Log("reset new waypoint");

                _animator.SetBool("Walking", false);
            }
        }

        public override NodeState Evaluate()
        {
            if (_waiting)
            {
                _waitCounter += Time.deltaTime;
                if (_waitCounter >= _waitTime)
                {
                    _waiting = false;
                    _animator.SetBool("Walking", true);
                }
            }
            else
            {
                Vector3 wp = _waypoints;
                
                if (Vector3.Distance(_transform.position, wp) < 1f)
                {
                    _transform.position = wp;
                    _waitCounter = 0f;
                    _waiting = true;
                    _setdestination= true;
                    while (true)
                    {
                        Vector3 mp = new Vector3(UnityEngine.Random.value * Maze.mazegen.GetLength(0) * 10, 0, UnityEngine.Random.value * Maze.mazegen.GetLength(1) * 10);
                        Maze.getXY(mp, out int x, out int y);
                        if (Maze.mazegen[x, y] == 0)
                        {
                            _waypoints = new Vector3(x * 10, 0, y * 10);
                            break;
                        }
                        Debug.Log("new waypoint");
                    }
                    
                    
                    _animator.SetBool("Walking", false);
                }
                else
                {
                    
                    if (_setdestination)
                    {
                        Debug.Log("go to point");
                        _transform.GetComponent<NavMeshAgent>().destination = _waypoints;
                        _setdestination= false;
                    }
                }
            }


            state = NodeState.RUNNING;
            return state;
        }
    }
}