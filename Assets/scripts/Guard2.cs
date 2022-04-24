using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard2 : MonoBehaviour {

    public static event System.Action OnGuardHasSpottedPlayer;

    public float speed = 5f;
    public float rotSpeed = 10f;
    public float waitTime = .3f;
    public float turnSpeed = 90;
    public float timeToSpotPlayer = .5f;
    public float followTime = 5f;

    public Light spotlight;
    public float viewDistance;
    public LayerMask viewMask;
    public static bool pararguardias;

    float viewAngle;
    float playerVisibleTimer;

    Transform player;
    Color originalSpotlightColour;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        viewAngle = spotlight.spotAngle;
        originalSpotlightColour = spotlight.color;
    }

    void Update()
    {
        if (Guard3.alerta == true)
        {
            followTime -= Time.deltaTime;
            if(followTime <= 0)
            {
                Guard3.alerta = false;
                followTime = 5;
            }
        }
        if (CanSeePlayer() && pararguardias == false)
        {
            playerVisibleTimer += Time.deltaTime;
            if (playerVisibleTimer >= timeToSpotPlayer)
            {
                transform.LookAt(player);
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }
        }
        else
        {
            playerVisibleTimer -= Time.deltaTime;
            transform.Rotate(Vector3.up* rotSpeed * Time.deltaTime);
        }
        playerVisibleTimer = Mathf.Clamp(playerVisibleTimer,0, timeToSpotPlayer);
        spotlight.color = Color.Lerp(originalSpotlightColour, Color.red, playerVisibleTimer/timeToSpotPlayer);
    }

    bool CanSeePlayer()
    {
        if (Guard3.alerta == true)
        {
            return true;
        }
        if (Vector3.Distance(transform.position, player.position) < viewDistance)
        {
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, dirToPlayer); 
            if(angleBetweenGuardAndPlayer < viewAngle / 2f)
            {
                if (!Physics.Linecast(transform.position, player.position, viewMask))
                {
                    return true;
                }
            }
        }
        return false;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (OnGuardHasSpottedPlayer != null)
            {
                OnGuardHasSpottedPlayer();
            }
        }
    }
}
