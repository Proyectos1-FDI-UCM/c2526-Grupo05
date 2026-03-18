//---------------------------------------------------------
// Componente que se añade a un objeto y le da la capacidad de realizar flashes periódicos mientras está activo.
// Ángel Seijas de Ema
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Script que realiza una secuencia configurable de flashes (tiempo por flash, del color del flash al sprite normal del jugador,
/// y cantidad de flashes realizados) de un color que ha de estar configurado en el material del Sprite Renderer. Este material ha de
/// usar el Shader "ColorFlashSpriteShader", si no lo tiene no funcionará.
/// Tiene un método público para iniciar la secuencia. Si se llama en mitad de una secuencia, simplemente se inicia otra desde 0. Como el
/// flash inicia con el color al máximo se representa claramente cada flash, aunque sea iniciado con una interrupción de otra secuencia.
/// 
/// Se inicia automáticamente como "disabled".
/// </summary>
public class CanFlash : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Cuánto tiempo tarda en hacer un flash (color completo -> sprite normal)
    /// </summary>
    [SerializeField]
    private float FlashTime = 0.2f;

    /// <summary>
    /// Cantidad de flashes que se realizarán. Después se desactivará el componente.
    /// </summary>
    [SerializeField]
    private int FlashesUntilDisable = 1;

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
    /// Almacena el último segundo en el que se completó un flash (vuelta a sprite normal).
    /// </summary>
    private float _tLastFlash;

    /// <summary>
    /// Almacena el material del SpriteRenderer.
    /// Este ha de tener el Shader "ColorFlashSpriteShader".
    /// </summary>
    private Material _mat;

    /// <summary>
    /// Almacena cuantos flashes han sido realizados, antes de desactivar el componente.
    /// </summary>
    private int _flashesAmmount;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    private void Awake()
    {
        if (GetComponent<SpriteRenderer>() == null)
        {
            Debug.Log("Se ha puesto el componente \"CanFlash\" en un GameObject sin Sprite Renderer y no funcionará.");
            Destroy(this);
        }
        else
        {
            _mat = GetComponent<SpriteRenderer>().material;
            if (!_mat.HasProperty("_FlashAmount"))
            {
                Debug.Log("Se ha puesto el componente \"CanFlash\" en un GameObject sin el material de flash correcto. No funcionará");
                Destroy(this);
            }
        }

        this.enabled = false; // el componente ha de iniciar desactivado y parpadear solo cuando se le indica.
    }
    /// <summary>
    /// Se llama cada frame si el componente esta activo.
    /// Realiza los flashes indicados por los parámetros.
    /// </summary>
    void Update()
    {
        if (Time.time - _tLastFlash > FlashTime) // 1  flash completo
        {
            _tLastFlash = Time.time;
            _flashesAmmount++;
            if (_flashesAmmount >= FlashesUntilDisable)
            {
                this.enabled = false;
                _mat.SetFloat("_FlashAmount", 0);
            }
        }
        else // hacer el flash
        {
            _mat.SetFloat("_FlashAmount", 1 - (Time.time - _tLastFlash)/FlashTime);
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

    /// <summary>
    /// Inicia la secuencia de flashes indicada por los parámetros.
    /// Puede iniciarse a mitad de otro flash; el color del flash se reinicia a 1.
    /// </summary>
    public void StartFlashes()
    {
        _tLastFlash = Time.time;
        _flashesAmmount = 0;
        this.enabled = true;
    }

    #endregion
    
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion   

} // class CanFlash 
// namespace
