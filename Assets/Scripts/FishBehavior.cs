using System.Collections;
using System.Collections.Generic;
using static System.Math;
using UnityEngine;

public class FishBehavior : MonoBehaviour {

    private float speedModifier = 0.01f;
    private float speedLimit = 2f;
    private float xVelMod = 0;
    private float yVelMod = 0;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;

	// Use this for initialization
	void Start () {
        this.rb = gameObject.GetComponent<Rigidbody2D>();
        this.sprite = gameObject.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.DownArrow))
            yVelMod = this.speedModifier * -1;
        if (Input.GetKeyDown(KeyCode.UpArrow))
            yVelMod = this.speedModifier;
        if (Input.GetKeyDown(KeyCode.RightArrow))
            xVelMod = this.speedModifier;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            xVelMod = this.speedModifier * -1;
        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow))
            yVelMod = 0;
        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
            xVelMod = 0;
    }

    private void FixedUpdate()
    {
        // add the velocity modifiers to the current velocity
        this.rb.velocity = new Vector2((float)Min(this.rb.velocity.x + xVelMod, this.speedLimit), (float)Min(this.rb.velocity.y + yVelMod, this.speedLimit));

        // Rotate the sprite to point in the direction of travel
        double rotationAngle = (180 / PI) * Atan2(this.rb.velocity.normalized.y, this.rb.velocity.normalized.x);
        this.rb.rotation = (float) rotationAngle;

        Debug.Log("Rotation Angle: " + this.rb.rotation);

        // Flip the sprite if it is moving backwards
        if (Abs(this.rb.rotation) > 90 && !this.sprite.flipY)
            this.sprite.flipY = true;
        else if (Abs(this.rb.rotation) < 90 && this.sprite.flipY)
            this.sprite.flipY = false;
    }
}
