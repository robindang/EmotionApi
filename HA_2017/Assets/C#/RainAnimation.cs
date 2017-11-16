using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainAnimation : MonoBehaviour {

	private GameObject[] rains;
	private GameObject water;
	private GameObject flame;


	// Use this for initialization
	void Start () {
		Debug.Log ("test");
		rains = GameObject.FindGameObjectsWithTag ("Rain");
		water = GameObject.Find ("Water4Advanced");
		flame = GameObject.Find ("Fire");

		StartCoroutine (Test ());
	}
	
	// Update is called once per frame
	void Update () {
	}

	IEnumerator Test() {
		setFire (true);
		yield return new WaitForSeconds (5);
		setFire (false);

	}
		

	void SetRain(bool visible) {
		foreach (GameObject rain in rains) {
			rain.SetActive (visible);
		}
	}

	enum WaterLevel {High, Medium, Low};  

	void SetWater(WaterLevel waterLevel) {
		switch (waterLevel) {
		case WaterLevel.High:
			this.water.transform.localPosition = new Vector3 (0.9638441f, 7.0f, 82.2421f);
			break;
		case WaterLevel.Medium:
			this.water.transform.localPosition = new Vector3 (0.9638441f, -1.0f, 82.2421f);
			break;
		case WaterLevel.Low:
			this.water.transform.localPosition = new Vector3 (0.9638441f, -5.3f, 82.2421f);
			break;
		}
	}

	void setFire (bool visible)
		{
		this.flame.SetActive (visible);
		}
	
}


