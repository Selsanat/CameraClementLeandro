using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float speed = 10.0f;

	Rigidbody _rigidbody = null;
	protected bool IsActive { get; private set; }

	public void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
	}

	void FixedUpdate()
    {
		Vector3 direction = Vector3.zero;
		direction += Input.GetAxisRaw("Horizontal") * transform.right;
		direction += Input.GetAxisRaw("Vertical") * transform.forward;
        direction.Normalize();
		_rigidbody.velocity = direction * speed + transform.up * _rigidbody.velocity.y;
	}
}
