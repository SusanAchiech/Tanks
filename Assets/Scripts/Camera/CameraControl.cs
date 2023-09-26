using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float m_DampTime = 0.2f;
    public Transform m_target;
    private Vector3 m_MoveVelocity;
    private Vector3 m_DesiredPosition;
    private Vector3 cameraOffset;
    private void Awake()
    {

        m_target = GameObject.FindGameObjectWithTag("Player").transform;

        cameraOffset = transform.position - m_target.transform.position;
    }
    private void FixedUpdate()
    {
        Move();
    }
    private void Move()
    {
        m_DesiredPosition = m_target.position + cameraOffset;
        transform.position = Vector3.SmoothDamp(transform.position,
        m_DesiredPosition, ref m_MoveVelocity, m_DampTime);
    }
}
