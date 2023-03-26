using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
namespace BehaviorTree
{
    public class ChaserAI : Tree
    {

        public LayerMask chaseLayers;
        [Range(-1, 1)]
        public float chasesentivity;
        public bool useprediction = true;
       
        public Node root;

        protected override Node SetupTree()
        {
            root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new getvisiontarget(transform),
                //new checkotherchaser(transform,chaseLayers,chasesentivity),
                new chase(transform,useprediction)
            }),
            new Sequence(new List<Node>
            {
                new sharedinfo(transform),
                new chase(transform,useprediction)
            }),
            new gotocoin(transform),
            new searching(transform)
        }); 

            return root;
        }
    }

}