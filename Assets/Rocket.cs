using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 30f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip death;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem deathParticles;

    Rigidbody rigidBody;
    AudioSource thruster;

    enum State { Alive, Dying, Transcending };
    State state = State.Alive;

    bool thruster_play;
    bool thruster_ToggleChange;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        thruster = GetComponent<AudioSource>();
        thruster_play = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespodToRotateInput();
        } 
	}

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag) 
        {

            case "Friendly":
                //Do nothing
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        thruster.Stop();
        thruster.PlayOneShot(death);
        deathParticles.Play();
        Invoke("LoadFirstLevel", 1f);
    }

    private void StartSuccessSequence()
    {
        state = State.Transcending;
        thruster.Stop();
        thruster.PlayOneShot(success);
        successParticles.Play();
        Invoke("LoadNextScene", 1f);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();

        }
        else
        {
            thruster.Stop();
            mainEngineParticles.Stop();

        }
    }

    private void ApplyThrust()
    {
        float thrustPower = mainThrust * Time.deltaTime;
        rigidBody.AddRelativeForce(Vector3.up * thrustPower);
        if (!thruster.isPlaying)
        {
            thruster.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();
    }

    private void RespodToRotateInput()
    {
        rigidBody.freezeRotation = true; //take manual control of rotation

        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; //resume physics control of rotation
    }
}
