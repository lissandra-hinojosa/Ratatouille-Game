using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaintainRotation : MonoBehaviour
{
	private Quaternion _initialRotation;
	private float rotationSpeed = 10f;
	private GameObject _cameraToLookAt;
	
    // Start is called before the first frame update
    
    void Start()
    {
        _initialRotation = transform.rotation;
        _cameraToLookAt = GameObject.FindGameObjectWithTag("FocusPoint");
    }

    // Update is called once per frame
    void Update()
    {
		SetRotate(this.gameObject, _cameraToLookAt);
    }
    
    void SetRotate(GameObject toRotate, GameObject camera){
    	Quaternion copyRotation = new Quaternion(_initialRotation.x,camera.transform.rotation.y,_initialRotation.z,camera.transform.rotation.w);
        transform.rotation = Quaternion.Lerp(toRotate.transform.rotation, copyRotation, rotationSpeed * Time.deltaTime);        
    }
}
