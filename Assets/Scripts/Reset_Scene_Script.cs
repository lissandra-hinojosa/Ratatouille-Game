using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Reset_Scene_Script : MonoBehaviour
{
	private Button _resetButton;
	private AudioAdmin _audio;
	private SceneLoader _sceneAdmin;
    // Start is called before the first frame update
    void Start()
    {
    	_resetButton = GetComponent<Button>();
    	_resetButton.image.gameObject.SetActive(false);
    	_resetButton.onClick.AddListener(TaskOnClick);
    	
    	_audio = FindObjectOfType<AudioAdmin>();
    	if( _audio == null){
			Debug.LogError("Audio script could not be found.");
		}
    	_sceneAdmin = FindObjectOfType<SceneLoader>();
    	if(_sceneAdmin == null){
    		Debug.LogError("Could not find Scene Loader");
    	}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void TaskOnClick(){
    	_sceneAdmin.loadGame();
    	//SceneManager.LoadScene("Game");
    	_audio.PlayAudio("Theme");
    }
    
    public void ShowButton(){
    	_resetButton.image.gameObject.SetActive(true);
    }
}
