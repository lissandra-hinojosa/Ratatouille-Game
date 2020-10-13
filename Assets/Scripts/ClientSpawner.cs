using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ClientSpawner : MonoBehaviour
{
	
    [SerializeField]
	private GameObject client; //Get Prefab to instantiate
	private Vector3 spawnValues;
	private float spawnWait;
	private float spawnWaitMax;
	private float spawnWaitMin;
	private static bool stop;
	private int randObject;
	private int maxObjects;
	static int spawnedObjects;
	
	#region TableSeats
	//private GameObject[] tables;
	private GameObject[] chairs;
	private int totalChairs;
	private int randomChair;
	private bool[] occupiedSeats;
	#endregion
	
	
    // Start is called before the first frame update
    void Start()
    {
    	
        #region TableSeats
        chairs = GameObject.FindGameObjectsWithTag("Chair");
        totalChairs = chairs.Length;
        #endregion
        
        
        spawnedObjects = 0;
        spawnWaitMin = 1;
        spawnWaitMax = 5;
        stop = false;
        maxObjects = totalChairs;
        StartCoroutine(WaitSpawner());
        
    }

    // Update is called once per frame
    void Update()
    {
    	spawnWait = Random.Range(spawnWaitMin, spawnWaitMax);
    	randomChair = Random.Range(0,totalChairs-1); //Select a chair to spawn a client
    	while(chairs[randomChair].GetComponent<ChairScript>().IsOccupied){ //Select free chair
    		randomChair = Random.Range(0,totalChairs); //Select a chair to spawn a client
    	}
    	
    }
    
    public void StopSpawner(){
    	stop = true;
    }
    
    IEnumerator WaitSpawner(){
    	yield return new WaitForSeconds(3); //Wait and return saving state
    	
    	
    	while(!stop && spawnedObjects < maxObjects ){
    		//Instanciar al cliente en la misma posición y rotación que la silla random
    		GameObject objectInstance = Instantiate(client, chairs[randomChair].transform.position, chairs[randomChair].transform.rotation);
    		//Cambiar la silla a estado de "Ocupada"
    		chairs[randomChair].GetComponent<ChairScript>().IsOccupied = true;
    		//Relate chair to client
    		GameObject chairObj = chairs[randomChair];
    		objectInstance.GetComponent<ClientController>().setChair(ref chairObj);
    		
    		objectInstance.transform.SetParent(transform); //Setze den Feind innerhalb des Spawns
    		spawnedObjects++;
    		
    		yield return new WaitForSeconds(spawnWait);
    	}
    }
    
}
