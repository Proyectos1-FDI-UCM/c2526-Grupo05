//---------------------------------------------------------
// Copia del PlayerGetShootingInput pero para disparar con la escopeta
// Ángel Seijas de Ema
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Este componente solo servirá para testear la escopeta. Es casi igual al PlayerGetShootingInput pero
/// en vez de buscar el componente Shoot busca el componente ShootEscopeta. No usa HasAmmo.
/// </summary>
public class PlayerGetShootingInputEscopeta : MonoBehaviour
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

    /// <summary>
    /// Variable que almacena el componente de disparo de escopeta.
    /// </summary>
    private ShootEscopeta _escopetafire;
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
    /// Ha de colocarse en un objeto con padre (y este deberá ser el jugador para que funcione bien)
    /// </summary>
    private void Start()
    {
        if (!InputManager.HasInstance())
        {
            Debug.Log("Se ha puesto el componente \"PlayerGetShootingInputEscopeta\" en una escena sin InputManager. No podrá disparar.");
            Destroy(this);
        }

        _escopetafire = GetComponent<ShootEscopeta>();
        if (_escopetafire == null)
        {
            Debug.Log("Se ha puesto el componente \"PlayerGetShootingInputEscopeta\" en un gameobject sin ShootEscopeta. No podrá disparar");
            Destroy(this);
        }

        if (Cursor == null)
        {
            Debug.Log("Se ha puesto el componente  \"PlayerGetShootingInputEscopeta\" sin asignarle objeto Cursor. No podrá disparar");
            Destroy(this);
        }
    }

    /// <summary>
    /// Si el componente es destruido por no poder funcionar, se asegura que el resto de componentes dejen de funcionar también.
    /// </summary>
    private void OnDestroy()
    {
        Destroy(GetComponent<ShootEscopeta>());
    }

    /// <summary>
    /// Se llama cada frame.
    /// Revisa si el jugador esta intentando disparar (si es posible) o recargar.
    /// </summary>
    void Update()
    {
        CompruebaDisparo();
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
            _escopetafire.ShootBullet(fireDir);
        }
    }


    #endregion   

} // class Player 
// namespace
