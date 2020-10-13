using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairScript : MonoBehaviour
{
	
	private bool _isOccupied;
    // Start is called before the first frame update
    void Start()
    {
        _isOccupied = false;
    }

    
    public bool IsOccupied{
    get { return _isOccupied; }
    set { _isOccupied = value; }
	}
    
}
