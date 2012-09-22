using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
	public float speed = 4;
	public float bounds = 6;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float xTranslation = 0;
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject player in players) {
			if (player.transform.position.x - transform.position.x > bounds) {
				xTranslation = speed * Time.deltaTime;
			}
		}
		foreach (GameObject player in players) {
			if (player.transform.position.x - transform.position.x <= -bounds) {
				return;
			}
		}
		transform.position += new Vector3(xTranslation, 0, 0);
	}
	
}
