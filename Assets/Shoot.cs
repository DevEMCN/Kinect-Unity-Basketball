using UnityEngine;
using System.Collections;
using Windows.Kinect;
using System.Linq;

public class Shoot : MonoBehaviour
{
    public GameObject ball;
    private Vector3 throwSpeed = new Vector3(0, 26, 40); //This value is a sure basket
    public Vector3 ballPos;
    private bool thrown = false;
    private GameObject ballClone;

    public GameObject availableShotsGO;
    private int availableShots = 5;

    public GameObject meter;
    public GameObject arrow;
    private float arrowSpeed = 0.3f; //Difficulty
    private bool right = true;

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
        counter_cooldown--;
        IsAvailable = _sensor.IsAvailable;

        if (_bodyFrameReader != null)
        {
            var frame = _bodyFrameReader.AcquireLatestFrame();

            if (frame != null)
            {
                frame.GetAndRefreshBodyData(_bodies);

                foreach (var body in _bodies.Where(b => b.IsTracked))
                {
                    IsAvailable = true;
                    //Windows.Kinect.Joint handLeft = body.Joints[JointType.HandLeft];
                    ////if (body.HandRightConfidence == TrackingConfidence.High && body.HandRightState == HandState.Lasso)
                    //if (body.HandLeftState == HandState.Open)
                    //{
                    //    IsFire = true;
                    //}
                    //else
                    //{
                    //    //PaddlePosition = RescalingToRangesB(-1, 1, -8, 8, body.Lean.X);
                    //    Windows.Kinect.Joint handRight = body.Joints[JointType.HandRight];
                    //    PaddlePosition = RescalingToRangesB(-1, 1, -8, 8, handRight.Position.X);
                    //    //handXText.text = PaddlePosition.ToString();
                    //}
                    /* Move Meter Arrow */

                    if (arrow.transform.position.x < 4.7f && right)
                    {
                        arrow.transform.position += new Vector3(arrowSpeed, 0, 0);
                    }
                    if (arrow.transform.position.x >= 4.7f)
                    {
                        right = false;
                    }
                    if (right == false)
                    {
                        arrow.transform.position -= new Vector3(arrowSpeed, 0, 0);
                    }
                    if (arrow.transform.position.x <= -4.7f)
                    {
                        right = true;
                    }

                    /* Shoot ball on Tap */

                    //if (Input.GetButton("Fire1") && !thrown && availableShots > 0)
                    if(body.HandLeftState == HandState.Open && (counter_cooldown <= 0))
                    {
                        counter_cooldown = 50;
                        thrown = true;
                        availableShots--;
                        availableShotsGO.GetComponent<GUIText>().text = availableShots.ToString();

                        ballClone = Instantiate(ball, ballPos, transform.rotation) as GameObject;
                        throwSpeed.y = throwSpeed.y + arrow.transform.position.x;
                        throwSpeed.z = throwSpeed.z + arrow.transform.position.x;

                        ballClone.GetComponent<Rigidbody>().AddForce(throwSpeed, ForceMode.Impulse);
                        //GetComponent<AudioSource>().Play();
                    }

                    /* Remove Ball when it hits the floor */

                    if (ballClone != null && ballClone.transform.position.y < -16)
                    {
                        Destroy(ballClone);
                        thrown = false;
                        throwSpeed = new Vector3(0, 26, 40);//Reset perfect shot variable

                        /* Check if out of shots */

                        if (availableShots == 0)
                        {
                            arrow.GetComponent<Renderer>().enabled = false;
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

    static float RescalingToRangesB(float scaleAStart, float scaleAEnd, float scaleBStart, float scaleBEnd, float valueA)
    {
        return (((valueA - scaleAStart) * (scaleBEnd - scaleBStart)) / (scaleAEnd - scaleAStart)) + scaleBStart;
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