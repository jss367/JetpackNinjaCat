﻿using UnityEngine;
using System.Collections;

public class CatController : MonoBehaviour {
    public float jetpackForce = 75.0f;
    public float forwardMovementSpeed = 3.0f;

    public Transform groundCheckTransform;
    private bool grounded;
    public LayerMask groundCheckLayerMask;
    Animator animator;

    public ParticleSystem jetpack;

    private bool dead = false;
    void OnTriggerEnter2D(Collider2D collider)
    {
        HitByLaser(collider);
    }
 
    void HitByLaser(Collider2D laserCollider)
    {
        dead = true;

        animator.SetBool("dead", true);
    }

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
        bool jetpackActive = Input.GetButton("Fire1");

        jetpackActive = jetpackActive && !dead;

        if (jetpackActive)
        {
            rigidbody2D.AddForce(new Vector2(0, jetpackForce));
        }

        if (!dead)
        {
            Vector2 newVelocity = rigidbody2D.velocity;
            newVelocity.x = forwardMovementSpeed;
            rigidbody2D.velocity = newVelocity;
        }

        UpdateGroundedStatus();

        AdjustJetpack(jetpackActive);
    }

    void UpdateGroundedStatus()
    {
        grounded = Physics2D.OverlapCircle(groundCheckTransform.position, 0.1f, groundCheckLayerMask);

        animator.SetBool("grounded", grounded);
    }

    void AdjustJetpack(bool jetpackActive)
    {
        jetpack.enableEmission = !grounded;
        jetpack.emissionRate = jetpackActive ? 300.0f : 75.0f;
    }
}
