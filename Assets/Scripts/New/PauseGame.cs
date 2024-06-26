using UnityEngine;
using UnityEngine.InputSystem;

public class PauseGame : MonoBehaviour
{
    [SerializeField] private InputAction pauseButton;
    [SerializeField] private Canvas canvas;

    private bool paused = false;

    private void OnEnable()
    {
        pauseButton.Enable();
    }

    private void OnDisable()
    {
        pauseButton.Disable();
    }

    private void Start()
    {
        pauseButton.performed += _ => Pause();
    }
    public void Pause()
    {
        paused = !paused;
        if(paused)
        {
            Time.timeScale = 0;
            canvas.enabled = true;
        }
        else
        {
            Time.timeScale = 1;
            canvas.enabled = false;
        }
    }
}
