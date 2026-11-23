using System.ComponentModel;

namespace Prompts;

public static class CatAverageWeight
{
    [Description("Gets the cat average weight by species.")]
    public static string ByWeight(
        [Description("The cat species")] string catSpecies)
    {
        switch (catSpecies)
        {
            case "leopard":
                return "50 kg";
            case "European cat":
            default:
                return "5 kg";
        }
    }
}