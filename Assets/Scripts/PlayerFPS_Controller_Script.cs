using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerFPS_Controller_Script : MonoBehaviour {

	#region Variables jugador
    private float _speed;
    private float _initialSpeed;
    private float _runningSpeed;
    [SerializeField]
    private int _puntaje = 0;
    private int _maxPuntaje;
    [SerializeField]
    private int _maxjumps = 2;
    [SerializeField]
    private float _jumpForce = 1f;
    [SerializeField]
    private int _currentJumps;
    [SerializeField]
    private Vector3 _StartPosition;
	private CharacterController _playerController;
	[SerializeField]
	private float _rotationSpeed = 4f;
	private float _jumping;
	private float _lastJump = 0;

    #endregion

	#region Variables Modelo
	private Animator _modelAnim;
	#endregion

	#region Variables Camara
	//Focus Point always looks at the camera
	[SerializeField]
	private Transform _playerCamera, _focusPoint;
	private float _camRotX, _camRotY;
	private float _cameraSensitivity = 10f; //Rotation speed
	private float _CameraHeight = 0.7f;
	[SerializeField]
	private float _zoom;
	[SerializeField]
	private float _zoomSpeed = 5f;
	[SerializeField]
	private float _zoomMin = -10f;
	[SerializeField]
	private float _zoomMax = 0;
	#endregion

	#region User Interface
    [SerializeField]
    private Text _FoodText, _MoneyText, _LifeText, _ClientsText;
    [SerializeField]
    private Text _WinLoseText;
    [SerializeField]
	private Button _ResetButton;
	private Reset_Scene_Script _ResetButtonScript;
	[SerializeField]
	private Image _BackgroundImage;
    #endregion
    
    #region Boundaries
	private float _YlowBound = -5f;
	#endregion
	
	#region Destroy spawn objects
	private GameObject[] enemies;
	private GameObject[] clients;
	private GameObject[] collectables;
	#endregion
	
	#region Audio
	AudioAdmin _audio;
	#endregion
	
	#region SpawnObjects
	SpawnObjects enemiesSpawnObj;
	ClientSpawner clientsSpawnObj;
	InfiniteSpawner[] collectablesSpawnerObj;
	#endregion
	
	#region PlayerLife
	private float health;
	private float maxHealth;
	[SerializeField]
	private Image healthBar;
	#endregion
	
	#region GameStatus
	private int foodNumber;
	[SerializeField]
	private GameObject foodPlatePrefab;
	private int moneyEarned;
	private int lifeRemaining;
	private int attendedClients;
	private int totalClientsToAttend;
	private bool _isTriggeredHelper;
	#endregion
	

	// Use this for initialization
	void Start () {
		
		#region initialize Player
		_initialSpeed = 5f;
		_speed = _initialSpeed;
		_runningSpeed = 10f;
		_playerController = GetComponent<CharacterController>();//Obtener el Controlador
		_currentJumps = 0;
		_StartPosition = transform.position;
		totalClientsToAttend = 10;
		_isTriggeredHelper = false;
		#endregion

		_modelAnim = GetComponentInChildren<Animator>();

		#region initialize Camera
		_zoom = -5f;
		_playerCamera.transform.localPosition = new Vector3(0,0,_zoom);
		_playerCamera.LookAt(_focusPoint);
		#endregion
	
		
		#region initialize UI
		
		foodNumber = 0;
		moneyEarned = 0;
		lifeRemaining = 3;
		attendedClients = 0;
		
		ShowScore();
		_WinLoseText.gameObject.SetActive(false);
		_BackgroundImage.enabled = false;
		#endregion
		
		#region Get Reset Scene Script
		_ResetButtonScript = _ResetButton.GetComponent<Reset_Scene_Script>();
		if(_ResetButtonScript == null){
			Debug.LogError("Button Script Missing");
		}
		#endregion
		
		#region Puntaje
		_maxPuntaje = GameObject.FindGameObjectsWithTag("Food").Length;
		#endregion
		
		#region Audio
		_audio = FindObjectOfType<AudioAdmin>();
		if( _audio == null){
			Debug.LogError("Audio script could not be found.");
		}
		#endregion
		
		#region SpawnObjects
		enemiesSpawnObj = FindObjectOfType<SpawnObjects>();
		if( enemiesSpawnObj == null){
			Debug.LogError("'Enemies SpawnObjects' could not be found.");
		}
		clientsSpawnObj = FindObjectOfType<ClientSpawner>();
		if(clientsSpawnObj == null){
			Debug.LogError("'Clients SpawnObjects' could not be found");
		}
		collectablesSpawnerObj = FindObjectsOfType<InfiniteSpawner>();
		if(collectablesSpawnerObj == null){
			Debug.LogError("'Infinite Spawners' could not be found");
		}
		#endregion
		
		#region PlayerLife
		maxHealth = 100;
		health = maxHealth;
		healthBar.fillAmount = health/maxHealth;
		InvokeRepeating("EnergyLoss",1,1);
		#endregion
		
	}
	
	// Update is called once per frame
	void Update () {
		#region Zoom
		//Si rueda del mouse se va para adelante o se gira
		if(Input.GetAxis("Mouse ScrollWheel") > 0 | Input.GetAxis("Mouse ScrollWheel") < 0 );
		{
			_zoom += Input.GetAxis("Mouse ScrollWheel") * _zoomSpeed; //Cuánto se debe mover el zoom
			_zoom = Mathf.Clamp(_zoom, _zoomMin, _zoomMax );
			_playerCamera.transform.localPosition = new Vector3(0,0,_zoom);
		}
		#endregion

		#region Camera rotation
		if(Input.GetMouseButton(1)){
			_camRotX += Input.GetAxis("Mouse X")* _cameraSensitivity; 
			_camRotY += Input.GetAxis("Mouse Y")* _cameraSensitivity; 
			_camRotY = Mathf.Clamp(_camRotY, -10, 10); //Stop Movement in Y
			_focusPoint.localRotation = Quaternion.Euler(_camRotY,_camRotX,0);
		}
		#endregion
		
		#region Health Bar rotation
		
		#endregion
		
		#region Focus and move towards
		_focusPoint.position = new Vector3(transform.position.x, transform.position.y + _CameraHeight, transform.position.z );
		//Rotate camera with the player and the speed
		if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0){
			Quaternion turnAngle = Quaternion.Euler(0, _focusPoint.eulerAngles.y,0);
			transform.rotation = Quaternion.Slerp(transform.rotation,
			turnAngle, Time.deltaTime * _rotationSpeed);
		}
	
		#endregion

		#region Player Movement
		float MovHorizontal = Input.GetAxis("Horizontal")* _speed;
		float MovVertical =Input.GetAxis("Vertical") * _speed;
		float runSpeed = Mathf.Max(Mathf.Abs(MovHorizontal), Mathf.Abs(MovVertical));
		_modelAnim.SetFloat("Speed", runSpeed/10);
		 

		Vector3 movement = new Vector3(MovHorizontal, _jumping, MovVertical);
		movement = transform.rotation * movement;
		_playerController.Move(movement * Time.deltaTime);
		
		#endregion
		
		#region Boundaries
		CheckBounds();
		#endregion

		

		#region Player Jump
		if(_playerController.isGrounded == true){
			_currentJumps = 0;
		}
		if(_currentJumps < _maxjumps){
			if(Input.GetKeyDown(KeyCode.Space)){
				_jumping += _jumpForce;
				_currentJumps++;
				_modelAnim.SetTrigger("isJumping");
				_lastJump = Time.time;
			}
		}
		#endregion
	}

	private void FixedUpdate(){
		//Set weight to player so that it goes back to the ground while jump
		if(Time.time - _lastJump > 0.05){
			if(_playerController.isGrounded == false){
				_jumping += Physics.gravity.y * Time.deltaTime;
			}
			else{
				_jumping = 0.0f;
			}
		}
	}

	//Colisionable - If ball touches the cube, it disappears
    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Food")){
			_audio.PlayAudio("NewPoint");
            foodNumber++;
            ShowScore();
            other.gameObject.SetActive(false);
        }
		else if(other.CompareTag("PowerUp")){
			_audio.PlayAudio("PowerUp");
            //_speed *= 1.1f;
            //_speed *= 1.15f;
            other.gameObject.SetActive(false);
            EnergyGain();
        }
		else if(other.CompareTag("SpeedUp")){
			_audio.PlayAudio("SpeedUp");
			other.gameObject.SetActive(false);
			FullEnergy();
			StartCoroutine(Run());
		}
		else if(other.CompareTag("Enemy")){
			if(_isTriggeredHelper){ //Prevents triggering twice
				_isTriggeredHelper = false;
				lifeRemaining = lifeRemaining -1;
				ShowScore();
				_audio.PlayAudio("Ouch");
				if(lifeRemaining == 0){ LoseGame("The rats ate your food");	}
			}
			else{_isTriggeredHelper = true;}
			
		}
		else if(other.CompareTag("Client")){
			ClientController client = other.GetComponent<ClientController>();
			if(!client.HasFood && foodNumber > 0){ //Give food to client
				foodNumber = foodNumber -1; 
				client.HasFood = true;
				attendedClients++;
				//Get money
				_audio.PlayAudio("Cash");
				moneyEarned = moneyEarned + other.GetComponent<ClientController>().payFood();
				//Instantiate food plate in front
	    		if(foodPlatePrefab == null){
	    			Debug.LogWarning("FoodPlatePrefab is null in Player Controller");
	    		}
				Vector3 platePosition = new Vector3(other.transform.position.x,2.2f,other.transform.position.z); //Set over table
	    		GameObject plateInstance = Instantiate(foodPlatePrefab, platePosition+other.transform.forward, other.transform.rotation); //Set in front of client
	    		plateInstance.transform.SetParent(other.transform);
				
				ShowScore();
				if(attendedClients == totalClientsToAttend){ WinGame("You won!"); }
			}
		}
		
        
    }
	
	private void LoseGame(string reason){
		ShowWinLose(reason);
		_BackgroundImage.enabled = true;
		_audio.StopAudio("Theme");
		_audio.PlayAudio("GameOver");
		DestroyEverything();
		
	}
	
	private void WinGame(string reason){
		ShowWinLose("You won!!!");
		_BackgroundImage.enabled = true;
		_audio.PlayAudio("WinGame");
		_audio.StopAudio("Theme");
        DestroyEverything();
	}
	
	private void DestroyEverything(){
		DestroyEnemies();
		DestroyClients();
		DestroyCollectables();
		Destroy(gameObject); //Player
	}
	
	private void DestroyEnemies(){
		enemiesSpawnObj.StopSpawner();
		enemies = GameObject.FindGameObjectsWithTag("Enemy");
			foreach(GameObject enemy in enemies){
				Destroy(enemy);
			}
	}
	
	
	private void DestroyClients(){
		clientsSpawnObj.StopSpawner();
		clients = GameObject.FindGameObjectsWithTag("Client");
			foreach(GameObject client in clients){
				Destroy(client);
			}
	}
	
	private void DestroyCollectables(){
		foreach(InfiniteSpawner collectableSpawner in collectablesSpawnerObj){
			collectableSpawner.StopSpawner();
			collectableSpawner.DestroyObjects();
		}
		
	}

	private void ShowScore(){
       _FoodText.text = foodNumber.ToString();
       _MoneyText.text = "$"+ moneyEarned.ToString();
       _LifeText.text = lifeRemaining.ToString();
       _ClientsText.text = attendedClients.ToString()+"/"+totalClientsToAttend;
    }
	
	private void ShowWinLose(string text){
		_WinLoseText.gameObject.SetActive(true);
       	_WinLoseText.text = text;
       	_ResetButtonScript.ShowButton();
    }
	
	private void CheckBounds(){
	 if(transform.position.y <= _YlowBound){
	 	transform.position = _StartPosition;
	 }
	}
	
	private void EnergyLoss(){
		health -= 2;
		healthBar.fillAmount = health / maxHealth;
		if(health <= 0){
			LoseGame("You ran out of energy!");
		}
	}
	private void EnergyGain(){
		health += 10;
		if(health > maxHealth){
			health = maxHealth;
		}
		else{
			healthBar.fillAmount = health / maxHealth;
		}
	}
	
	private void FullEnergy(){
		health += (maxHealth / 2);
		if(health > maxHealth){
			health = maxHealth;
		}
		healthBar.fillAmount = health / maxHealth;
	}
	
	public int getFoodNumber(){
		return foodNumber;
	}
	
	IEnumerator Run(){
		_speed = _runningSpeed;
		yield return new WaitForSeconds(10); //Wait and return saving state
		_speed = _initialSpeed;
	}
	
	

}
