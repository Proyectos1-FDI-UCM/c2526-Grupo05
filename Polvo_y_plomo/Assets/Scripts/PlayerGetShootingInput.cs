//---------------------------------------------------------
// Este componente se encargará de recibir el input de disparo del InpuManager y llamará a una función para disparar una bala.
// Juan Jsé de Reyna Godoy
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Este componente recibirá el valor del input booleano _fire del InputManager. Cuando se reciba el input, llamará a otra función de un componente llamado Shoot, que generará una bala en 
/// la dirección que esta clase le introduzca.
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

    // GameObject asignable desde el editor que guarda el cursor del jugador

    // Variable float que guarda el cooldown (CD) del disparo. Actualizada en el Update cada vez que este se realiza.
    [SerializeField]
    private float Rate = 0.15f;

    // Variable float que guarda el tiempo de recarga del disparo. Actualizada en el Update cada vez que este se realiza.
    [SerializeField]
    private float Reload = 0.3f;

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

    /// <summary>
    /// Esta variable almacena el último momento en el que se presionó la acción de recargar.
    /// </summary>
    private float _tiempoDesdeUltimaRecarga = -99f;

    ///<summary>
    ///Esta variable almacena el componente de tipo Shoot (encargado de disparar) que tiene este GameObject;
    ///</summary>>
    private Shoot _shoot = null;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Se llama una vez, en el primer frame.
    /// Se harán comprobaciones de que el editor está correctamente colocado.
    /// </summary>
    void Start()
    {
        if (!InputManager.HasInstance())
        {
            Debug.Log("Se ha puesto el componente \"PlayerGetShootingInput\" en una escena sin InputManager. No podrá disparar.");
            Destroy(this);
        }

        _shoot = GetComponent<Shoot>();
        if (_shoot == null)
        {
            Debug.Log("Se ha puesto el componente  \"PlayerGetShootingInput\" en un objeto sin el componente \"Shoot\", y no disparar.");
            Destroy(this);
        }
    }


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

    private void CompruebaDisparo()
    {
        if (InputManager.Instance.FireWasReleasedThisFrame() && Time.time - _tiempoDesdeUltimoDisparo > Rate)
        {
            _shoot.ShootBullet();
            _tiempoDesdeUltimoDisparo = Time.time;
        }
    }

    private void CompruebaRecarga()
    {
        if (InputManager.Instance.ReloadWasReleasedThisFrame() && Time.time - _tiempoDesdeUltimaRecarga > Reload)
        {
            //_shoot.;
            _tiempoDesdeUltimaRecarga = Time.time;
        }
    }

    #endregion   

} // class Player 
// namespace
