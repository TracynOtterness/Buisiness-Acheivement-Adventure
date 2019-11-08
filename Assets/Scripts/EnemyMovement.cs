using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    [SerializeField] float MoveSpeed = 1f;

    Rigidbody2D myRigidbody;
    [SerializeField] Collider2D myCollider;
   
    float movementScale = 1f;

	// Use this for initialization
	void Start () {
        myRigidbody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        Move();
    }

    private void Move()
    {
        myRigidbody.velocity = new Vector2(MoveSpeed * movementScale, myRigidbody.velocity.y);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        movementScale *= -1f;
        transform.localScale = new Vector2(-Mathf.Sign(myRigidbody.velocity.x), 1f);
    }
}
