//---------------------------------------------------------
// Este componente moverá el propio GameObject al que pertenece a una posición determinada en un tiempo
// determinado, y cuando llegue, se desactivará y activará otro componente de tipo Explode.
// Juan José de Reyna Godoy
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Este componente, durante su FixedUpdate, desplaza el GameObject a una posición dada desde el editor
/// a una velocidad tal que tarde en llegar a su destino un tiempo dado desde el editor.
/// En su Update, una vez haya pasado este tiempo, activa el componente de tipo Explode, y desactiva este.
/// </summary>
public class MoveToCoordsAndExplode : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Es el tiempo que tardará en desplazarse a la posición.
    /// </summary>
    [SerializeField]
    private float MovingTime;

    /// <summary>
    /// Es la posición a la que se desplazará.
    /// </summary>
    [SerializeField]
    private Vector3 Pos;
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
    /// Es el vector dirección que se desplazará el GameObject cada unidad de tiempo.
    /// </summary>
    private Vector3 _vel;

    /// <summary>
    /// Componente de parpadear que se activa al desactivarse este.
    /// </summary>
    private CanFlash _flash;

    /// <summary>
    /// Componente a activar.
    /// </summary>
    private Explode _exp;



    private float _timer = 0f;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    /// <summary>
    /// Se llama al iniciar la escena. Se harán comprobaciones, y se incializarán variables.
    /// </summary>
    void Start()
    {
        _flash = GetComponent<CanFlash>();
        _exp = GetComponent<Explode>();

        _exp.enabled = false;//Desactiva el componente,en caso de que estuviese activado.

        _vel = (Pos - transform.position) / MovingTime; //Inicializar la velocidad de movimiento.

        if (GetComponent<Explode>() == null)
        {
            Debug.Log("Se ha puesto un componente MoveToCoordsAndExplode sin un componente de tipo Explode. Se eliminará  este componente.");
            Destroy(this);
        }
    }

    /// <summary>
    /// Se llama varias veces en un solo frame. Realizará el movimiento.
    /// </summary>
    void FixedUpdate()
    {
        if (GameManager.HasInstance()) transform.Translate(_vel * Time.fixedDeltaTime * GameManager.SlowMultiplier);
        else transform.Translate(_vel * Time.fixedDeltaTime);
    }

    private void Update()
    {


        if (GameManager.HasInstance()) _timer += Time.deltaTime * GameManager.SlowMultiplier;
        else _timer += Time.deltaTime;


        if (_timer > MovingTime)
        {
            _flash.StartFlashes();
            _exp.enabled = true;
            this.enabled = false;
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
    /// Cambia el valor de Pos (que mide el cambio de posición local de este GameObject)
    /// por la diferencia entre la posición final global y la posición global de este objeto.
    /// </summary>
    /// <param name="v"></param>
    public void SetFinalPosition(Vector3 posGlob)
    {
        Pos = posGlob;
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class MoveToCoordsAndActivateComponent 
// namespace
