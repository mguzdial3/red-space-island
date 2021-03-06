using UnityEngine;
using System.Collections;

//To be inherited from
public class BasicEnemy : MonoBehaviour {
	public int health = 10;
	public float rangeOfVision = 10.0f;
	
	//Damage this enemy
	public void damageEnemy(int damage){
		health-=damage;
		
		if(health<0){
			Destroy(gameObject);
		}
	}
	
	//Returns closest player within some minimum
	public GameObject FindClosestPlayer(float rangeOfSight){
		//This is a pretty expensive way to do this, if we can find a better way, that'd be good
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		
		//Has to be within this value to be found
		Vector3 minimum = new Vector3(rangeOfSight,0,rangeOfSight);
		GameObject playerToReturn = null;
		
		
		
		foreach(GameObject player in players){
			if(player!=null){
				Vector3 difference = (player.transform.position-transform.position);
			
				if(minimum.sqrMagnitude>difference.sqrMagnitude){
					minimum=difference;
					playerToReturn=player;
				}
			}
		}
		return playerToReturn;
		
		
	}
}
