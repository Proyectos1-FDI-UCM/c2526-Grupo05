//---------------------------------------------------------
// Hace rotar este objeto hacia otro para que lo "mire".
// Ángel Seijas de Ema
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Componente que hace rotar a un objeto frente a otro asignable, para que lo mire.
/// Contiene un parámetro de Offset de rotación y la posibilidad de invertir el eje Y según la rotación para que esta se vea bien
/// </summary>
public class rotateTowardsObject : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// El objeto en el que esta este script se rotará mirando hacia donde este el transform del objeto asignado
    /// </summary>
    [SerializeField]
    private Transform Object;

    /// <summary>
    /// Determina si el objeto se dará la vuelta al hacer el cambio de mirar de "izquierda a derecha".
    /// Por ejemplo, un arma que apunta a la derecha, si se rotase normal 180º acabaría invertida en el eje Y.
    /// Este cambio invierte el eje Y del objeto que se rota en el momento adecuado para que se vea bien.
    /// </summary>
    [SerializeField]
    private bool FlipY = true;

    /// <summary>
    /// Permite darle un offset a la rotación frente al objeto.
    /// 0º es que lo mire directamente. 5º es que lo mire, en sentido antihorario, 5º mas.
    /// </summary>
    [SerializeField]
    private float RotationOffset = 0f;

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
    /// Guarda la escala original del objeto si "FlipY" esta activo, para invertir respecto a esta escala cuando sea necesario.
    /// </summary>
    private Vector3 _originalScale;

    /// <summary>
    /// Registra si el objeto esta invertido o no, para evitar estar cambiando el parámetro de escala innecesariamente.
    /// </summary>
    private bool _flipped = false;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    /// <summary>
    /// Se llama al ser cargado, una vez.
    /// Hace comprobaciones necesarias para el componente.
    /// También guarda la escala original del objeto.
    /// </summary>
    private void Awake()
    {
        if (Object == null)
        {
            Debug.Log("A un objeto con el componente \"rotateTowardsObject\" no se le ha puesto el transform de un objeto hacia el que rotar. No podrá hacerlo.");
            Destroy(this);
        }

        _originalScale = transform.localScale;
    }

    /// <summary>
    /// Se llama cada frame
    /// Calcula el ángulo de rotación necesario para "mirar" al objeto asignado.
    /// Si FlipY esta activo, comprueba el ángulo para ver si es necesario invertir el eje Y.
    /// Cuando lo es, lo invierte si no lo estaba. Y cuando no lo es, comprueba si esta invertido y si es así, lo pone normal.
    /// </summary>
    void Update()
    {
        // Cálculo del angulo
        float angulo = 180f / Mathf.PI * Mathf.Atan2((Object.position - transform.position).y, (Object.position - transform.position).x) + RotationOffset;
        // Normalización a angulos de 0 a 360
        angulo %= 360;
        if (angulo < 0) angulo += 360f;

        transform.rotation = Quaternion.Euler(0, 0, angulo);
        if (FlipY)
        {
            if (angulo >= 90f && angulo <= 270f)
            {
                if (!_flipped)
                {
                    transform.localScale = new Vector3(_originalScale.x, _originalScale.y * -1, _originalScale.z);
                    _flipped = true;
                }
            }
            else if (_flipped)
            {
                transform.localScale = _originalScale;
                _flipped = false;
            }
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

} // class rotateTowardsObject 
// namespace
