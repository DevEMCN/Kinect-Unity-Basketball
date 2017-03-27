using UnityEngine;
using System.Collections;
using Windows.Kinect;
using System.Linq;
using System;

public class Shoot : MonoBehaviour
{
    // player 1 objects 
    public GameObject player1Ball;
    private Vector3 player1ThrowSpeed = new Vector3(0, 26, 40); //This value is a sure basket
    public Vector3 player1BallPos;
    private bool player1Thrown = false;
    private GameObject player1BallClone;
    public GameObject player1AvailableShotsGO;
    private ulong player1TrackingID;
    private int player1AvailableShots = 5;
    public GameObject player1Meter;
    public GameObject player1Arrow;
    private float player1ArrowSpeed = 0.2f; //Difficulty
    private bool player1Right = true;

    // player 2 objects
    public GameObject player2Ball;
    private Vector3 player2ThrowSpeed = new Vector3(0, 26, 40); //This value is a sure basket
    public Vector3 player2BallPos;
    public GameObject player2AvailableShotsGO;
    private bool player2Thrown = false;
    private GameObject player2BallClone;
    //public GameObject player2AvailableShotsGO;
    private ulong player2TrackingID;
    private int player2AvailableShots = 5;
    public GameObject player2Meter;
    public GameObject player2Arrow;
    private float player2ArrowSpeed = 0.2f; //Difficulty
    private bool player2Right = true;

    public GameObject gameOver;

    private KinectSensor _sensor;
    private BodyFrameReader _bodyFrameReader;
    private Body[] _bodies = null;

    public GameObject kinectAvailableText;
    //public Text handXText;

    public bool IsAvailable;
    public float PaddlePosition;
    public bool IsFire;

    public static Shoot instance = null;

    public Body[] GetBodies()
    {
        return _bodies;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
            Destroy(gameObject);
    }

    // Use this for initialization
    void Start()
    {
        /* Increase Gravity */
        //Physics.gravity = new Vector3(0, -20, 0);
        Physics.gravity = new Vector3(0, -20, 0);
        _sensor = KinectSensor.GetDefault();

        if (_sensor != null)
        {
            IsAvailable = _sensor.IsAvailable;

            //kinectAvailableText.SetActive(IsAvailable);

            _bodyFrameReader = _sensor.BodyFrameSource.OpenReader();

            if (!_sensor.IsOpen)
            {
                _sensor.Open();
            }

            _bodies = new Body[_sensor.BodyFrameSource.BodyCount];
        }
    }


    public int counter_cooldown = 0;
    void FixedUpdate()
    {
        counter_cooldown--; // delay the amount of times you can shoot to once per movement
        IsAvailable = _sensor.IsAvailable;

        if (_bodyFrameReader != null)
        {
            var frame = _bodyFrameReader.AcquireLatestFrame();

            if (frame != null)
            {
                frame.GetAndRefreshBodyData(_bodies);
                int bodyCount = 0;

                foreach (var body in _bodies.Where(b => b.IsTracked))
                {
                    bodyCount++;
                    if (bodyCount == 1)
                    {
                        player1TrackingID = body.TrackingId;
                    }
                    else
                    {
                        player2TrackingID = body.TrackingId;
                        bodyCount = 0;
                    }

                }
                
                foreach (var body in _bodies.Where(b => b.IsTracked))
                {
                    IsAvailable = true;
                   


                    /* get instances of the elbow and shoulders and head*/
                    /*  insert left elbow and shoulder here for 2 hand throw*/
                    Windows.Kinect.Joint elbow = body.Joints[JointType.ElbowLeft];
                    Windows.Kinect.Joint shoulder = body.Joints[JointType.ShoulderLeft];
             
                    Windows.Kinect.Joint hand = body.Joints[JointType.HandLeft];
                    // if the right elbow goes above the right shoulder the power meter becomes
                    // available 
                    if (elbow.Position.Y > shoulder.Position.Y) {
                        /* Move Meter Arrow */
                        /* Move Meter Arrow player 1*/
                        if (player1Arrow.transform.position.x < 24f && player1Right)
                        {
                            player1Arrow.transform.position += new Vector3(player1ArrowSpeed, 0, 0);
                        }
                        if (player1Arrow.transform.position.x >= 24f)
                        {
                            player1Right = false;
                        }
                        if (player1Right == false)
                        {
                            player1Arrow.transform.position -= new Vector3(player1ArrowSpeed, 0, 0);
                        }
                        if (player1Arrow.transform.position.x <= 14.5f)
                        {
                            player1Right = true;
                        }

                        /* move meter arrow player 2*/
                        if (player2Arrow.transform.position.x < -14.3f && player2Right)
                        {
                            player2Arrow.transform.position += new Vector3(player2ArrowSpeed, 0, 0);
                        }
                        if (player2Arrow.transform.position.x >= -14.3f)
                        {
                            player2Right = false;
                        }
                        if (player2Right == false)
                        {
                            player2Arrow.transform.position -= new Vector3(player2ArrowSpeed, 0, 0);
                        }
                        if (player2Arrow.transform.position.x <= -23.7f)
                        {
                            player2Right = true;
                        }

                        /* Shoot ball on Tap */

                        //if (Input.GetButton("Fire1") && !player1Thrown && player1AvailableShots > 0)
                        //if(body.TrackingId == player1TrackingID && body.HandLeftState == HandState.Open && !player1Thrown && player1AvailableShots > 0)
                        if (body.HandLeftState == HandState.Open && !player1Thrown && player1AvailableShots > 0)
                        {
                            player1Thrown = true;
                            player1AvailableShots--;
                            player1AvailableShotsGO.GetComponent<GUIText>().text = player1AvailableShots.ToString();

                            player1BallClone = Instantiate(player1Ball, player1BallPos, transform.rotation) as GameObject;
                            //player1ThrowSpeed.y = player1ThrowSpeed.y + player1Arrow.transform.position.x;// * 0.7F;

                            player1ThrowSpeed.y =  player1ThrowSpeed.y + (hand.Position.Y - shoulder.Position.Y) * 2.5F;
                            player1ThrowSpeed.z = player1ThrowSpeed.z + (hand.Position.Y - shoulder.Position.Y) * 2.0F;

                            //player1ThrowSpeed.z = player1ThrowSpeed.z + player1Arrow.transform.position.x * 0.8F;
                            player1ThrowSpeed.x = -10; // players are out to the side 

                            

                            player1BallClone.GetComponent<Rigidbody>().AddForce(player1ThrowSpeed, ForceMode.Impulse);
                            //GetComponent<AudioSource>().Play();
                        }
                        //if (body.TrackingId == player2TrackingID && body.HandLeftState == HandState.Open && !player2Thrown && player2AvailableShots > 0)
                        if(body.HandRightState == HandState.Open && !player2Thrown && player2AvailableShots > 0)
                        {
                            player2Thrown = true;
                            player2AvailableShots--;
                            //player2AvailableShotsGO.GetComponent<GUIText>().text = player2AvailableShots.ToString();

                            player2BallClone = Instantiate(player2Ball, player2BallPos, transform.rotation) as GameObject;
                            //player2ThrowSpeed.y = Math.Abs(player2ThrowSpeed.y) + Math.Abs(player2Arrow.transform.position.x);
                            //player2ThrowSpeed.z = Math.Abs(player2ThrowSpeed.z) + Math.Abs(player2Arrow.transform.position.x);

                            player1ThrowSpeed.y = player1ThrowSpeed.y + (hand.Position.Y - shoulder.Position.Y) * 2.5F;
                            player1ThrowSpeed.z = player1ThrowSpeed.z + (hand.Position.Y - shoulder.Position.Y) * 1.3F;

                            player2ThrowSpeed.x = 15; // players are out to the side 

                            player2BallClone.GetComponent<Rigidbody>().AddForce(player2ThrowSpeed, ForceMode.Impulse);
                            //GetComponent<AudioSource>().Play();
                        }
                        //if (body.HandLeftState == HandState.Open && (counter_cooldown <= 0) && availableShots > 0)
                        //{
                        //    counter_cooldown = 100; // delay next available shot
                        //    thrown = true;
                        //    availableShots--;
                        //    availableShotsGO.GetComponent<GUIText>().text = availableShots.ToString();

                        //    ballClone = Instantiate(ball, ballPos, transform.rotation) as GameObject;
                        //    //throwSpeed.y = throwSpeed.y + arrow.transform.position.x;
                        //    //throwSpeed.z = throwSpeed.z + arrow.transform.position.x;
                        //    // justification for following code.
                        //    // because the ball can only be thrown when your elbow is above your shoulder
                        //    // the only way to move your hand closer to your shoulder would be to bend your elbow back
                        //    // in a throwing motion
                        //    // hence the throw power is determined by the distance between the height of the hand and
                        //    // height of the shoulder
                        //    throwSpeed.y = throwSpeed.y + (hand.Position.Y - shoulder.Position.Y) * 2;
                        //    throwSpeed.z = throwSpeed.z + (hand.Position.Y - shoulder.Position.Y) * 2;
                        //    ballClone.GetComponent<Rigidbody>().AddForce(throwSpeed, ForceMode.Impulse);


                        //}
                    }


                    /* Remove Ball when it hits the floor */

                    if (player1BallClone != null && player1BallClone.transform.position.y < -16)
                    {
                        Destroy(player1BallClone);
                        player1Thrown = false;
                        player1ThrowSpeed = new Vector3(0, 26, 40);//Reset perfect shot variable

                        /* Check if out of shots */

                        if (player1AvailableShots == 0)
                        {
                            player1Arrow.GetComponent<Renderer>().enabled = false;
                            Instantiate(gameOver, new Vector3(0.31f, 0.2f, 0), transform.rotation);
                            Invoke("restart", 2);
                        }
                    }
                    /* Remove Ball when it hits the floor */

                    if (player2BallClone != null && player2BallClone.transform.position.y < -16)
                    {
                        Destroy(player1BallClone);
                        player2Thrown = false;
                        player2ThrowSpeed = new Vector3(0, 26, 40);//Reset perfect shot variable

                        /* Check if out of shots */

                        if (player2AvailableShots == 0)
                        {
                            player2Arrow.GetComponent<Renderer>().enabled = false;
                            Instantiate(gameOver, new Vector3(0.31f, 0.2f, 0), transform.rotation);
                            Invoke("restart", 2);
                        }
                    }
                }

                frame.Dispose();
                frame = null;
            }
        }

        
    }
    

    void restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    

    void OnApplicationQuit()
    {
        if (_bodyFrameReader != null)
        {
            _bodyFrameReader.IsPaused = true;
            _bodyFrameReader.Dispose();
            _bodyFrameReader = null;
        }

        if (_sensor != null)
        {
            if (_sensor.IsOpen)
            {
                _sensor.Close();
            }

            _sensor = null;
        }
    }
}