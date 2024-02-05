using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollControl : MonoBehaviour
{
  public Animator animator;
  public Rigidbody[] Rigidbodies;
  public bool test;
  private bool test2;

  private void Awake()
  {
    for (int i = 0; i < Rigidbodies.Length; i++)
    {
      Rigidbodies[i].isKinematic = true;
    }
  }

  private void Update()
  {
    if (test2)
      return;

    if (test)
    {
      animator.enabled = false;
      for (int i = 0; i < Rigidbodies.Length; i++)
      {
        Rigidbodies[i].isKinematic = false;
      }

      test2 = true;
    }
  }
}