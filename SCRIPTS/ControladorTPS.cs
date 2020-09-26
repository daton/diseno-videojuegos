using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorTPS : MonoBehaviour
{
    CharacterController caracter; // Referencia a tu personaje de Mixamos por ejemplo la kachugin
    private Transform camara;                  // Referencia a la camara principal
    private Vector3 camara_forward;             // Direccion de la camara actual.
    private Vector3 movimiento;                 //Direccion del movimiento del personaje
    Animator animator;
    Rigidbody rigidbody;

    public float rapidezGiro = 20f;
    Quaternion rotacion = Quaternion.identity;


    // El método Start si ejecuta una sola vez al inicio de la escena en la cual esta siendo ocupado
    void Start()
    {
        //Inicamos cada uno de los atributos
        animator = GetComponent<Animator>();
        caracter = GetComponent<CharacterController>();
        rigidbody = GetComponent<Rigidbody>();
        camara = Camera.main.transform;
    

    }
    //Este es como el update pero con un rate fijo.
    private void FixedUpdate()
    {
        //Leemos las entradas de los botones de flechas

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");


        // calculamos los movimientos
        if (camara != null)
        {
            // calculamos la camara relativa al movimiento de la misma:
            camara_forward = Vector3.Scale(camara.forward, new Vector3(1, 0, 1)).normalized;
            movimiento = v * camara_forward + h * camara.right;
        }
        else
        {
            // Si no hay camara calculamos los movimientos relativos
            movimiento = v * Vector3.forward + h * Vector3.right;
        }

        //Activamos la animacion para correr o caminar
        bool hasHorizontalInput = !Mathf.Approximately(h, 0f);
        bool hasVerticalInput = !Mathf.Approximately(v, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        animator.SetBool("IsWalking", isWalking);


        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, movimiento, rapidezGiro * Time.deltaTime, 0f);
          rotacion = Quaternion.LookRotation(desiredForward);


    }
 
    void OnAnimatorMove()
    {
        rigidbody.MoveRotation(rotacion);

        //Inicio brinco
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("Brincar");
            animator.SetTrigger("jumping");
        }

        caracter.Move(movimiento * 4 * Time.deltaTime);
  
    }
}
