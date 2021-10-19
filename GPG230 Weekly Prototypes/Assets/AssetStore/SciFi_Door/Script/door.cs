using UnityEngine;
using System.Collections;

public class door : MonoBehaviour {
	public Animation thedoor;

    void OnTriggerEnter ( Collider obj  ){
	thedoor.Play("open");
}

void OnTriggerExit ( Collider obj  ){
	thedoor.Play("close");
}
}