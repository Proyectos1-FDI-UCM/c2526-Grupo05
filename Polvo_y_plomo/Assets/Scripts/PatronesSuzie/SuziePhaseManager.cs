//---------------------------------------------------------
// Gestor central de las fases y bucles de ataque de Suzie.
// Samuel Asensio Torres
// Polvo y plomo
// Proyectos 1 - Curso 2025-26
//---------------------------------------------------------

using UnityEditor.ShaderGraph;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Máquina de estados que controla el orden de ejecución de los patrones de Suzie.
/// Fase 1: Patrón 1 -> Patrón 1 -> Patrón 2
/// Fase 2 (Mitad de vida): Patrón 3 -> Patrón 1 -> Patrón 1 -> Patrón 2
/// </summary>
public class SuziePhaseManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    [SerializeField]
    private HealthChanger _healthManager;

    [SerializeField]
    private SuziesFirstPattern _patron1;

    [SerializeField]
    private SuziesSecondPattern _patron2;

    [SerializeField]
    private SuziesThirdPattern _patron3;

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
    /// Indica si estamos en la fase 1 o 2
    /// </summary>
    private int _faseActual = 1;

    /// <summary>
    /// Controla por qué paso del bucle vamos
    /// </summary>
    private int _indiceSecuencia = 0;

    /// <summary>
    /// Está atacando?
    /// </summary>
    private bool _atacando = false;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Desactivamos los patrones antes de empezar
    /// para que no actuen por libre
    /// </summary>
    void Start()
    {
        if (_patron1 != null) _patron1.enabled = false;
        if (_patron2 != null) _patron2.enabled = false;
        if (_patron3 != null) _patron3.enabled = false;

        SiguienteAtaque();
    }

    /// <summary>
    /// Comprobamos el cambio de fase (Si llega a 10 de vida o menos, pasa a fase 2)
    /// </summary>
    void Update()
    {
        if (_faseActual == 1 && _healthManager != null && _healthManager.GetCurrentHealth() <= 10)
        {
            CambiarAFase2();
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
    /// Los scripts de los patrones deben llamar a este método cuando terminen su secuencia
    /// para que el Manager dé paso al siguiente ataque.
    /// </summary>
    public void ReportarAtaqueTerminado()
    {
        _atacando = false;
        _indiceSecuencia++;
        SiguienteAtaque();
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    /// <summary>
    /// Método privado que se encarga del orden de patrones
    /// </summary>
    private void SiguienteAtaque()
    {
        if (_atacando) return;

        _atacando = true;

        if (_faseActual == 1)
        {
            if (_indiceSecuencia > 2) _indiceSecuencia = 0; // Reiniciar bucle

            if (_indiceSecuencia == 0 || _indiceSecuencia == 1) EjecutarPatron1();
            else if (_indiceSecuencia == 2) EjecutarPatron2();
        }
        else if (_faseActual == 2)
        {
            if (_indiceSecuencia > 3) _indiceSecuencia = 0; // Reiniciar bucle

            if (_indiceSecuencia == 0) EjecutarPatron3();
            else if (_indiceSecuencia == 1 || _indiceSecuencia == 2) EjecutarPatron1();
            else if (_indiceSecuencia == 3) EjecutarPatron2();
        }
    }

    /// <summary>
    /// Método privado que cambia la fase 1 a la 2
    /// </summary>
    private void CambiarAFase2()
    {
        Debug.Log("Suzie entra en Fase 2");
        _faseActual = 2;
        _indiceSecuencia = 0; // Reseteamos la secuencia para empezar con el Patrón 3
        _atacando = false; // Forzamos el estado por si estaba a mitad de un ataque

        // Detenemos los patrones actuales
        _patron1.enabled = false;
        _patron2.enabled = false;

        SiguienteAtaque(); // Lanzamos el patrón 3 inmediatamente
    }

    /// <summary>
    /// Método privado que da paso al patrón 1
    /// </summary>
    private void EjecutarPatron1()
    {
        _patron2.enabled = false;
        _patron3.enabled = false;
        _patron1.enabled = true; 
    }

    /// <summary>
    /// Método privado que da paso al patrón 2
    /// </summary>
    private void EjecutarPatron2()
    {
        _patron1.enabled = false;
        _patron3.enabled = false;
        _patron2.enabled = true;
        _patron2.IniciarPatron(); 
    }

    /// <summary>
    /// Método privado que da paso al patrón 3
    /// </summary>
    private void EjecutarPatron3()
    {
        _patron1.enabled = false;
        _patron2.enabled = false;
        _patron3.enabled = true;
        _patron3.IniciarPatron();
    }

    #endregion

} // class SuziePhaseManager 
// namespace
