using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {
    Rigidbody rb;
    
    void Start() {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update() {
    
    }

    void FixedUpdate() {
        rb.MovePosition(new Vector3(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 0, 50)).x, gameObject.transform.position.y, 0));
    }
}
