using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Character))]
public class PlayerInputHandler : MonoBehaviour
{
    private Character character;

    private Vector2 moveValue;

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    private void Update()
    {
        // x rotation, y forward/backward
        character.SetMovementInput(moveValue.y, moveValue.x);
    }

    // Input System callbacks
    public void OnMove(InputAction.CallbackContext context)
    {
        moveValue = context.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed) // bouton enfoncé
            character.OnAttackPressed();
        else if (context.canceled) // bouton relâché
            character.OnAttackReleased();
    }

    public void OnCycleElement(InputAction.CallbackContext context)
    {
        if (context.performed && character is Mage mage)
            mage.CycleElement();
    }
}
