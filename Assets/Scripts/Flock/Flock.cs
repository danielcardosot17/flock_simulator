using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockAgent agentPrefab;
    List<FlockAgent> agents = new List<FlockAgent>();
    public FlockBehaviorSO behavior;

    [Range(10, 500)]
    public int startingCount = 200;
    
    [Range(0.01f, 5f)]
    public float AgentDensity = 0.1f;

    [Range(1f, 100f)]
    public float driveFactor = 10f;

    [Range(1f, 100f)]
    public float maxSpeed = 5f;
    
    [Range(1f, 10f)]
    public float neighborRadius = 2f;
    
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.8f;

    public Vector3 verticalAxis = Vector3.up;
    public bool planeMoveOnly = true;


    float squareMaxSpeed;
    float squareNeigborRadius;
    float squareAvoidanceRadius;

    public float SquareAvoidanceRadius { get => squareAvoidanceRadius; private set => squareAvoidanceRadius = value; }




    // Start is called before the first frame update
    void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeigborRadius = neighborRadius * neighborRadius;
        SquareAvoidanceRadius = squareNeigborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;
        var spawnPosition = Vector3.zero;
        var spawnRotation = Quaternion.Euler(Vector3.zero);
        if(planeMoveOnly)
        {
            for(int i =0; i < startingCount; i++)
            {
                spawnPosition = Vector3.ProjectOnPlane(Random.insideUnitCircle * startingCount * AgentDensity,verticalAxis) + verticalAxis * 2;
                spawnRotation = Quaternion.Euler(verticalAxis * Random.Range(0f, 360f));
                FlockAgent newAgent = Instantiate(
                    agentPrefab,
                    spawnPosition,
                    spawnRotation,
                    transform
                );
                newAgent.name = "Agent " + i;
                newAgent.Initialize(this);
                agents.Add(newAgent);
            }
        }
        else
        {
            for(int i =0; i < startingCount; i++)
            {
                spawnPosition = Random.insideUnitSphere * startingCount * AgentDensity;
                spawnRotation = Quaternion.Euler(verticalAxis * Random.Range(0f, 360f));
                FlockAgent newAgent = Instantiate(
                    agentPrefab,
                    spawnPosition,
                    spawnRotation,
                    transform
                );
                newAgent.name = "Agent " + i;
                newAgent.Initialize(this);
                agents.Add(newAgent);
            }
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(FlockAgent agent in agents)
        {
            List<Transform> context = GetNearbyObjects(agent);
            
            // agent.GetComponentInChildren<Renderer>().material.color = Color.Lerp(Color.white, Color.red, context.Count/5f);
            
            Vector3 move = behavior.CalculateMove(agent, context, this);
            move *= driveFactor;
            if(move.sqrMagnitude > squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }
            if(planeMoveOnly)
            {
                agent.Move(Vector3.ProjectOnPlane(move,verticalAxis));
            }
            else
            {
                agent.Move(move);
            }
        }
    }

    List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider[] contextColliders = Physics.OverlapSphere(agent.transform.position, neighborRadius);
        foreach(Collider c in contextColliders)
        {
            if(c != agent.AgentCollider)
            {
                context.Add(c.transform);
            }
        }

        return context;
    }
}
