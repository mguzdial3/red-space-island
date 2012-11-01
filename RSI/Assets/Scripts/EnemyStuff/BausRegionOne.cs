using UnityEngine;
using System.Collections;

public class BausRegionOne : BasicEnemy {	
	public float timer =0.0f;
	public int state =0;
	public int stateCounter=3;
	public const int MELEE = 1;
	public const int BULLRUSH =2;
	public const int LAZAR = 3;
	
	
	public Vector3 goalLocation;
	private bool rushing;
	
	//Things to be changed per level
	public int availableAttacks=1;
	public float speed=0.3f;
	public float rangeOfSight=30.0f;
	public float waitTime=1.0f;
	
	public Projectile punchProjectile;
	public float rangeOfPunch = 2.0f;
	public float speedOfPunch = 7.0f;
	
	public int jumpDamage=10;
	
	public Projectile lazar;
	public float rangeOfLazar = 10.0f;
	//How long lazar lasts
	public float speedOfLazar=3.0f;
	
	
	
	// Update is called once per frame
	void Update () {
		
		if(stateCounter>0){
			switch(state){
			case 0:
				Waiting();
				break;
			case MELEE:
				Melee();
				break;
			case BULLRUSH:
				Rushing();
				break;
			case LAZAR:
				Lazaring();
				break;
				
			}
		}
		else{
			//Generate new state
			stateCounter=3;
			float midState = Random.Range(0.0f,(float)(availableAttacks+1));
			
			//print("MidState: "+midState);
			if(midState>0.3 && midState<1.0f){
				//state=MELEE;
				state=2;//LAZAR;
			}
			else if(midState>1.0f && midState<2.0f){
				
				//GONNA HAVE TO BE BULLRUSH
				state=2;//BULLRUSH;
			}
			else if(midState>2.0f && midState<3.0f){
				state=2;//LAZAR;
			}
			else{
				state=2;//0;
			}
			
			if(state==0){
				//print("Got to wait once again");
				timer=waitTime;
			}
			print("State was: "+state);
		}
		
	}
	
	/** 
	 * Causes boss to wait for specified amount of time
	 * 
	 */
	public void wait(float time){
		timer=time;
		state=0;
	}
	
	void Waiting(){
		if(timer<=0.0f){
			stateCounter=0;
				
		}
		else{
			timer-=Time.deltaTime;
		}
	}
	
	void Melee(){
		if(goalLocation==Vector3.zero){
			goalLocation=FindClosestPlayer(rangeOfSight).transform.position;
		}
		else{
			Vector3 differenceToGoal = goalLocation-transform.position;
			if(differenceToGoal.magnitude<1){
				//Punch time
				Punch();
				
				goalLocation=Vector3.zero;
				stateCounter--;
			}
			else{
				transform.position=Vector3.Lerp(transform.position,goalLocation,Time.deltaTime*speed);
			}
		}
		
	}
	
	void Lazaring(){
		if(goalLocation==Vector3.zero){
			goalLocation=FindClosestPlayer(rangeOfSight).transform.position;
		}
		else{
			Vector3 differenceToGoal = goalLocation-transform.position;
			if(differenceToGoal.magnitude<1){
				//Punch time
				LazarShoot();
				
				goalLocation=Vector3.zero;
				stateCounter--;
			}
			else{
				transform.position=Vector3.Lerp(transform.position,goalLocation,Time.deltaTime*speed);
			}
		}
	}
	
	
	void Rushing(){
		rushing=true;
		if(goalLocation==Vector3.zero){
			goalLocation=FindClosestPlayer(rangeOfSight).transform.position;
		}
		else{
			//print("RUSHING");
			Vector3 differenceToGoal = goalLocation-transform.position;
			if(differenceToGoal.magnitude<1){
				//Gone far enough
				goalLocation=Vector3.zero;
				stateCounter--;
				rushing=false;
				
			}
			else{
				transform.position=Vector3.Lerp(transform.position,goalLocation,Time.deltaTime*speed*2);
			}
		}
		
	}
	
	void Punch(){
		if(transform.position.x<goalLocation.x){
		
			Vector3 spawnPos = transform.position;
			spawnPos.z=transform.position.z;
			spawnPos.x+=0.5f;
					
			Projectile proj = Network.Instantiate(punchProjectile,spawnPos,transform.rotation,1) as Projectile;
			proj.shoot(false,speedOfPunch,rangeOfPunch);
		}
		else{
			Vector3 spawnPos = transform.position;
			spawnPos.z=transform.position.z;
			spawnPos.x-=0.5f;
					
			Projectile proj = Network.Instantiate(punchProjectile,spawnPos,transform.rotation,1) as Projectile;
			proj.shoot(true,speedOfPunch,rangeOfPunch);
		}
	}
	
	
	
	
	//Lazar will go in both directions
	void LazarShoot(){
				
		//Instantiate both parts
		//One on left
		
		Vector3 leftPos = transform.position;
		leftPos.x=transform.position.x-lazar.transform.localScale.x/2-lazar.gameObject.transform.localScale.x/2;
		Projectile proj = Network.Instantiate(lazar,leftPos,lazar.transform.rotation,1) as Projectile;
		proj.shoot(true,speedOfLazar,rangeOfLazar);
		
		//One on right
		Vector3 rightPos = transform.position;
		rightPos.x=transform.position.x+lazar.transform.localScale.x/2+lazar.gameObject.transform.localScale.x/2;
		
		Projectile proj2 = Network.Instantiate(lazar,rightPos,lazar.transform.rotation,1) as Projectile;
		proj2.shoot(true,speedOfLazar,rangeOfLazar);
		
		wait(speedOfLazar);
		
		
	}
	
	void OnTriggerEnter(Collider other){
		if(rushing){
			if(other.tag=="Player"){
				print("Hit playert");
				Player playah = other.gameObject.GetComponent("Player") as Player;
				playah.health-=jumpDamage;
			}
		}
	}
	void OnCollisionEnter(Collision other){
		if(rushing){
			if(other.gameObject.tag=="Player"){
				print("Hit playerxc");
				Player playah = other.gameObject.GetComponent("Player") as Player;
				playah.health-=jumpDamage;
			}
		}
	}
	
	public void applyDamage(int amount) {
		//print ("test2");
		health -= amount;
		if (health <= 0) {
			Network.Destroy (gameObject);
		}
	}
}