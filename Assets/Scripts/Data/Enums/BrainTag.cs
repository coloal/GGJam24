using System;
using System.Collections.Generic;

public enum BrainTag
{
    Bendecido,
    Maldito,
    Asustado
};

public enum NumericTags
{
    Keys,
    Bombs,
    Potion,
    Maldad
}


public static class StateInfo
{
    public static List<Tuple<string, List<string>>> info = new List<Tuple<string, List<string>>>
        {
            //El primer estado es el valor por defecto
            new Tuple<string, List<string>>("MasterSword", new List<string> { "Oculto", "Recogido", "Roto"}),
            new Tuple<string, List<string>>("Jhon", new List<string> {"Vivo", "Herido", "Muerto", "Atrapado"}),
            new Tuple<string, List<string>>("Zapato", new List<string> {"Suela", "Cordones", "Converse", "Nike", "Adidas"})
        };
}

