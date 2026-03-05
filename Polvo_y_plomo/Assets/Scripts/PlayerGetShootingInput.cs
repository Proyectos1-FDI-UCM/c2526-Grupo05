//---------------------------------------------------------
// Controlador para el disparo del jugador.
// Juan José de Reyna Godoy
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Este componente recibirá el valor del input booleano _fire del InputManager. Cuando se reciba el input,
/// llamará al componente HasAmmo del jugador y este se encargará del resto del disparo.
/// Este componerse ha de situarse en el mismo GameObject junto a "HasAmmo" y "Shoot" para poder funcionar.
/// </summary>
public class PlayerGetShootingInput : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints


    /// <summary>
    /// Variable float que guarda el cooldown (CD) del disparo.
    /// </summary>
    [SerializeField]
    private float Rate = 0.15f;

    /// <summary>
    /// Transform del cursor, hacia el que se disparará la bala.
    /// </summary>
    [SerializeField]
    private Transform Cursor;

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
    /// Esta variable almacena el último momento en el que se presionó la acción de disparar.
    /// </summary>
    private float _tiempoDesdeUltimoDisparo = -99f;


    ///<summary>
    /// Esta variable almacena el componente de tipo HasAmmo (encargado de disparar en el jugador) que tiene este GameObject;
    ///</summary>>
    private HasAmmo _hasAmmo;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Se llama una vez si el componente esta activo al cargase en escena, después del Awake().
    /// Se harán comprobaciones necesarias para el componente. Se tiene que hacer después del Awake() ya
    /// que los componentes HasAmmo o Shoot podrían destruirse en el Awake() si falta algún componente.
    /// </summary>
    private void Start()
    {
        if (!InputManager.HasInstance())
        {
            Debug.Log("Se ha puesto el componente \"PlayerGetShootingInput\" en una escena sin InputManager. No podrá disparar.");
            Destroy(this);
        }

        _hasAmmo = GetComponent<HasAmmo>();
        if (_hasAmmo == null)
        {
            Debug.Log("Se ha puesto el componente  \"PlayerGetShootingInput\" en un objeto sin el componente \"HasAmmo\", y no podrá disparar.");
            Destroy(this);
        }

        if (Cursor == null)
        {
            Debug.Log("Se ha puesto el componente  \"PlayerGetShootingInput\" sin asignarle objeto Cursor. No podrá disparar");
            Destroy(this);
        }
    }

    /// <summary>
    /// Si el componente es destruido por no poder funcionar, se asegura que el resto de componentes dejen de funcionar también.
    /// </summary>
    private void OnDestroy()
    {
        Destroy(GetComponent<HasAmmo>());
        Destroy(GetComponent<Shoot>());
    }

    /// <summary>
    /// Se llama cada frame.
    /// Revisa si el jugador esta intentando disparar (si es posible) o recargar.
    /// </summary>
    void Update()
    {
        CompruebaDisparo();
        CompruebaRecarga();
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
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

    /// <summary>
    /// Revisa si el jugador intenta disparar y si ha pasado suficiente tiempo como
    /// para que pueda hacer, según la cadencia del arma.
    /// Si es posible, llama a _hasAmmo para que se encargue del resto del intento del disparo.
    /// Si se dispara con éxito se actualiza _tiempoDesdeUltimoDisparo.
    /// </summary>
    private void CompruebaDisparo()
    {
        if (InputManager.Instance.FireWasPressedThisFrame() && Time.time - _tiempoDesdeUltimoDisparo > Rate)
        {
            // Calculo de la dirección de disparo.
            Vector2 fireDir;
            fireDir.x = Cursor.position.x - transform.parent.position.x;
            fireDir.y = Cursor.transform.position.y - transform.parent.position.y;

            // Disparo
            if (_hasAmmo.IntentaDisparo(fireDir)) _tiempoDesdeUltimoDisparo = Time.time;
        }
    }

    /// <summary>
    /// Revisa si el jugador esta intentando recargar y manda una señal a
    /// _hasAmmo para que intente empezar a recargar.
    /// </summary>
    private void CompruebaRecarga()
    {
        if (InputManager.Instance.ReloadWasPressedThisFrame())
        {
            _hasAmmo.IntentaRecarga();
        }
    }

    #endregion   

} // class Player 
// namespace
