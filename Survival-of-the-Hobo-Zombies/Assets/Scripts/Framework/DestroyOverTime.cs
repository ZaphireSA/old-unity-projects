using UnityEngine;
using System.Collections;

public class DestroyOverTime : MonoBehaviour {

    public float duration = 2f;

	void Start () {
        Destroy(gameObject, duration);
	}

}
