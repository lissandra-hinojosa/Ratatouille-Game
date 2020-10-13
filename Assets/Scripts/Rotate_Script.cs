using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate_Script : MonoBehaviour
{
    // Start is called before the first frame update
    private float _rotateX, _rotateY, _rotateZ;
    
    void Start()
    {	
    	_rotateX = Random.Range(0,30);
    	_rotateY = Random.Range(0,30);
    	_rotateZ = Random.Range(0,30);

        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(_rotateX,_rotateY,_rotateZ) * Time.deltaTime);

    }
}
