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
		cam = GetComponent<Camera> ();
		water = GameObject.Find ("Water4Advanced");
		flame = GameObject.Find ("FlamesEffects");
		fireflies = GameObject.Find ("Particles_Fireflies");
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
		StartCoroutine (getEmotion ());
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
//				alterLandscape (json);
			}
		}
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

//	void alterLandscape (EmotionJson emotionJson)
//	{
//		Emo emotion = emotionJson.getMax ();
//		switch (emotion) {
//		case Emo.Angry:
//			AudioSource.PlayClipAtPoint (angryClip, new Vector3 (0, 0, 0));
////			setWaterLevel (new Vector3 (0, 0, 0));
////			setFire (true);
////			setBackgroundColor (Color.red);
//			break;
//		case Emo.Fear:
//			AudioSource.PlayClipAtPoint (fearClip, new Vector3 (0, 0, 0));
//			break;
//		case Emo.Happy:
//			AudioSource.PlayClipAtPoint (happyClip, new Vector3 (0, 0, 0));
////			setBackgroundColor (Color.yellow);
////			setWaterLevel (new Vector3 (1, 1, 1));
//			break;
//		case Emo.Neutral:
//			setBackgroundColor (Color.green);
//			setWaterLevel (new Vector3 (1, 1, 1));
//		case Emo.Sad:
//			setBackgroundColor(Color.blue);
//			set
//		case Emo.Surprise:
//			return surprise;
//		default:
//			return neutral;
//		}
//			
//	}

	//angry: no water, fire, red background, burnt trees
	//fear: dark grey/purple background, fog, lightning, burnt trees, low water
	//happy: yellow background, sun, water, sparkly things, trees with leaves
	//neutral: green background, water, fog
	//sadness: blue background, fog, rain, raindrops
	//surprise: orange background, sparks, no water

	void setBackgroundColor (Color color)
	{
		cam.backgroundColor = color;
	}

	void setWaterLevel (Vector3 vector)
	{
		this.water.transform.TransformVector (vector);
	}

	void setFire (bool visible)
	{
		// set x value very large to move fire of the screen when not visible
//		var vector = new Vector3 (visible ? 49.9 : 10000, 3.9, 21.1);
//		this.flame.transform.TransformVector (vector);
	}

	void setFireflies (bool visible)
	{
		// set x value very large to move fire of the screen when not visible	
//		var vector = new Vector3 (visible ? 25.5 : 10000, 6.74, -6.8);
//		this.fireflies.transform.TransformVector (vector);
	}

	static void setMusic ()
	{
		
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

