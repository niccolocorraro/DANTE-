using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputM : MonoBehaviour
{
    public Vector2 MoveInput;
   
    public void OnMove(InputValue input) => MoveInput = input.Get<Vector2>();
}
