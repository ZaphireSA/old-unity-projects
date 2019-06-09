using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour {

    [SerializeField]
    float timeBeforeDeath = 5f;
	
	void Start () {
        Destroy(gameObject, timeBeforeDeath);
	}

}
