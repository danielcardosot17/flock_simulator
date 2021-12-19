using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PlayerMovement : MonoBehaviour
{
    [Range(1f,10f)] public float moveSpeed = 5;

    [Range(1f,3f)] public float turboMultiplier = 1.2f;
    [Range(1f,10f)] public float explosionCooldown = 5f;
    [Range(1f,10f)] public float explosionRadius = 5f;
    [Range(1f,10f)] public float colorChangeCooldown = 5f;

    public LayerMask obstacleLayer;
    public AudioEventSO explosionSFX;
    public GameObject explosionPrefab;

    private Vector3 direction = Vector3.zero;
    private float multiplier = 1;
    private float explosionTimer = 0;
    private float colorChangeTimer = 0;

    private Color playerColor;
    private SpriteRenderer spriteRenderer;
    private bool justExploded = false;

    private PlayerHandler handler;

    public PlayerHandler Handler { get => handler; private set => handler = value; }

    private void Awake() {
        justExploded = false;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    // Update is called once per frame
    public void Initialize(PlayerHandler handler){
        Handler = handler;
    }
    private void Start() {
        ChangeSpriteColor(playerColor);
        justExploded = true;
        colorChangeTimer = colorChangeCooldown/2;
        explosionTimer = explosionCooldown/2;
    }

    void Update()
    {
        if(explosionTimer > 0)
        {
            explosionTimer -= Time.deltaTime;
        }

        if(colorChangeTimer > 0 )
        {
            colorChangeTimer -= Time.deltaTime;
        }
        else
        {
            if(justExploded)
            {
                ChangeSpriteColor(Color.white);
                justExploded = false;
            }
        }

        // check to see fi is out of bounds
        if(IsOutOfBounds()) ReturnToOrigin();

        // direction = Vector3.right * Input.GetAxis("Horizontal") + Vector3.up * Input.GetAxis("Vertical");
        // multiplier = Input.GetKey(KeyCode.Space) ? turboMultiplier : 1;
        if(direction.magnitude > 0) Move(direction, multiplier);    
    }

    private void ReturnToOrigin()
    {
        transform.position = Vector3.zero;
    }

    private bool IsOutOfBounds()
    {
        if(Mathf.Abs(transform.position.x) > 35 || Mathf.Abs(transform.position.y) > 20 || Mathf.Abs(transform.position.z) > 0) return true;
        return false;
    }

    private void Move(Vector3 direction, float multiplier)
    {
        transform.up = direction.normalized;
        transform.position += direction.normalized * multiplier * moveSpeed * Time.deltaTime;
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        direction = new Vector3(context.ReadValue<Vector2>().x, context.ReadValue<Vector2>().y, 0);
    }

    public void OnTurbo(InputAction.CallbackContext context)
    {
        // multiplier = context.action.triggered ? turboMultiplier : 1;
        // now it will explode!!!
        if(context.action.triggered && (explosionTimer <= 0)) Explode();
    }

    private void Explode()
    {
        explosionSFX.Raise();
        ChangeSpriteColor(playerColor);
        ExplosionAnimation();
        DestroyObjectsInArea();
        justExploded = true;
        colorChangeTimer = colorChangeCooldown;
        explosionTimer = explosionCooldown;
    }

    private void DestroyObjectsInArea()
    {
        var objectsAffected = GetNearbyObjects();
        foreach(var obj in objectsAffected)
        {
            var agent = obj.GetComponent<FlockAgent>();
            var otherPlayer = obj.GetComponent<PlayerMovement>();
            if(agent)
            {
                agent.DestroyThisAgent();
            }
            else if(otherPlayer)
            {
                Handler.DestroySinglePlayerAndRemoveFromList(otherPlayer.gameObject);
            }
        }
    }

    private void ExplosionAnimation()
    {
        var explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        ParticleSystem.MainModule settings = explosion.GetComponent<ParticleSystem>().main;
        Color minColor = new Color(playerColor.r/2,playerColor.g/2,playerColor.b/2);
        settings.startColor = new ParticleSystem.MinMaxGradient(minColor, playerColor);
    }

    public void SetPlayerColor(Color color)
    {
        playerColor = color;
    }

    private void ChangeSpriteColor(Color color)
    {
        spriteRenderer.color = color;
    }

    // Start is called before the first frame update
    private void OnCollisionStay(Collision other) {
        if(obstacleLayer == (obstacleLayer | 1 << other.gameObject.layer))
        {
            if(direction.magnitude > 0)
            {
                Vector3 closestPoint = other.gameObject.GetComponent<Collider>().ClosestPoint(transform.position);
                var normalVector = (transform.position - closestPoint).normalized;
                Move(normalVector, multiplier);
            } 
        }
    }

    List<Transform> GetNearbyObjects()
    {
        List<Transform> context = new List<Transform>();
        Collider[] contextColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        var playerCollider = GetComponent<Collider>();
        foreach(Collider c in contextColliders)
        {
            if(c != playerCollider)
            {
                context.Add(c.transform);
            }
        }

        return context;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position,explosionRadius);
    }
}
