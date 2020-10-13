using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
	private Quaternion _initialRotation;
	private GameObject _objective;
	[SerializeField]
	private float _speed = 5f;
	private Transform _EnemyCollisionDetector;
	[SerializeField]
	//private LayerMask _wallMask; //Use 1 << 9 instead
	private float _colliderRadius = 0.5f;
	
	#region Angry Status
	[SerializeField]
	private float _distToObjective;
	private float _angleToObjective;
	#endregion
	
    // Start is called before the first frame update
    void Start()
    {
    	_objective = GameObject.FindGameObjectWithTag("Player");
    	_EnemyCollisionDetector = transform.GetChild(0);
    	
    	transform.Rotate(0,Random.value * 360,0);
    	_initialRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
    	
    	_distToObjective = Vector3.Distance(_objective.transform.position, transform.position);
    	_angleToObjective = (Vector3.Angle(_objective.transform.position - transform.position, transform.forward));
    	if(_angleToObjective >= -45 && _angleToObjective <= 45 && _distToObjective <= 10 ){
    		FollowPlayer();
    	}
    	else{
    		BlindSearch();
    	}
    	
    }
    
    private void FollowPlayer(){
    	Vector3 targetPostition = new Vector3( _objective.transform.position.x, 
                                        this.transform.position.y, 
                                        _objective.transform.position.z ) ;
    	transform.LookAt(targetPostition); //Rotate towards objective. Dp not consider y axis
    	transform.position += transform.forward * _speed * Time.deltaTime; //Move towards objective 
    }
    
    private void BlindSearch(){
    	if(Physics.CheckSphere(_EnemyCollisionDetector.position, _colliderRadius, 1 << 9)){
    		if(Random.value > 0.5f){
    			transform.Rotate(0,90,0);
    		}
    		else{
    			transform.Rotate(0,-90,0);
    		}
    		
    	}
    	else{
    		transform.position += transform.forward * _speed * Time.deltaTime;
    	}
    }
}
