//---------------------------------------------------------
// Archivo que permite a un objeto moverse y rotar con el jugador y su cursor
// Creado por Jorge Ladrón de Guevara Jiménez
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.UIElements;
// Añadir aquí el resto de directivas using


/// <summary>
/// Clase que, guardando el Transform del jugador y de su cursor, calcula la posición relativa del objeto con esta clase.
/// </summary>
public class MoveWithPlayerAndCursor : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    /// <summary>
    /// Bool que determina si el objeto que lleva esta clase es la sombra del ataque melee
    /// </summary>
    [SerializeField] private bool MeleeShadow = false;
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
    /// Transform del jugador
    /// </summary>
    Transform _player = null;
    /// <summary>
    /// Transform del cursor del jugador
    /// </summary>
    Transform _playerCursor = null;
    /// <summary>
    /// Vector dirección (normalizado) hacia el que "mira" el objeto con esta clase
    /// </summary>
    Vector2 _dir = Vector2.zero;
    /// <summary>
    /// Ángulo que determina la rotación del objeto con esta clase
    /// </summary>
    float _angle = 0;
    /// <summary>
    /// Distancia del objeto al jugador
    /// </summary>
    float _distanceToPlayer = 1;
    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    
    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 
    
    /// <summary>
    /// Start de programación defensiva que comprueba si hay LevelManager e InputManager en la escena, además de declarar los Transform del jugador y su cursor.
    /// </summary>
    void Start()
    {
        if (!LevelManager.HasInstance())
        {
            Debug.Log("Se ha puesto el componente \"MoveWithPlayerAndCurosr\" en una escena sin LevelManager. No podrá moverse.");
            Destroy(gameObject);
        }
        else
        {
            _player = LevelManager.Instance.PlayerTransform();
            _playerCursor = _player.gameObject.transform.GetChild(2);
        }

        if (!InputManager.HasInstance())
        {
            Debug.Log("Se ha puesto el componente \"MoveWithPlayerAndCurosr\" en una escena sin InputManager. No podrá desactivarse.");
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Update que calcula la posición y rotación del objeto con esta clase, en función obviamente de jugador y cursor; y además lo destruye cuando le
    /// corresponda.
    /// </summary>
    void Update()
    {
        _dir = (_playerCursor.position - _player.position).normalized;
        _angle = 180f / Mathf.PI * Mathf.Atan2(_dir.y, _dir.x);
        transform.position = (Vector2)_player.position + (_dir.normalized * _distanceToPlayer);
        transform.rotation = Quaternion.Euler(0, 0, _angle);

        if (MeleeShadow && InputManager.Instance.MeleeWasReleasedThisFrame()) Destroy(gameObject);
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController
    /// <summary>
    /// Método público que determina la distancia que mantiene el objeto con esta clase con respecto al jugador.
    /// </summary>
    /// <param name="distanceToPlayer"></param>
    public void InitialDistanceValue(float distanceToPlayer)
    {
        _distanceToPlayer = distanceToPlayer;
    }
    #endregion
    
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion   

} // class MoveWithPlayerAndCursor 
// namespace
