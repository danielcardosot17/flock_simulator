using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AlignmentBehaviorSO", menuName = "chicken_revolution/Flock/Behaviors/AlignmentBehaviorSO", order = 0)]
public class AlignmentBehaviorSO : FilteredFlockBehaviorSO
{
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        // if no neighbors, mantain current alignment
        if(context.Count == 0)
        {
            if(flock.verticalAxis == Vector3.up)
            {
                return agent.transform.forward;
            }
            else
            {
                return agent.transform.up;
            }
        }
        // Add all points together and average
        Vector3 alignmentMove = Vector3.zero;
        
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent,context);

        foreach(Transform item in filteredContext)
        {
            if(flock.verticalAxis == Vector3.up)
            {
                alignmentMove += item.transform.forward;
            }
            else
            {
                alignmentMove += item.transform.up;
            }
        }

        alignmentMove /= context.Count;
        
        // return Vector3.ProjectOnPlane(aligmentMove,Vector3.up);
        return alignmentMove;
    }
}
