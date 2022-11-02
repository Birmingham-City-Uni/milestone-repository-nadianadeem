package NN;
import robocode.*;
import static robocode.util.Utils.normalRelativeAngleDegrees;
import java.awt.Color;

// API help : http://robocode.sourceforge.net/docs/robocode/robocode/Robot.html


//NadiaRobot - a robot by (Nadia)

public class NadiaRobot extends Robot
{
	
	//run: NadiaRobot's default behavior
	 
	public void run() {
		// Initialization of the robot should be put here

		// After trying out your robot, try uncommenting the import at the top,
		// and the next line:

		setColors(Color.red,Color.blue,Color.green); // body,gun,radar

		// Robot main loop
		while(true) {
			int randomInt = (int)Math.random() * 4;

			if(randomInt == 1)
			{
				ahead(200);
				turnGunRight(360);
			}
			else if(randomInt == 2)
			{
				back(200);
				turnGunLeft(360);
			}
			else if(randomInt == 3)
			{
				turnRight(90);
				ahead(200);
				turnGunRight(360);
			}
			else
			{
				turnLeft(90);
				back(200);
				turnGunLeft(360);
			}
		}
	}

	/**
	 * onScannedRobot: What to do when you see another robot
	 */
	public void onScannedRobot(ScannedRobotEvent e) {
		int randomInt = (int)Math.random() * 2;
		// Replace the next line with any behavior you would like
		if(randomInt == 1)
		{
			if (e.getBearing() >= 0) {
				turnRight(1);
			} else {
				turnLeft(1);
			}
	
			turnRight(e.getBearing());
			ahead(e.getDistance() + 5);
			turnGunRight(normalRelativeAngleDegrees(e.getBearing() + (getHeading() - getRadarHeading())));
			fire(2.5);
		}
		else
		{
			fire(3.5);
		}
	}

	public void onHitRobot(HitRobotEvent e) {
		if (e.getBearing() >= 0) {
			turnRight(1);
		} else {
			turnLeft(1);
		}
		turnRight(e.getBearing());

		// Determine a shot that won't kill the robot...
		// We want to ram him instead for bonus points
		if (e.getEnergy() > 16) {
			fire(3);
		} 
		else if (e.getEnergy() > 10) {
			fire(2);
		} 
		else if (e.getEnergy() > 4) {
			fire(1);
		} 
		else if (e.getEnergy() > 2) {
			fire(.5);
		} 
		else if (e.getEnergy() > .4) {
			fire(.1);
		}
		ahead(40); // Ram him again!
		flee();
	}

	/**
	 * onHitByBullet: What to do when you're hit by a bullet
	 */
	public void onHitByBullet(HitByBulletEvent e) {
		// Replace the next line with any behavior you would like
		back(40);
		flee();
	}
	
	/**
	 * onHitWall: What to do when you hit a wall
	 */
	public void onHitWall(HitWallEvent e) {
		// Replace the next line with any behavior you would like
		int randomInt = (int)Math.random() * 2;

		if(randomInt == 1)
		{
			turnRight(90);
			ahead(100);
			turnGunRight(360);
		}
		else if(randomInt == 2)
		{
			turnLeft(90);
			ahead(100);
			turnGunLeft(360);
		}
	}
	
	public void flee()
	{
		int randomInt = (int)Math.random() * 2;

		if(randomInt == 1)
		{
			turnRight(360);
			back(200);
			turnGunRight(360);
		}
		else if(randomInt == 2)
		{
			turnLeft(360);
			back(200);
			turnGunLeft(360);
		}
	}
}
