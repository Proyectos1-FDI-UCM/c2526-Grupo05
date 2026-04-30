//---------------------------------------------------------
// Breve descripción del contenido del archivo
// CamiloSandovalSánchez
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class PlayerAnimations : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Transform asignable desde el editor que guarda el arma del jugador
    /// </summary>
    [SerializeField]
    private Transform RevolverRotation = null;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    /// <summary>
    /// Animator del jugador
    /// </summary>
    private Animator _playerAnimator = null;
    /// <summary>
    /// playerRoll del jugador
    /// </summary>
    private playerRoll _playerRoll = null;
    /// <summary>
    /// Rigidbody2D del jugador
    /// </summary>
    private Rigidbody2D _rb = null;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 
    /// <summary>
    /// Método Awake() de programación defensiva, revisando cosas necesarias para el componente.
    /// </summary>
    private void Awake()
    {
        _playerAnimator = GetComponent<Animator>();
        if (_playerAnimator == null)
        {
            Debug.Log("Se ha puesto el componente  \"PlayerAnimator\" en un objeto sin el componente \"Animator\", y no se podrá animar.");
            Destroy(this);
        }
        _playerRoll = GetComponent<playerRoll>();
        if (_playerRoll == null)
        {
            Debug.Log("Se ha puesto el componente  \"PlayerAnimator\" en un objeto sin el componente \"playerRoll\", y no se podrá animar.");
            Destroy(this);
        }
        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null)
        {
            Debug.Log("Se ha puesto el componente  \"PlayerAnimator\" en un objeto sin el componente \"Rigidbody2D\", y no se podrá animar.");
            Destroy(this);
        }
    }
    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        float playerAngle = RevolverRotation.localEulerAngles.z;
        playerAngle += 45f;
        float posAngle = Mathf.Abs((int)(playerAngle) / 90);
        float negAngle = Mathf.Abs((int)(360f + playerAngle) / 90);
        if (!_playerRoll.GetRoll())
        {
            _playerAnimator.SetInteger("IsRolling", 0);
            if (_rb.linearVelocity.magnitude != 0f)
            {
                if (playerAngle >= 0) _playerAnimator.Play(("Walk" + (posAngle + 1)));
                else _playerAnimator.Play(("Walk" + (negAngle + 1)));
            }
            else
            {
                if (playerAngle >= 0) _playerAnimator.Play(("Idle" + (posAngle + 1)));
                else _playerAnimator.Play(("Idle" + (negAngle + 1)));
            }
            //_playerAnimator.enabled = false;
            //_playerAnimator.enabled = true;
            /*if (playerAngle >= 0) _playerAnimator.Play(("Roll" + (posAngle + 1)), 0);
            else _playerAnimator.Play(("Roll" + (negAngle + 1)), 0);*/
        }
        else
        {
            if (playerAngle >= 0) _playerAnimator.SetInteger("IsRolling", (int)posAngle);
            else _playerAnimator.SetInteger("IsRolling", (int)negAngle);
        }
        Debug.Log("Roll" + _playerAnimator.GetInteger("IsRolling"));
        //Debug.Log("angulo" + (int)(RevolverRotation.eulerAngles.z - 225f) / 45);
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS s
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    /*
    public float AngleRounding(float angle)
    {
        return 
    }*/
    #endregion

} // class PlayerAnimations 
// namespace
