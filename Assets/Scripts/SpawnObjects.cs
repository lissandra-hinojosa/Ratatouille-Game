using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
	[SerializeField]
	private GameObject[] objects;
	private Vector3 spawnValues;
	private float spawnWait;
	private float spawnWaitMax;
	private float spawnWaitMin;
	private static bool stop;
	private int randObject;
	private int maxObjects;
	static int spawnedObjects;
	
    // Start is called before the first frame update
    void Start()
    {
        spawnedObjects = 0;
        spawnWaitMin = 1;
        spawnWaitMax = 4;
        stop = false;
        maxObjects = 7;
        StartCoroutine(WaitSpawner());
        spawnValues = new Vector3(5, transform.position.y, 5); //Spawn objects around the spawner
        
    }

    // Update is called once per frame
    void Update()
    {
    	spawnWait = Random.Range(spawnWaitMin, spawnWaitMax);
    }
    
    public void StopSpawner(){
    	stop = true;
    }
    
    IEnumerator WaitSpawner(){
    	yield return new WaitForSeconds(3); //Wait and return saving state
    	
    	
    	while(!stop && spawnedObjects < maxObjects ){
    		randObject = Random.Range(0, objects.Length); //Select a random object to instantiate
    		//Quadratische Bereich zum Platzieren von Objekten
    		Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), 1, Random.Range(-spawnValues.z, spawnValues.z));
    		
    		//Objekt erstellen
    		GameObject objectInstance = Instantiate(objects[randObject], spawnPosition+transform.TransformPoint(0,0,0), gameObject.transform.rotation);
    	
    		objectInstance.transform.SetParent(transform); //Setze den Feind innerhalb des Spawns
    		spawnedObjects++;
    		
    		yield return new WaitForSeconds(spawnWait);
    	}
    }
}
