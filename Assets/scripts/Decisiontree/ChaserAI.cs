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
        public GameObject questioning;
        public GameObject anger;
        public GameObject alert;
        public Node root;

        protected override Node SetupTree()
        {
            root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new getvisiontarget(transform,alert),
                //new checkotherchaser(transform,chaseLayers,chasesentivity),
                new chaservalidatetarget(transform,questioning),
                new chase(transform,useprediction)
            }),
            new Sequence(new List<Node>
            {
                new sharedinfo(transform),
                new chase(transform,useprediction)
            }),
            new gotocoinchaser(transform,anger),
            new searching(transform)
        }); 

            return root;
        }
    }

}