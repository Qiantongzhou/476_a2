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

        protected override Node SetupTree()
        {
            Node root = new Selector(new List<Node>
            {
                new Sequence(new List<Node>
                {
                    new getvisiontarget(transform),
                    new Hide(transform,HidableLayers,Hidesenstivity)
                }),
                new searching(transform)
            });

            return root;
        }
    }
}