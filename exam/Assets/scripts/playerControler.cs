using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class playerControler : MonoBehaviour
{
    Rigidbody prota;
    float horizontal;
    float vertical;
    bool saltar;
    bool unSalto;
    bool dosSaltos;
    RaycastHit rayo;

    public float velocidad;
    public float fuerzaSalto;
    public LayerMask layer;
    public GameObject perdiste;
    public GameObject ganaste;
    SpriteRenderer Renderer;
    public static event Action actualizarVida;
    public static event Action quitarVida;
    public static event Action actualizarPuntos;
    public static event Action ganar;

    protected virtual void Ganaste()
    {
        ganar?.Invoke();
    }
    protected virtual void ActualizarPuntos()
    {
        actualizarPuntos?.Invoke();
    }
    protected virtual void ActualizarVida()
    {
        actualizarVida?.Invoke();
    }
    protected virtual void QuitarVida()
    {
        quitarVida?.Invoke();
    }
    private void Awake()
    {
        prota = GetComponent<Rigidbody>();
        Renderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        TiempoDelJuego(1);
    }
    private void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            saltar = true;
        }
    }
    private void FixedUpdate()
    {
        prota.velocity = new Vector3(horizontal * velocidad, prota.velocity.y, vertical * velocidad);
        CheckRaycast();
        if (saltar == true)
        {
            if (unSalto == true)
            {
                prota.AddForce(new Vector3(0, fuerzaSalto), ForceMode.Impulse);
                saltar = false;
            }
            else if (dosSaltos == true)
            {
                prota.AddForce(new Vector3(0, fuerzaSalto), ForceMode.Impulse);
                dosSaltos = false;
            }
        }
    }
    private void CheckRaycast()
    {
        //rayo = Physics.Raycast(transform.position, Vector3.down, 1.03f, layer);
        if (rayo.collider != null)
        {
            unSalto = true;
            dosSaltos = true;
        }
        else
        {
            unSalto = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "enemy")
        {
            if (Renderer.color != collision.gameObject.GetComponent<SpriteRenderer>().color)
            {
                QuitarVida();
            }

        }
        if (collision.gameObject.tag == "finish")
        {
            Ganaste();
        }
    }
    public void TiempoDelJuego(int a)
    {
        Time.timeScale = a;
    }
    private void Perdiste()
    {
        perdiste.SetActive(true);
        TiempoDelJuego(0);
    }
    private void OnEnable()
    {
        gameManager.perdiste += Perdiste;
        ganar += Ganar;
    }
    private void OnDisable()
    {
        gameManager.perdiste -= Perdiste;
        ganar -= Ganar;
    }
    private void Ganar()
    {
        ganaste.SetActive(true);
        TiempoDelJuego(0);
    }
    public void ReadDirection(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<float>();
    }
    public void ReadJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            saltar = true;
        }
    }

}
