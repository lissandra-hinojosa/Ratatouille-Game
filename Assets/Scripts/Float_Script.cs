using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Float_Script : MonoBehaviour
{
	float speed = 1f;
    float delta = 3f;  //delta is the difference between min y to max y.
   
    // Update is called once per frame
    void Update()
    {
        float y = Mathf.PingPong(speed * Time.time, delta);
     	Vector3 pos = new Vector3(transform.position.x, y, transform.position.z);
     	transform.position = pos;
    }
}
