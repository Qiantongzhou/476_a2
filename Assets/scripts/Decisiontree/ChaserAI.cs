using System.Collections.Generic;
using BehaviorTree;

public class ChaserAI : Tree
{
    

    public static float speed = 2f;
    public static float fovRange = 6f;
    public static float attackRange = 1f;

    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new getvisiontarget(transform),
                new chase(transform)
            }),
            new gotocoin(transform),
            new searching(transform)
        }) ;

        return root;
    }
}

