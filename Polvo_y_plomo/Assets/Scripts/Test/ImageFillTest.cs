//---------------------------------------------------------
// Script que usa ImageFill para comprobar que funciona
// Ángel Seijas de Ema
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Cambia usando un componente ImageFill en el mismo gameobject el fillAmmount de 
/// una imagen de tipo Filled. Va ciclando entre hacer que baje a 0 y que suba a 1.
/// Se puede asignar que tan rápido ocurre cada ciclo.
/// </summary>
public class ImageFillTest : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Segundos que se tarda en realizar un ciclo de llenado / vaciado de la imagen.
    /// </summary>
    [SerializeField]
    private float CycleTime = 2f;

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
    /// fillAmmount inicial a la que se va a ciclar actualmente en el Update().
    /// </summary>
    private float _initTarget = 0;

    /// <summary>
    /// fillAmmount final a la que se va a ciclar actualmente en el Update().
    /// </summary>
    private float _finalTarget = 1;

    /// <summary>
    /// Almacena el ImageFill. Inicializado en el Awake().
    /// </summary>
    private ImageFill _imageFill;

    /// <summary>
    /// Almacena el momento en el que empieza un ciclo.
    /// Inicializado en el Awake().
    /// </summary>
    private float _t;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    private void Awake()
    {
        _imageFill = GetComponent<ImageFill>();
        if (_imageFill == null)
        {
            Debug.Log("Se ha puesto ImageFillTest en un GameObject sin \"ImageFill\". No funcionará.");
            Destroy(this);
        }

        _t = Time.time;
    }

    /// <summary>
    /// Se llama cada frame si el componente esta activo.
    /// Cicla entre hacer disminuir y aumentar.
    /// </summary>
    void Update()
    {
        if (Time.time - _t >= CycleTime)
        {
            _imageFill.UpdateImageFillAmmount(_finalTarget);
            _t = Time.time;
            (_initTarget, _finalTarget) = (_finalTarget, _initTarget);
        }
        else
        {
            _imageFill.UpdateImageFillAmmount(_initTarget + (_finalTarget - _initTarget) * ((Time.time - _t) / CycleTime));
        }
        
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

} // class ImageFillTest 
// namespace
