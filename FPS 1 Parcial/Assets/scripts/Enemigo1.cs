using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemigo1 : MonoBehaviour
{
    public int rutina;
    public float cronometro;
    public Animator ani;
    public Quaternion angulo;
    public float grado;

    public GameObject target;
    public bool atacando;

    public NavMeshAgent agente;
    public float distancia_ataque;
    public float radio_vision;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        target = GameObject.Find("Mutant");
        agente = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Comportamiento_enemigo();
    }

    public void Comportamiento_enemigo()
    {
        float distancia = Vector3.Distance(transform.position, target.transform.position);

        if (distancia > radio_vision) // Si el jugador está fuera de la visión del enemigo
        {
            agente.enabled = false;
            ani.SetBool("run", false);
            cronometro += Time.deltaTime;

            if (cronometro >= 4)
            {
                rutina = Random.Range(0, 2);
                cronometro = 0;
            }

            switch (rutina)
            {
                case 0:
                    ani.SetBool("walk", false);
                    break;
                case 1:
                    grado = Random.Range(0, 360);
                    angulo = Quaternion.Euler(0, grado, 0);
                    rutina++;
                    break;
                case 2:
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, angulo, 0.5f);
                    transform.Translate(Vector3.forward * speed * Time.deltaTime);
                    ani.SetBool("walk", true);
                    break;
            }
        }
        else // Si el jugador está en el rango de visión
        {
            agente.enabled = true;
            agente.SetDestination(target.transform.position);
            transform.LookAt(target.transform.position);

            if (distancia > distancia_ataque && !atacando) // Persigue al jugador
            {
                ani.SetBool("walk", false);
                ani.SetBool("run", true);
            }
            else if (!atacando) // Si está en distancia de ataque
            {
                StartCoroutine(RealizarAtaque());
            }
        }
    }

    IEnumerator RealizarAtaque()
    {
        atacando = true;
        ani.SetBool("attack", true);
        ani.SetBool("run", false);
        ani.SetBool("walk", false);
        agente.enabled = false;

        yield return new WaitForSeconds(0.5f); // Ajustar tiempo para el golpe

        if (Vector3.Distance(transform.position, target.transform.position) <= distancia_ataque)
        {
            target.GetComponent<vidajugador>().restarvida(10); // Aplica daño al jugador
        }

        yield return new WaitForSeconds(1f); // Esperar antes de otro ataque
        Final_ani();
    }

    public void Final_ani()
    {
        ani.SetBool("attack", false);
        atacando = false;
        agente.enabled = true;
    }
}

