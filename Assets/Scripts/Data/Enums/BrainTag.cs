using System;
using System.Collections.Generic;

//Toma valores bool, por defecto es false
public enum BrainTag
{
    Alarm,
    HasPlayedAgainstPunkieBoy,
    TruckDriverFavor,
    HasGoneThroughTownSquare,
    HasGoneThroughGasStation,
    GotDrivingLicense,
    GotVanFixed,
    IsBabyBoyWithYou,
    Mobilephone,
    Bendecido,
    Maldito,
    Asustado,
    Combate
};

//Toma valores int, por defecto es 0, no se puede decrementar por debajo de 0
public enum NumericTags
{
    testViolence,
    testMoney,
    testInfluence,
    Keys,
    Bombs,
    Potion,
    Maldad, 
    ConverBabyBoy,
    ConverSmokieGirl,
    ConverWheelchaired,
    MOOD
}


public static class StateInfo
{
    //Para un Tag toma una lista de valores
    public static List<Tuple<string, List<string>>> info = new List<Tuple<string, List<string>>>
        {
            //El primer estado es el valor por defecto
            new Tuple<string, List<string>>("MasterSword", new List<string> { "Oculto", "Recogido", "Roto"}),
            new Tuple<string, List<string>>("Jhon", new List<string> {"Vivo", "Herido", "Muerto", "Atrapado"}),
            new Tuple<string, List<string>>("Zapato", new List<string> {"Suela", "Cordones", "Converse", "Nike", "Adidas"})
        };
}

public enum BrainTagType
{
    Bool,
    Numeric, 
    State,
}