using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class DialogSoundManager : MonoBehaviour {
	AudioSource audio;

	void Start(){
		audio = GetComponent<AudioSource> ();
		PlayDialogue ("hmm");
	}

	public IEnumerator PlayDialogue(string clipName){
		
		Debug.Log ("odtwarzam" + clipName);
		audio.clip = GetAudioClipFromFile (clipName);
		audio.Play ();
		yield return new WaitForSeconds(audio.clip.length);
	}

	public AudioClip GetAudioClipFromFile(string clipName){
		return (AudioClip)Resources.Load ("Sound/" + clipName, typeof(AudioClip));
	}
}
