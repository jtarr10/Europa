using System.Collections;
using System.Collections.Generic;
using static System.Math;
using UnityEngine;

public class Boid : MonoBehaviour {

    private float speedLimit = 2f;
    private Vector2 acceleration = new Vector2(0f, 0f);
    public Rigidbody2D rb;
    private SpriteRenderer sprite;
    public float perceptionDistance = 1f;
    public float maximumSteeringForce = 0.5f;

	// Use this for initialization
	void Start () {
        this.rb = gameObject.GetComponent<Rigidbody2D>();
        this.sprite = gameObject.GetComponent<SpriteRenderer>();

        // Starting with a random velocity vector
        this.rb.velocity = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
	}
	
	// Update is called once per frame
	void Update () {

    }

    private void FixedUpdate()
    {
        // add the velocity modifiers to the current velocity
        this.rb.velocity = new Vector2((float)Min(this.rb.velocity.x + this.acceleration.x, this.speedLimit), (float)Min(this.rb.velocity.y + this.acceleration.y, this.speedLimit));

        // Rotate the sprite to point in the direction of travel
        double rotationAngle = (180 / PI) * Atan2(this.rb.velocity.normalized.y, this.rb.velocity.normalized.x);
        this.rb.rotation = (float) rotationAngle;

        // Flip the sprite if it is moving backwards
        if (Abs(this.rb.rotation) > 90 && !this.sprite.flipY)
            this.sprite.flipY = true;
        else if (Abs(this.rb.rotation) < 90 && this.sprite.flipY)
            this.sprite.flipY = false;

    }
    /// <summary>
    /// Call this per frame to update the boid acceleration according to boid's algorithm
    /// </summary>
    /// <param name="boids"></param>
    public void Flock(List<GameObject> boids)
    {
        Vector2 alignmentSteering = Align(boids);
        this.acceleration = alignmentSteering;
    }

    private Vector2 Align(List<GameObject> localFish)
    {   
        Vector2 steeringForce = new Vector2(0f, 0f);
        int totalInRange = 0;

        foreach(GameObject other in localFish)
        {
            Boid boid = other.GetComponent<Boid>();
            float dist = calculateBoidDistance(boid);
            if (other != this && dist < perceptionDistance)
            {
                steeringForce += boid.rb.velocity;
                totalInRange++;
            }
        }
        if (totalInRange > 0)
        {
            steeringForce /= totalInRange;
            steeringForce -= this.rb.velocity;
            steeringForce = steeringForce.normalized * Min(steeringForce.magnitude, maximumSteeringForce);
      
        }
        return steeringForce;
    }

    private float calculateBoidDistance(Boid boid)
    {
        double deltaX = this.rb.position.x - boid.rb.position.x;
        double deltaY = this.rb.position.y - boid.rb.position.y;
        float distance = (float) Sqrt(Pow(deltaX, 2) + Pow(deltaY, 2));
        return distance;
    }
}
