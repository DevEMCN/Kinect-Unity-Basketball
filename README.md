# Kinect-Unity-Basketball
A basketball game built in Unity and played using the Kinect v2

# Table of Contents
* [Introduction](#introduction)
* [Installation](#installation)
* [Project Details](#details)

<a name="introduction"></a><b>Introduction</b><br/>
Team: Eamon McNicholas & Alan Niemiec
With the aim of this module focusing on gestures we have decided to implement multiple gestures using a Basketball free throw game in Unity as a basis for this research. Our technology of choice has been the Kinect v2 as the Kinect v1 version did not allow for the tracking of as many joints as we needed.<br/>
The application allows one or two users to compete in a free throw basketball game over who can get more shots into the basket. The application works whether or not there is a second user.The players use their raised left hand to throw the basketball at the correct power level.

Game code sourced from : https://code.tutsplus.com/tutorials/create-a-basketball-free-throw-game-with-unity--cms-21203
and : https://github.com/tutsplus/BasketballFreeThrowUnity

Body Joints List : https://www.codeproject.com/articles/743862/kinect-for-windows-version-body-tracking <br/>
The UI has multiple elements to help the user:<br/>

* In the top left there is a camera output that shows what the Kinect can see. We have tried but couldn't get it to output the Skeleton or Joints of the current users.

* Top Middle – This is the basketball hoop that the players have to shoot at.

* Bottom left & right – The user scores are shown as digit values in both corners. They increase when the user gets a basket.

* Bottom middle – This is the outline of the basketball to imitate the baskebtall being held by the user.

* Bottom left & right of the basketball outline – These are the power bars. When they start moving the user knows that the Kinect is ready to shoot.


<a name="installation"></a><b>Installation</b><br/>
To play the game you will need the Kinect V2 equipment for Windows as well as the Kinect SDK. You Will also need Unity.

#<a name="details"></a><b>Project Details</b><br/>

There are three gestures in this application both of which imitate an actual throw in basketball:

* Closed fist – The user has to start the the game with the fist closed to imitate the holding of a basketball. This is also crucial for the shooting as the Kinect shoots as soon as the hand is opened.

* Arm raised above shoulder – In order for Kinect to allow the user to shoot, the users arm must be raised above his shoulder. The user can then Move his hand back and forth to adjust the power of the shot. The distance between the hand and shoulder joints is the force that is added to the ball.

* Open hand gesture – Once the user completes the predefined action of raising back the arm he has to shoot by opening the palm of his hand and swinging forward.

We have determined for these to be the most natural actions a person would take during a basketball throw. Apart from the closed fist which was a necessity these imitate the actions taken during a throw.

A dribbling gesture could also be implemented to allow for the bouncing of the ball from the ground (perhaps with possible sound feedback) but we didn't have enough time to implement it.

