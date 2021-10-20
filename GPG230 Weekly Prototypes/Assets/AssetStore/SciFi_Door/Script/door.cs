using UnityEngine;
using System.Collections;

public class door : MonoBehaviour 
{
	public Animation thedoor;
	public AudioSource audioSource;

    void OnTriggerEnter ( Collider obj  )
	{
		thedoor.Play("open");

		if (audioSource)
			audioSource.Play();
	}

	void OnTriggerExit ( Collider obj  )
	{
		thedoor.Play("close");

		if (audioSource)
			audioSource.Play();
	}
}