using UnityEngine;
using UnityEngine.InputSystem;

public class ControlsManager : MonoBehaviour
{
    public static ControlsManager instance { get; private set; }

    public Player player { get; private set; }
    public PlayerControls inputActions;

    public Vector2 moveInput { get; private set; }
    public Vector2 lookInput { get; private set; }

    private void Awake()
    {
        instance = this;

        inputActions = new PlayerControls();
    }

    private void Start()
    {
        AssignInputEvents();
    }

    private void Update()
    {
        if (inputActions.Player.enabled)
            SetupLookInput();
    }

    public void Init(Player owner)
    {
        player = owner;
    }

    public void SetupLookInput()
    {
        // Mouse input takes priority over gamepad input for looking direction
        if (Mouse.current != null)
        {
            Vector2 mouseScreen = Mouse.current.position.ReadValue();
            float camZ = Mathf.Abs(Camera.main.transform.position.z);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreen.x, mouseScreen.y, camZ)
);
            lookInput = ((Vector2)(worldPos - player.transform.position)).normalized;
        }
        else
        {
            // Gamepad fallback
            lookInput = inputActions.Player.Look.ReadValue<Vector2>().normalized;
        }
    }

    private void AssignInputEvents()
    {
        // Move input
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        // UI
        inputActions.UI.SwitchToInGame.performed += ctx =>
        {
            if (UI.instance.IsSkillBoardUI())
                return;

            if (UI.instance.IsInGameUI())
            {
                UI.instance.OpenSettings();
                return;
            }

            UI.instance.SwitchToIngameUI();
        };
    }

    // Attack input
    public bool PressedAttack() => inputActions.Player.Attack.WasPressedThisFrame();

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
}
