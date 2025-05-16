using UnityEngine;
using System.Collections.Generic;
public enum TipoDeAtaque { Ligero, Pesado }

[System.Serializable]
public class Attack
{
    public string nombre;
    public TipoDeAtaque tipo;
    public AnimationClip animacion;
    public float tiempoParaCombo = 0.5f;

    [Tooltip("Ataques que pueden venir justo después de este")]
    public List<string> ataquesSiguientes;

    [Tooltip("Ataques que deben haberse hecho antes para permitir este")]
    public List<string> ataquesRequeridos;
}