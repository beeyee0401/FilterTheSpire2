using FilterTheSpire2.FilterTheSpire2Code.Ancients.Config;
using MegaCrit.Sts2.Core.Models;

namespace FilterTheSpire2.FilterTheSpire2Code.Ancients.Filtering;

public static class AncientFactory
{
    /// <summary>
    /// Gets the correct Ancient and uses their GenerateInitialOptions logic
    /// </summary>
    /// <param name="ancient"></param>
    /// <param name="actNum">This is used for the multi-act ancients, such as Darv</param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public static AbstractAncient? GetAncient(Ancient ancient, int actNum)
    {
        return ancient switch
        {
            Ancient.Orobas => new Orobas(),
            Ancient.Pael => new Pael(),
            Ancient.Tezcatara => new Tezcatara(),
            Ancient.Darv => new Darv(actNum),
            Ancient.Nonupeipe => new Nonupeipe(),
            Ancient.Tanx => new Tanx(),
            Ancient.Vakuu => new Vakuu(),
            Ancient.Any => null,
            _ => throw new NotSupportedException("Unsupported Ancient type")
        };
    }
}