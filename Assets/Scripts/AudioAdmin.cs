using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioAdmin : MonoBehaviour
{
	public Sound[] sounds;
	public static AudioAdmin instance;
	
    // Start is called before the first frame update
    void Start()
    {
    	PlayAudio("Theme");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void Awake(){
    	if(instance == null){
    		instance = this;
    	}
    	else{
    		Destroy(gameObject);
    		return;
    	}
    	DontDestroyOnLoad(gameObject);
    	foreach(Sound s in sounds){
    		s.source = gameObject.AddComponent<AudioSource>();
    		s.source.clip = s.clip;
    		s.source.volume = s.volume;
    		s.source.loop = s.loop;
    	}
    }
    
    public void PlayAudio(string name){
    	Sound s = Array.Find(sounds, sound => sound.name == name);
    	if(s == null){
    		Debug.LogWarning("Ton: '" + name + "' nicht gefunden.");
    	}
    	s.source.Play();
    }
    
    public void StopAudio(string name){
    	Sound s = Array.Find(sounds, sound => sound.name == name);
    	if(s == null){
    		Debug.LogWarning("Ton: '" + name + "' nicht gefunden.");
    	}
    	s.source.Stop();
    }
}
