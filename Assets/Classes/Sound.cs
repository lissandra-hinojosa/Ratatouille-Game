using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
	public string name;
	public AudioClip clip;
	[Range(0f,1f)] //Movable bar
	public float volume;
	public AudioSource source;
	public bool loop;
}
