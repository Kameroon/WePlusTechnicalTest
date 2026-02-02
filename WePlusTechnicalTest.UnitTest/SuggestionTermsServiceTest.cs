using System;
using System.Collections.Generic;
using System.Text;
using WePlusTechnicalTest.Services.Implementations;
using WePlusTechnicalTest.Services.Interfaces;

namespace WePlusTechnicalTest.UnitTest;

public class SuggestionTermsServiceTest
{
    private readonly ISuggestionTermsService _suggestionService;

    public SuggestionTermsServiceTest()
    {
        _suggestionService = new SuggestionTermsService();
    }

    [Theory]
    // -- Correspondance exacte (Ton premier test) -- 
    [InlineData("gros", new[] { "gros" }, 1, new[] { "gros" })]
    // -- Pas de correspondance (Mot trop court - Ton deuxième test) -- 
    [InlineData("gros", new[] { "fr" }, 1, new string[0])]
    // -- Exemple complet de l'énoncé (Pour prouver que l'algo marche) -- 
    [InlineData("gros", new[] { "gros", "gras", "graisse", "aggressif", "go" }, 2, new[] { "gros", "gras" })]
    // -- Vérification de la limite N (numberOfSuggestions) -- 
    [InlineData("gros", new[] { "gros", "gras" }, 1, new[] { "gros" })]
    // -- Vérification de l'ordre alphabétique en cas d'égalité --
    [InlineData("test", new[] { "testz", "testa" }, 2, new[] { "testa", "testz" })]
    public void GetSuggestions_ShouldWorkForVariousScenarios(
        string query,           // -- Le terme recherché -- 
        string[] dictionary,    // -- La liste de mots en entrée -- 
        int n,                  // -- Le nombre de suggestions demandées -- 
        string[] expected)      // -- Le résultat attendu
    {
        // -- Arrange -- 
        var choices = dictionary.ToList();

        // -- Act -- 
        var result = _suggestionService.GetSuggestions(query, choices, n).ToList();

        // -- Assert --
        Assert.Equal(expected.Length, result.Count);

        // -- On vérifie que le contenu et l'ordre sont identiques -- 
        Assert.Equal(expected, result);
    }

}
