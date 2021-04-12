using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kangroo : MonoBehaviour {

	// Use this for initialization

	Animator anim;
	public float speed;
	void Start () {

		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKey(KeyCode.W)) {


			anim.SetBool ("walk", true);
			anim.SetBool ("idle", false);
			transform.Translate (Vector3.forward * speed * Time.deltaTime);
		
		}

	 if (Input.GetKey (KeyCode.LeftShift)) {

		
		
			anim.SetBool ("jump", true);
			anim.SetBool ("walk", false);
			anim.SetBool ("idle", false);
			transform.Translate (Vector3.forward * speed * Time.deltaTime);
		} 

		 if (Input.GetKey(KeyCode.A)) {

		
			transform.Rotate (Vector3.down * 80f * Time.deltaTime);

		} 
		if (Input.GetKey(KeyCode.D)) {


			transform.Rotate (Vector3.up * 80f * Time.deltaTime);

		} 
		if (Input.GetKeyUp (KeyCode.W)) {
		
			anim.SetBool ("walk", false);
			anim.SetBool ("idle", true);
		}

		if (Input.GetKeyUp (KeyCode.LeftShift)) {

			anim.SetBool ("walk", false);
			anim.SetBool ("idle", true);
			anim.SetBool ("jump", false);
		}

	



	

	}
}
