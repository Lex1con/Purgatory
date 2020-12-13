using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player;

	private Vector3 offset;


	// Use this for initialization
	void Start () {
		offset = new Vector3 (transform.position.x - player.transform.position.x, 0, transform.position.z);
	}
	
	// Update is called once per frame
	void LateUpdate () {
		transform.position = new Vector3 (player.transform.position.x + offset.x, transform.position.y + offset.y, transform.position.z);
	}
}
