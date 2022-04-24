using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public static event System.Action NotFinish;

    public event System.Action OnReachedEndOfLevel;

    public float moveSpeed = 7;
    public float smoothMoveTime = .1f;
    public float turnSpeed = 8;

    public static bool congActivado;

    public static Stack<GameObject> llaves = new Stack<GameObject>(3);

    float angle;
    float smoothInputMagnitude;
    float smoothMoveVelocity;
    Vector3 velocity;

    new Rigidbody rigidbody;
    bool disabled;

    public GameObject llave1;
    public GameObject llave2;
    public GameObject llave3;

    // Use this for initialization
    void Start () {

        rigidbody = GetComponent<Rigidbody>();
        Guard.OnGuardHasSpottedPlayer += Disable;
        llave1 = GameObject.Find("Llave1");
        llave2 = GameObject.Find("Llave2");
        llave3 = GameObject.Find("Llave3");

    }
	
	// Update is called once per frame
	void Update () {

        Vector3 inputDirection = Vector3.zero;
        if (!disabled)
        {
            inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        }
        float inputMagnitude = inputDirection.magnitude;
        smoothInputMagnitude = Mathf.SmoothDamp(smoothInputMagnitude, inputMagnitude, ref smoothMoveVelocity, smoothMoveTime);

        float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
        angle = Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * turnSpeed *inputMagnitude);

        velocity = transform.forward * moveSpeed * smoothInputMagnitude;


        if (disabled == false)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                if (congActivado == false)
                {
                    congelar();
                }
                else
                {
                    Debug.Log("Esta en cooldown");
                }
            }
        }
        
    }

    void OnTriggerEnter(Collider hitCollider)
    {

        GameObject[] Array = new GameObject[3];
        llaves.CopyTo(Array, 0);

        if (Array[0].Equals(llave3) && Array[1].Equals(llave2) && Array[2].Equals(llave1))
        {
            if (hitCollider.tag == "Finish" && ScoreManager.Instance.CurrentScore == 3)
            {
                Disable();
                if (OnReachedEndOfLevel != null)
                {
                    OnReachedEndOfLevel();
                }
            }
            Debug.Log("Ganaste");
        }
        else
        {
            Disable();
            if (NotFinish != null)
            {
                NotFinish();
            }
        }
            foreach (GameObject a in llaves)
        {
            Debug.Log(a.ToString());
        }

    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Disable();
            Time.timeScale = 0;
        }
        if(collision.gameObject.tag == "llave")
        {
            ScoreManager.Instance.IncreaseScore();
        }
    }

    public void Disable()
    {
        disabled = true;
    }

    void FixedUpdate()
    {
        rigidbody.MoveRotation(Quaternion.Euler(Vector3.up * angle));
        rigidbody.MovePosition(rigidbody.position + velocity * Time.deltaTime);
       
    }

    void OnDestroy()
    {
        Guard.OnGuardHasSpottedPlayer -= Disable;
    }

    public void congelar()
    {
        congActivado = true;
        Cooldown.tiempodeCooldown = 30;
        Cooldown.moveAgain = 5;
        Cooldown.activado = true;
        Guard.pararguardias = true;
        Guard2.pararguardias = true;
    }
}
