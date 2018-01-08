using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
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

    bool thruster_play;
    bool thruster_ToggleChange;
    bool collisionsDisabled = false;
    bool isTransitioning = false;

    public int counter;
    public int successPlay;

    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        thruster = GetComponent<AudioSource>();
        thruster_play = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTransitioning)
        {
            RespondToThrustInput();
            RespodToRotateInput();
        }
        if (Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }

        const float tau = Mathf.PI * 2f;
        print(Mathf.Sin((tau / 4f)));
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextScene();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionsDisabled = !collisionsDisabled;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning || collisionsDisabled) { return; }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                //Do nothing
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                if (counter < 1 && successPlay < 1)
                {
                    StartDeathSequence();
                }
                break;
        }
    }

    private void StartDeathSequence()
    {
        isTransitioning = true;
        counter++;
        thruster.Stop();
        deathParticles.Play();
        thruster.PlayOneShot(death);
        mainEngineParticles.Stop();
        Invoke("LoadFirstLevel", 2f);

    }



    private void StartSuccessSequence()
    {
        if (successPlay < 1)
        {
            Landed();
        }
    }

    private void Landed()
    {
        isTransitioning = true;
        thruster.Stop();
        thruster.PlayOneShot(success);
        successPlay++;
        successParticles.Play();
        Invoke("LoadNextScene", 2f);
    }

    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings) 
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
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
            StopApplyingThrust();

        }
    }

    private void StopApplyingThrust()
    {
        thruster.Stop();
        mainEngineParticles.Stop();
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
        rigidBody.angularVelocity = Vector3.zero; // remove rotation due to physics

        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
    }
}