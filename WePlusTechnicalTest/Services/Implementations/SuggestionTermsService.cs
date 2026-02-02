using System;
using System.Collections.Generic;
using System.Text;
using WePlusTechnicalTest.Services.Interfaces;

namespace WePlusTechnicalTest.Services.Implementations;

public class SuggestionTermsService : ISuggestionTermsService
{
    /// <summary>
    /// -- Obtient les suggestions les plus proches du terme donné --
    /// </summary>
    /// <param name="term"></param>
    /// <param name="termes"></param>
    /// <param name="number"></param>
    /// <returns></returns>
    public IEnumerable<string> GetSuggestions(
        string term,
        IEnumerable<string> termes,
        int number)
    {
        if (string.IsNullOrWhiteSpace(term))
            return Enumerable.Empty<string>();

        if (number <= 0)
            return Enumerable.Empty<string>();

        // -- On transforme l'énumérable en liste pour éviter les multiples itérations -- 
        // -- On prépare une liste de tuples (Structure de valeur, allouée sur la pile) -- 
        var scoredResults = new List<(string Original, int Score, int Length)>();

        foreach (var choice in termes)
        {
            if (string.IsNullOrEmpty(choice))
                continue;

            // -- Calcul du score --  
            int score = CalculateSimilarity(term, choice);

            if (score != int.MaxValue)
            {
                // -- On stocke dans le tuples (très léger) -- 
                scoredResults.Add((choice, score, choice.Length));
            }
        }

        // -- Tri final avec LINQ (très efficace sur une liste de structures) -- 
        return scoredResults
            .OrderBy(x => x.Score)
            .ThenBy(x => Math.Abs(x.Length - term.Length))
            .ThenBy(x => x.Original, StringComparer.OrdinalIgnoreCase) // -- Tri alphabétique final -- 
            .Take(number)
            .Select(x => x.Original);
    }

    /// <summary>
    /// -- Calcule la similarité entre la requête et la cible --
    /// </summary>
    /// <param name="query"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    private int CalculateSimilarity(string query, string target)
    {
        // -- Règle de base : la cible doit pouvoir contenir la requête -- 
        if (query.Length > target.Length)
            return int.MaxValue;

        int minDiff = int.MaxValue;
        int queryLen = query.Length;
        int targetLen = target.Length;

        // -- Fenêtre glissante -- 
        for (int i = 0; i <= targetLen - queryLen; i++)
        {
            int currentDiff = 0;
            for (int j = 0; j < queryLen; j++)
            {
                // -- Comparaison directe de caractères -- 
                if (char.ToLowerInvariant(query[j]) != char.ToLowerInvariant(target[i + j]))
                {
                    currentDiff++;
                }

                // -- Si on dépasse déjà le minDiff actuel ==> on arrête -- 
                if (currentDiff >= minDiff)
                    break;
            }

            if (currentDiff < minDiff)
                minDiff = currentDiff;

            // -- Si on trouve une correspondance parfaite ==> on sort -- 
            if (minDiff == 0)
                return 0;
        }

        return minDiff;
    }
}
