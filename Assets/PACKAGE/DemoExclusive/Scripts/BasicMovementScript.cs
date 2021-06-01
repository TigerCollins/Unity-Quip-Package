using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterController))]
public class BasicMovementScript : MonoBehaviour
{
    [SerializeField]
    private CharacterController _characterController;
    [SerializeField]
    private GravityOptions _gravitySettings;
    [ReadOnly]
    [SerializeField]
    private Vector3 _moveVector; //debug purposes only
    

    // Start is called before the first frame update
    void Awake()
    {
        CheckForNullReference();
        AwakeGravity();
       
        
    }

    private void Start()
    {

    }

    public void CheckForNullReference()
    {
        //If the user hasn't assigned the CharacterController script
        if (_characterController == null)
        {
            //A more effecient version of GetComponent<>
            if (TryGetComponent(out CharacterController newCharacterController))
            {
                _characterController = newCharacterController;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckGravity();
        ApplyGravity();
    }

    void AwakeGravity()
    {
        if (_gravitySettings.useCustomGravity && _gravitySettings.customGravityDetails != _gravitySettings.previousGravityStrength)
        {
            _gravitySettings.previousGravityStrength = _gravitySettings.customGravityDetails;
        }

        else if (Physics.gravity != _gravitySettings.previousGravityStrength)
        {
            _gravitySettings.previousGravityStrength = Physics.gravity;
        }
    }

    void ApplyGravity()
    {
        //Reset the MoveVector
        Debug.Log(gameObject.name + ": Character controller is currently " + _characterController.isGrounded);
        if (_gravitySettings.useCustomGravity && !_characterController.isGrounded)
        {
            _moveVector = Vector3.zero;
            _characterController.Move(_gravitySettings.customGravityDetails * Time.deltaTime);
            _moveVector += _gravitySettings.customGravityDetails;
        }

        else if (!_gravitySettings.useCustomGravity && !_characterController.isGrounded)
        {
            _moveVector = Vector3.zero;
            _characterController.Move(Physics.gravity * Time.deltaTime); ;
            _moveVector += Physics.gravity;
        }
    }

    void CheckGravity()
    {
        if (_gravitySettings.useCustomGravity && _gravitySettings.customGravityDetails != _gravitySettings.previousGravityStrength)
        {
            _gravitySettings.previousGravityStrength = _gravitySettings.customGravityDetails;
            _gravitySettings.onCustomGravityStrengthChange.Invoke();
        }

        else if (Physics.gravity != _gravitySettings.previousGravityStrength)
        {
            _gravitySettings.previousGravityStrength = Physics.gravity;
            _gravitySettings.onCustomGravityStrengthChange.Invoke();
        }
    }
}

[System.Serializable]
public class GravityOptions
{
    public bool useCustomGravity;
    public Vector3 customGravityDetails = new Vector3(0,.2f,0);
    [HideInInspector]
    public Vector3 previousGravityStrength;

    [Space(10)]

    public UnityEvent onCustomGravityStrengthChange;
}
