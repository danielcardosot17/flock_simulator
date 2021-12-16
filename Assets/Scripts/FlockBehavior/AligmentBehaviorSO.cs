using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AligmentBehaviorSO", menuName = "chicken_revolution/Flock/Behaviors/AligmentBehaviorSO", order = 0)]
public class AligmentBehaviorSO : FilteredFlockBehaviorSO
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
        Vector3 aligmentMove = Vector3.zero;
        
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent,context);

        foreach(Transform item in filteredContext)
        {
            if(flock.verticalAxis == Vector3.up)
            {
                aligmentMove += item.transform.forward;
            }
            else
            {
                aligmentMove += item.transform.up;
            }
        }

        aligmentMove /= context.Count;
        
        // return Vector3.ProjectOnPlane(aligmentMove,Vector3.up);
        return aligmentMove;
    }
}
