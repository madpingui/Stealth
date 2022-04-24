using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard3 : MonoBehaviour {

    public static event System.Action OnGuardHasSpottedPlayer;

    public float speed = 5f;
    public float rotSpeed = 10f;
    public float waitTime = .3f;
    public float turnSpeed = 90;
    public float timeToSpotPlayer = .5f;
    public static bool alerta;

    public Light spotlight;
    public float viewDistance;
    public LayerMask viewMask;

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

        if (CanSeePlayer())
        {
            playerVisibleTimer += Time.deltaTime;
            if (playerVisibleTimer >= timeToSpotPlayer)
            {
                alerta = true;
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
