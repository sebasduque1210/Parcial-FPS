using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vidajugador : MonoBehaviour
{
    
    public int vidaPlayer = 100;
    
    public void restarvida(int cantidad)
    {
        vidaPlayer -= cantidad;
    }
    
    
}
