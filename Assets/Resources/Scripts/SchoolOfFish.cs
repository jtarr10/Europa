using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchoolOfFish : MonoBehaviour {
    private List<GameObject> boids = new List<GameObject>();
    public GameObject fishPrefab;
    public int numberOfBoids = 20;
    public float alignment = 0.8f;
    public float cohesion = 0.75f;
    public float seperation = 2f;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < this.numberOfBoids; i++)
            this.boids.Add(Instantiate(fishPrefab, new Vector2(Random.Range(-7f, 7f), Random.Range(-4f, 4f)), Quaternion.identity));
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject fish in this.boids)
        {
            fish.GetComponent<Boid>().Flock(boids, alignment, cohesion, seperation);
        }
    }
}
