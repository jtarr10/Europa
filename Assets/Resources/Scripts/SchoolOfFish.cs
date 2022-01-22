using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchoolOfFish : MonoBehaviour {
    private List<GameObject> boids = new List<GameObject>();
    public GameObject fishPrefab;
    public int numberOfBoids = 20;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < this.numberOfBoids; i++)
            this.boids.Add(Instantiate(fishPrefab, new Vector2(Random.Range(-4f, 4f), Random.Range(-2f, 2f)), Quaternion.identity));
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject fish in this.boids)
        {
            fish.GetComponent<Boid>().Flock(boids);
        }
    }
}
