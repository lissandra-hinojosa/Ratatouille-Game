using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientController : MonoBehaviour
{
	
	#region ClientHappinessCanvas
	private bool _hasFood;
	private bool isEating;
	private float happy;
	private float maxHappy;
	[SerializeField]
	private Image happinessBar;
	[SerializeField]
	private Sprite sadImage;
	
	[SerializeField]
	private Image statusFace;
	#endregion
	
	#region Table objects
	private GameObject chair;
	private GameObject foodPlatePrefab; //Get prefab to instantiate
	#endregion
	
    // Start is called before the first frame update
    void Start()
    {
    	
    	#region ClientHappinessCanvas
    	_hasFood = false;
    	isEating = false;
		maxHappy = 100;
		happy = maxHappy;
		happinessBar.fillAmount = happy/maxHappy;
		InvokeRepeating("LessHappy",1,1);
		#endregion
		
		
		#region Table objects
		chair = null;
		foodPlatePrefab = null;
		#endregion
		
		/*
		#region Materials
		for(int i=0; i < transform.childCount; i++){
			//if(transform.GetChild(i).name == "client"){
			//}
			//Debug.Log(transform.GetChild(i).name);
		}
		#endregion
		*/
        
    }

    // Update is called once per frame
    void Update()
    {
    	if(_hasFood){
    		StartCoroutine(EatAndLeave());	
    	}	
    }
    
       
    
    private void LessHappy(){
    	if(!_hasFood){
    		happy -= 5;
			happinessBar.fillAmount = happy / maxHappy;
			if(happy < (maxHappy / 2)){
				statusFace.sprite = sadImage;
			}
			if(happy <= 0){
				destroyClient();
				//TODO: Destroy client and quit points to 
			}
    	}
		
	}
    
    IEnumerator EatAndLeave(){
		yield return new WaitForSeconds(5f);
		destroyClient();
	}
    
    private void destroyClient(){ 
    	if(chair == null){ //TODO: It could be standing
    		Destroy(this.gameObject);
    	}else{    	
    		chair.GetComponent<ChairScript>().IsOccupied = false; //Indicate that chair is free before destroying client
    		Destroy(this.gameObject);
    	}
    	
    }
    
    public void setChair(ref GameObject chairObj){
    	chair = chairObj;
    }
    public void setFoodPlate(GameObject foodPlate){
    	foodPlatePrefab = foodPlate;
    }
    
    public int payFood(){
    	return (int)happy;
    }
    
    public bool HasFood{
    get { return _hasFood; }
    set { _hasFood = value; }
	}
    
    
    /*
	private Renderer _MatD;
	private Renderer _Mat;
	private Color _color;
	void Start(){
		_Mat = transform.parent.GetComponent<Renderer>();
		_MatD = destino.GetComponent<Renderer>();
		_color = new Color(
		Random.Range(0f, 1f),
		Random.Range(0f, 1f),
		Random.Range(0f, 1f)
		);
		_Mat.material.color = _color;
		_MatD.material.color = _color;
	}*/
	
    
}
