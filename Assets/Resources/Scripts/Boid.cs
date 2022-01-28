using System.Collections;
using System.Collections.Generic;
using static System.Math;
using UnityEngine;

public class Boid : MonoBehaviour {

    private Vector2 acceleration = new Vector2(0f, 0f);
    public Rigidbody2D rb;
    private SpriteRenderer sprite;
    public float perceptionDistance = 0.5f;
    public float maximumSteeringForce = 0.04f;
    public float speedLimit = 2f;

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
        this.rb.velocity = new Vector2(Min(this.rb.velocity.x + this.acceleration.x, speedLimit), Min(this.rb.velocity.y + this.acceleration.y, speedLimit));

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
    public void Flock(List<GameObject> boids, float alignmentModifier, float cohesionModifier, float seperationModifier)
    {
        Vector2 alignmentSteering = Align(boids) * alignmentModifier;
        Vector2 cohesionSteering = Cohesion(boids) * cohesionModifier;
        Vector2 seperationSteering = Seperation(boids) * seperationModifier;
        Edge();
        this.acceleration = cohesionSteering + alignmentSteering + seperationSteering;
    }

    private Vector2 Align(List<GameObject> localFish)
    {   
        Vector2 steeringForce = new Vector2(0f, 0f);
        int totalInRange = 0;

        foreach(GameObject other in localFish)
        {
            Boid boid = other.GetComponent<Boid>();
            float dist = calculateBoidDistance(boid);
            if (other.GetComponent<Boid>() != this && dist < perceptionDistance)
            {
                steeringForce += boid.rb.velocity;
                totalInRange++;
            }
        }
        if (totalInRange > 0)
        {
            steeringForce /= totalInRange;
            steeringForce = steeringForce.normalized * speedLimit;
            steeringForce -= this.rb.velocity;
            steeringForce = steeringForce.normalized * Min(steeringForce.magnitude, maximumSteeringForce);
      
        }
        return steeringForce;
    }

    private Vector2 Cohesion(List<GameObject> localFish)
    {
        Vector2 steeringForce = new Vector2(0f, 0f);
        int totalInRange = 0;

        foreach(GameObject other in localFish)
        {
            Boid boid = other.GetComponent<Boid>();
            float dist = calculateBoidDistance(boid);
            if(other.GetComponent<Boid>() != this && dist < perceptionDistance)
            {
                steeringForce += boid.rb.position;
                totalInRange++;
            }
        }
        if (totalInRange > 0)
        {
            steeringForce /= totalInRange;
            steeringForce -= this.rb.position;
            steeringForce = steeringForce.normalized * speedLimit;
            steeringForce -= this.rb.velocity;
            steeringForce = steeringForce.normalized * Min(steeringForce.magnitude, maximumSteeringForce);
        }

        return steeringForce;
    }

    private Vector2 Seperation(List<GameObject> localFish)
    {
        Vector2 steeringForce = new Vector2(0f, 0f);
        int totalInRange = 0;

        foreach (GameObject other in localFish)
        {
            Boid boid = other.GetComponent<Boid>();
            float dist = calculateBoidDistance(boid);
            if (other.GetComponent<Boid>() != this && dist < perceptionDistance / 2)
            {
                Vector2 difference = this.rb.position - boid.rb.position;
                difference /= dist;
                steeringForce += difference;
                totalInRange++;
            }
        }
        if (totalInRange > 0)
        {
            steeringForce /= totalInRange;
            steeringForce = steeringForce.normalized * speedLimit;
            steeringForce -= this.rb.velocity;
            steeringForce = steeringForce.normalized * Min(steeringForce.magnitude, maximumSteeringForce);
        }

        return steeringForce;
    }

    private void Edge()
    {
        if (this.rb.position.x > 8.38)
        {
            this.rb.position = new Vector2(-8.38f, this.rb.position.y);
        } else if (this.rb.position.x < -8.38)
        {
            this.rb.position = new Vector2(8.38f, this.rb.position.y);
        }

        if(this.rb.position.y > 5)
        {
            this.rb.position = new Vector2(this.rb.position.x, -5f);
        } else if (this.rb.position.y < -5)
        {
            this.rb.position = new Vector2(this.rb.position.x, 5f);
        }
    }

    private float calculateBoidDistance(Boid boid)
    {
        double deltaX = this.rb.position.x - boid.rb.position.x;
        double deltaY = this.rb.position.y - boid.rb.position.y;
        float distance = (float) Sqrt(Pow(deltaX, 2) + Pow(deltaY, 2));
        return distance;
    }
}
