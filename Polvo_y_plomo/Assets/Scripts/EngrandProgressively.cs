//---------------------------------------------------------
// Este Componente Cambia la escala del GameObject progresivamente durante un tiempo. 
// Luego, después de un tiempo, puede o no desaparecer.
// Juan José de Reyna Godoy. 
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Este componente, en cada frame, aumenta la escala del GameObject, hasta que pase el tiempo adecuado
/// para que alcance el tamaño objetivo. Luego, si la variable Vanishes es true, después de un
/// tiempo especificado, se elimina el gameobject. Si es false, se elimina este componente, y el tamaño
/// del gameobject se mantiene igual.
/// </summary>
public class EngrandProgressively : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Variable que guarda si desaparece o no.
    /// </summary>
    [SerializeField]
    private bool Vanishes;

    /// <summary>
    /// Tiempo que tarda en escalarse.
    /// </summary>
    [SerializeField]
    private float ChangeTime;

    /// <summary>
    /// Tiempo que permanece en el mundo antes de desvanecerse.
    /// </summary>
    [SerializeField]
    private float VanishTime;

    /// <summary>
    /// Es la escala final a la que se quiere llegar.
    /// </summary>
    [SerializeField]
    private float FinalScale;
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
    /// Cantidad de unidades de escala que se aumenta por unidad de tiempo.
    /// </summary>
    private float _scalePerSecond;

    /// <summary>
    /// Almacena la escala del GameObject en cada frame
    /// </summary>
    private float _actualScale;

    /// <summary>
    /// Almacena la escala inicial en el eje x.
    /// </summary>
    private float _x;

    /// <summary>
    /// Almacena la escala inicial en el eje y.
    /// </summary>
    private float _y;

    /// <summary>
    /// Almacena la escala inicial en el eje z.
    /// </summary>
    private float _z;

    /// <summary>
    /// Almacena el tiempo que ha pasado desde que se activa este componente.
    /// </summary>
    private float _actualTime = 0;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Se llama antes de cualquier Update, y después de activar el componente.
    /// Inicializará la escala del GameObject y velocidad de escala.
    /// </summary>
    void Start()
    {
        _scalePerSecond = FinalScale / ChangeTime; //Inicializa velocidad de escalado.

        _actualScale = 0; //Inicializa la escala aumentada actual.

        VanishTime += ChangeTime; //Tiempo total desde que se activa el componente para que
                                  // se elimine.

        _x = this.gameObject.transform.localScale.x; //Guarda la escala inicial en 3 variables.
        _y = this.gameObject.transform.localScale.y;
        _z = this.gameObject.transform.localScale.z;
    }

    /// <summary>
    /// Se llama cada frame. Tendrá los temporizadores de escalado y de desaparición,
    /// y destruye este componente cuando estos terminan.
    /// </summary>
    void Update()
    {
        _actualTime += Time.deltaTime;
        if (_actualTime < ChangeTime)
        {
            _actualScale = _scalePerSecond * _actualTime;
            this.gameObject.transform.localScale = new Vector3(_x + _actualScale, _y + _actualScale, _z);
        }
        else if (Vanishes)
        {
            if (_actualTime > VanishTime)
            {
                Destroy(this.gameObject);
            }
        }
        else Destroy(this);
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

    #endregion   

} // class EngrandProgressively 
// namespace
