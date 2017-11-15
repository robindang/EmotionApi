using System.IO;
using UnityEngine;
using System;
using System.Collections;

public class EmotionService : MonoBehaviour
{

	private float sampleRateSeconds = 10F;
	private float nextSampleTime = 0.0F;
	private Camera cam;
	private GameObject water;
	private GameObject flame;
	private GameObject fireflies;
	private GameObject[] deadtrees;
	private GameObject[] livetrees;
	private GameObject[] plants;
	private GameObject[] flowers;
	private GameObject[] fire;
	private GameObject[] sparks;
	private GameObject[] rain;

	private AudioSource happyClip;
	private AudioSource sadClip;
	private AudioSource angryClip;
	private AudioSource fearClip;
	private AudioSource surprisedClip;
	private AudioSource neutralClip;

	public enum Emo
	{
		Angry,
		Fear,
		Happy,
		Neutral,
		Sad,
		Surprise
	}

	// Use this for initialization
	void Start ()
	{
//		StartCoroutine(getEmotion()); 	
		cam = GameObject.Find ("CameraSky").GetComponent <Camera> ();
		water = GameObject.Find ("Water4Advanced");
		flame = GameObject.Find ("FlamesEffects");
		fireflies = GameObject.Find ("Particles_Fireflies");

		deadtrees = GameObject.FindGameObjectsWithTag ("DeadTrees");
		livetrees = GameObject.FindGameObjectsWithTag ("LiveTrees");
		flowers = GameObject.FindGameObjectsWithTag ("Flowers");
		plants = GameObject.FindGameObjectsWithTag ("Plants");
		rain = GameObject.FindGameObjectsWithTag ("Rain");
		fire = GameObject.FindGameObjectsWithTag ("Fire");
		happyClip = GameObject.Find ("HappyAudio").GetComponent <AudioSource> ();
		sadClip = GameObject.Find ("SadAudio").GetComponent <AudioSource> ();
		angryClip = GameObject.Find ("AngerAudio").GetComponent <AudioSource> ();
		neutralClip = GameObject.Find ("NeutralAudio").GetComponent <AudioSource> ();
		fearClip = GameObject.Find ("FearAudio").GetComponent <AudioSource> ();
		surprisedClip = GameObject.Find ("SurpriseAudio").GetComponent <AudioSource> ();
//		playMusic (Emo.Sad);
		resetMusic();
		Example();
	}



	// Update is called once per frame
	void Update ()
	{
		Debug.Log ("Calling emotion..");
//		StartCoroutine (getEmotion ());
		getFakeEmotion ();
		Debug.Log ("Getting emotion..");
		//		if (shouldSample()) {
		//			EmotionScores scores = getEmotion();
		//		}
	}

	bool shouldSample ()
	{
		float now = Time.time;
		if (now > nextSampleTime) {
			nextSampleTime = nextSampleTime + sampleRateSeconds;
			return true;
		}
		return false;
	}

	IEnumerator Example()
	{
		playMusic (Emo.Sad);
		yield return new WaitForSeconds(5);
		playMusic (Emo.Angry);
	}

	IEnumerator getEmotion ()
	{
		WWW www = new WWW ("http://localhost:5000/api/emotions");
		yield return www;
		Debug.Log (www.isDone);
		Debug.Log ("Done getting emotion.");
		if (www.isDone) {
			string jsonString = www.text;
			if (!jsonString.StartsWith ("null")) {
				EmotionJson json = JsonUtility.FromJson<EmotionJson> (jsonString);
			}
		}
	}

	void getFakeEmotion()
	{
		EmotionJson json = new EmotionJson ();
		json.angry = 1F;
		json.fear = 0F;
		json.happy = 0F;
		json.neutral = 0F;
		json.sad = 2F;
		json.surprise = 3F;
		alterLandscape (json);
	}

	void playMusic(Emo emotion) {
		resetMusic ();
		switch (emotion) {
		case Emo.Angry:
			angryClip.volume = 1.0F;
			break;
		case Emo.Fear:
			fearClip.volume = 1.0F;
			break;
		case Emo.Happy:
			happyClip.volume = 1.0F;
			break;
		case Emo.Neutral:
			neutralClip.volume = 1.0F;
			break;
		case Emo.Sad:
			sadClip.volume = 1.0F;
			break;
		case Emo.Surprise:
			surprisedClip.volume = 1.0F;
			break;
		default:
			break;
		}
	}

	void resetMusic() {
		surprisedClip.volume = 0.0F;
		sadClip.volume = 0.0F;
		happyClip.volume = 0.0F;
		neutralClip.volume = 0.0F;
		fearClip.volume = 0.0F;
		angryClip.volume = 0.0F;
	}

	void alterLandscape (EmotionJson emotionJson)
	{
		Emo emotion = emotionJson.getMax ();
		switch (emotion) {
		case Emo.Angry:
			setAngryLandscape ();
			break;
		case Emo.Fear:
			setFearLandscape ();
			break;
		case Emo.Happy:
			setHappyLandscape ();
			break;
		case Emo.Neutral:
			setNeutralLandscape ();
			break;
		case Emo.Sad:
			setSadLandscape ();
			break;
		case Emo.Surprise:
			setSurpriseLandscape ();
			break;
		default:
			break;
		}
			
	}

	void setAngryLandscape() {
		setBackgroundColor (new Color (173F/256, 29F/256, 16F/256));
		setFire (true);
		setFireflies (false);
		setRain (false);
		setFog (false);
		setSparks (true);
		setDeadTrees (true);
	}

	void setFearLandscape() {
		setBackgroundColor (new Color (99F/256, 40F/256, 158F/256));
		setFire (true);
		setFireflies (true);
		setRain (false);
		setFog (true);
		setSparks (true);
		setDeadTrees (true);
	}

	void setHappyLandscape() {
		setBackgroundColor (new Color (255F/256, 227F/256, 54F/256));
		setFire (false);
		setFireflies (false);
		setRain (false);
		setFog (false);
		setSparks (true);
		setDeadTrees (false);
	}

	void setNeutralLandscape() {
		setBackgroundColor (new Color (37F/256, 255F/256, 165F/256));
		setFire (false);
		setFireflies (true);
		setRain (false);
		setFog (true);
		setSparks (false);
		setDeadTrees (false);
	}

	void setSadLandscape() {
		setBackgroundColor (new Color (20F/256, 57F/256, 136F/256));
		setFire (false);
		setFireflies (true);
		setRain (true);
		setFog (true);
		setSparks (true);
		setDeadTrees (true);
	}

	void setSurpriseLandscape() {
		setBackgroundColor (new Color (241F/256, 146F/256, 0F));
		setFire (false);
		setFireflies (false);
		setRain (false);
		setFog (false);
		setDeadTrees (false);
	}

	//angry: no water, fire, red background, burnt trees
	//fear: dark grey/purple background, fog, lightning, burnt trees, low water
	//happy: yellow background, sun, water, sparkly things, trees with leaves
	//neutral: green background, water, fog
	//sadness: blue background, fog, rain, raindrops
	//surprise: orange background, sparks, no water

	void setBackgroundColor (Color color)
	{
		Debug.Log ("background is becoming red");
		cam.backgroundColor = color;
	}

	void setWaterLevel (Vector3 vector)
	{
		this.water.transform.TransformVector (vector);
	}

	void setDeadTrees(bool visible) {
//	foreach (GameObject deadtree in this.deadtrees) {
//			// set x value very large to move fire of the screen when not visible
//			var x = deadtree.transform.position.x;
//			if (visible && x < 500F || !visible && x > 500F) {
//				continue;
//			}
//			x = deadtree.transform.position.x + (visible ? 0 : 10000F);
//			var vector = new Vector3 (x, deadtree.transform.position.y, deadtree.transform.position.z);
//			this.flame.transform.TransformVector (vector);
//	}
	}

//	IEnumerator transformZ(GameObject obj, float diff) {
//		yield return new WaitForSeconds(1);
//		var vector = new Vector3 (obj.transform.position.x, obj.transform.position.y, obj.transform.position.z - diff);
//		this.flame.transform.TransformVector (vector);
//	}

	void setFire (bool visible)
	{
		// set x value very large to move fire of the screen when not visible
		var vector = new Vector3 (visible ? 49.9F : 10000F, 3.9F, 21.1F);
		this.flame.transform.TransformVector (vector);
	}

	void setFireflies (bool visible)
	{
		// set x value very large to move fire of the screen when not visible	
		var vector = new Vector3 (visible ? 25.5F : 10000F, 6.74F, -6.8F);
		this.fireflies.transform.TransformVector (vector);
	}

	void setRain(bool visible) {

		// set x value very large to move fire of the screen when not visible
		var vector = new Vector3 (visible ? -7.8F : 10000F, -0.6F, -25.3F);
		foreach (GameObject r in this.rain) {
			r.transform.TransformVector (vector);
		}
	}

	void setFog(bool visible) {
	}

	void setSparks(bool visible) {
	}

	void setLightning(bool visible) {
	}

	public class EmotionScores
	{
		public double Anger { get; set; }

		public double Fear { get; set; }

		public double Happiness { get; set; }

		public double Neutral { get; set; }

		public double Sadness { get; set; }

		public double Surprise { get; set; }

		public void set (string name, double value)
		{
			switch (name.ToLower ()) {
			case "anger":
				Anger = value;
				break;
			case "fear":
				Fear = value;
				break;
			case "happiness":
				Happiness = value;
				break;
			case "neutral":
				Neutral = value;
				break;
			case "sadness":
				Sadness = value;
				break;
			case "surprise":
				Surprise = value;
				break;
			}
		}
	}

	[Serializable]
	public class EmotionJson
	{
		public float angry;
		public float fear;
		public float happy;
		public float neutral;
		public float sad;
		public float surprise;

		public float getEmotion (Emo emo)
		{
			switch (emo) {
			case Emo.Angry:
				return angry;
			case Emo.Fear:
				return fear;
			case Emo.Happy:
				return happy;
			case Emo.Neutral:
				return neutral;
			case Emo.Sad:
				return sad;
			case Emo.Surprise:
				return surprise;
			default:
				return neutral;
			}
		}

		public Emo getMax ()
		{
			Emo max = Emo.Angry;
			if (getEmotion (Emo.Fear) > getEmotion (max)) {
				max = Emo.Fear;
			}
			if (getEmotion (Emo.Happy) > getEmotion (max)) {
				max = Emo.Happy;
			}
			if (getEmotion (Emo.Neutral) > getEmotion (max)) {
				max = Emo.Neutral;
			}

			if (getEmotion (Emo.Sad) > getEmotion (max)) {
				max = Emo.Sad;
			}

			if (getEmotion (Emo.Surprise) > getEmotion (max)) {
				max = Emo.Surprise;
			}
			return max;
		}

	}
}

