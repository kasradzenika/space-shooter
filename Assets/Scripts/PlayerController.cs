﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary {

	public float xMin, xMax, zMin, zMax;

}

public class PlayerController : MonoBehaviour {

	[SerializeField] Boundary boundary;
	[SerializeField] float shipSpeed;
    [SerializeField] float tilt;
	//[SerializeField] GameObject boltPrefab;
	[SerializeField] Transform shotSpawn;
	[SerializeField] float fireRate;

	Rigidbody ship;
	Transform shipEnd;
	float moveHorizontal;
	float moveVertical;
	GameObject bolt;
	float nextFireTime = 0.0f;
	AudioSource fireSound;
    
    public int gunLevel = 0;
    public int[] levelUpPoints;
    public GameObject[] bolts;
	
	void Awake ()
    {
		ship = GetComponent<Rigidbody> ();
		fireSound = GetComponent<AudioSource> ();
	}

	void Update ()
    {
		if(Input.GetButtonDown("Fire1") && Time.time > nextFireTime)
        {
			nextFireTime = Time.time + fireRate; 
			bolt = Instantiate (bolts[gunLevel], shotSpawn.position, Quaternion.identity) as GameObject;
			fireSound.Play ();
		}
	}

	void FixedUpdate ()
    {
		// Move ship based on input
		moveHorizontal = Input.GetAxis ("Horizontal") * shipSpeed;
		moveVertical = Input.GetAxis ("Vertical") * shipSpeed;
		ship.velocity = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		// Correct the ship from going out of bounds with clamp
		float newXPosition = Mathf.Clamp (ship.position.x, boundary.xMin, boundary.xMax);
		float newYPosition = Mathf.Clamp(ship.position.z, boundary.zMin, boundary.zMax);
		ship.position = new Vector3 (newXPosition, 0.0f, newYPosition);

		ship.rotation = Quaternion.Euler (0.0f, 0.0f, ship.velocity.x * -tilt);
	}
    
    public void CheckLevelUp(int score)
    {
        if (gunLevel >= levelUpPoints.Length)
            return;

        if(score >= levelUpPoints[gunLevel])
        {
            gunLevel++;
        }
    }
}
