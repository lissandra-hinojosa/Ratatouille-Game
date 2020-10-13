using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteSpawner : MonoBehaviour
{
	[SerializeField]
	private int maxObjectsOnScreen = 10;
	private Vector3 spawnValues;
	private float spawnWait;
	[SerializeField]
	private float spawnWaitMin = 1;
	[SerializeField]
	private float spawnWaitMax = 4;
	private static bool stop;
	private int randObject;
	[SerializeField]
	private GameObject[] objects;
	static int spawnedObjects;
	
	
	
    // Start is called before the first frame update
    void Start()
    {
        spawnedObjects = 0;
        //spawnWaitMin = 1;
        //spawnWaitMax = 4;
        stop = false;
        //maxObjectsOnScreen = 10; //Max Objects per element on screen
        StartCoroutine(WaitSpawner());
        spawnValues = new Vector3(20, transform.position.y, 20); //Terrain size. Spawner Container must be in the middle of terrain
        
    }

    // Update is called once per frame
    void Update()
    {
    	spawnWait = Random.Range(spawnWaitMin, spawnWaitMax); //Wait different times to spawn an object
    	
    }
    
    public void StopSpawner(){
    	stop = true;
    }
    
    public void DestroyObjects(){
    	foreach(GameObject obj in objects){
    		GameObject[] collection = GameObject.FindGameObjectsWithTag(obj.tag);
    		foreach(GameObject element in collection){
    			Destroy(element);
    		}
    	}
    }
    
    IEnumerator WaitSpawner(){
    	yield return new WaitForSeconds(3); //Wait and return saving state
    	
    	
    	while(!stop){ //Infinite spawining
			randObject = Random.Range(0, objects.Length); //Select random object to spawn
			int localObjLength = GameObject.FindGameObjectsWithTag(objects[randObject].tag).Length;
			//Debug.Log(objects[randObject].tag+": "+localObjLength);
			
			if(localObjLength < maxObjectsOnScreen){ ////If max elements on screen is not yet reached, keep spawning. If it is reached, wait until object is destroyed
				//Quadratische Bereich zum Platzieren von Objekten
    			Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), 1, Random.Range(-spawnValues.z, spawnValues.z));
    			//Objekt erstellen
    			GameObject objectInstance = Instantiate(objects[randObject], spawnPosition+transform.TransformPoint(0,0,0), gameObject.transform.rotation);
    			objectInstance.transform.SetParent(transform); //Setze den Feind innerhalb des Spawns
    			spawnedObjects++;	
			}
		
			yield return new WaitForSeconds(spawnWait);
    		
    	}
    }
}
