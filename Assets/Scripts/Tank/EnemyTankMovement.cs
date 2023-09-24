using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class EnemyTankMovement : MonoBehaviour
{

    public List<Transform> _waypoints = new List<Transform> ();
    private int currentWaypoint;

    // The tank will stop moving towards the player once it reaches this distance
    public float m_CloseDistance = 8f;
    // The tank's turret object
    public Transform m_Turret;
    // A reference to the player - this will be set when the enemy is loaded
    private GameObject m_Player;
    // A reference to the nav mesh agent component
    private UnityEngine.AI.NavMeshAgent m_NavAgent;
    // A reference to the rigidbody component
    private Rigidbody m_Rigidbody;
    // Will be set to true when this tank should follow the player
    private bool m_Follow;
    // Use this for initialization
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        if (m_Follow == false)
        {
            if (_waypoints.Count > 0)
            {
                if (Vector3.Distance(transform.position, _waypoints[currentWaypoint].position) > 2)
                {
                    m_NavAgent.SetDestination(_waypoints[currentWaypoint].position);
                }
                else
                {
                    // Increment the currentWaypoint and loop back to the start if necessary
                    currentWaypoint = (currentWaypoint + 1) % _waypoints.Count;
                }
            }
        }
        else
        {

        // get distance from player to enemy tank
        float distance = (m_Player.transform.position - transform.position).magnitude;
        // if distance is less than stop distance, then stop moving
        if (distance > m_CloseDistance)
        {
            m_NavAgent.SetDestination(m_Player.transform.position);
            m_NavAgent.isStopped = false;
        }
        else
        {
            m_NavAgent.isStopped = true;
        }
        if (m_Turret != null)
        {
            m_Turret.LookAt(m_Player.transform);
        }
        }
    }
    private void Awake()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_NavAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Follow = false;
    }

    private void OnEnable()
    {
        // when the tank is turned on, make sure it is not kinematic
        m_Rigidbody.isKinematic = false;
    }
    private void OnDisable()
    {
        // when the tank is turned off, set it to kinematic so it stops moving
        m_Rigidbody.isKinematic = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            m_Follow = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            m_Follow = false;
        }
    }

    
}
