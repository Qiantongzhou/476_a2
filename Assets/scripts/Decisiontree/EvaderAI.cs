using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class EvaderAI : Tree
    {
        public LayerMask HidableLayers;
        [Range(-1,1)]
        public float Hidesenstivity;
        public Node root;
        public GameObject freezeboom;
        public GameObject altert;
        protected override Node SetupTree()
        {
            root = new Selector(new List<Node>
            {

                new Sequence(new List<Node>
                {
                    new getvisiontarget(transform,altert),
                    new evadervalidatetarget(transform,freezeboom),
                    new Hide(transform,HidableLayers,Hidesenstivity)
                }),
                new saveother(transform),
                new gotocoin(transform),
                new searching(transform),

            });
            
            return root;
        }
        
    }
}