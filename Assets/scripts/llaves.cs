using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class llaves : MonoBehaviour {

    public float rotSpeed = 10;

    void Update()
    {
        transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime);

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameObject.SetActive(false);
            Player.llaves.Push(gameObject);
        }
    }
}
