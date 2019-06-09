using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup_BallSplit : Powerup {

	public override void PickedUp ()
	{
		//Write code here when picked up
		Ball[] balls = FindObjectsOfType<Ball>();
		foreach (Ball ball in balls) {
			Vector3 targetVelocity = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)) * ball.GetComponent<Ball>().speed;
            GameObject newBall = Instantiate (ball.gameObject, ball.transform.position, Quaternion.identity);
			newBall.GetComponent<Ball> ().isStuck = false;
			newBall.GetComponent<Ball> ().velocity = targetVelocity;
            newBall.GetComponent<Rigidbody>().velocity = targetVelocity;
			GameManager.instance.BallAliveAdded (newBall);

            Vector3 targetVelocity2 = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)) * ball.GetComponent<Ball>().speed;
            GameObject newBall2 = Instantiate(ball.gameObject, ball.transform.position, Quaternion.identity);
            newBall2.GetComponent<Ball>().isStuck = false;
            newBall2.GetComponent<Ball>().velocity = targetVelocity2;
            newBall2.GetComponent<Rigidbody>().velocity = targetVelocity2;
            GameManager.instance.BallAliveAdded(newBall2);
        }





		//GameObject newBall = Instantiate (ballPrefab,ballHolderPosition, Quaternion.identity);
		//aliveBalls.Add (newBall);
		//ballsLeft -= 1;	
		//UpdateBallsUI();
		base.PickedUp ();
	}

}
