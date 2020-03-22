using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class rocket : MonoBehaviour
{
    Rigidbody rigid_body;
    AudioSource thrustsound;

    enum State {Alive, Dying, Trascending}
    State state = State.Alive;

    [SerializeField] float rotation_speed = 100f;
    [SerializeField] float thrust_speed = 50f;


    void Start()
    {
        rigid_body = GetComponent<Rigidbody>();
        thrustsound = GetComponent<AudioSource>();
        rigid_body.angularVelocity = Vector3.zero;
        //rigid_body.constraints = RigidbodyConstraints.FreezeRotationY;
        //rigid_body.constraints = RigidbodyConstraints.FreezeRotationX;

    }

    
    void Update()
    {   if (state == State.Alive)
        {
            Rocket_thrust();
            Rotate();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(state != State.Alive) { return; }
      
        switch (collision.gameObject.tag)
        {
            case "friendly":
               
                break;
            case "finish":
                state = State.Trascending;
                Invoke("LoadNextScene", 1.5f);
                
                break;
            case "hahaha":
                state = State.Dying;
                Invoke("LoadLevel2", 1.25f);
                break;
            default:
                state = State.Dying;
                Invoke("LoadFirstLevel", 1.25f);
                print("dead");
                break;
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }
    private void LoadLevel2()
    {
        SceneManager.LoadScene(1);
    }

    private void Rocket_thrust()
    {
        if (Input.GetKey(KeyCode.Space)) // can thrust while rotation
        {
            float thrust_speedThisframe = thrust_speed* Time.deltaTime;

            rigid_body.AddRelativeForce(Vector3.up * thrust_speedThisframe);

            if (!thrustsound.isPlaying)
            { thrustsound.Play(); }


        }
        else { thrustsound.Stop(); }

       
    }

    private void Rotate()
    {
        rigid_body.freezeRotation = true; // take mannual control of rotation

        
        float rotationThisframe = rotation_speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            
            transform.Rotate(Vector3.forward * rotationThisframe);
        }
        if (Input.GetKey(KeyCode.D))
        {
            
            transform.Rotate(-Vector3.forward * rotationThisframe);


        }

        rigid_body.freezeRotation = false; // resume physics control of rotation
    }

    
}
