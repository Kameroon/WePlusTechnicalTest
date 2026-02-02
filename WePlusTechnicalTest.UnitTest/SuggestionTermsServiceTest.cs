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

    [Fact]
    public void GetSuggestion_ShouldReturnSuggestion()
    {
        // -- Arrange -- 
        var list = new List<string> { "gros" };

        // -- Act -- 
        var result = _suggestionService.GetSuggestions("gros", list, 1).ToList();

        // -- Assert -- 
        Assert.Single(result);
    }


    [Fact]
    public void GetSuggestions_ShouldReturnExpectedOriginalResults()
    {
        // -- Arrange -- 
        var list = new List<string> { "gros", "gras", "graisse", "aggressif", "go" };

        // -- Act -- 
        var result = _suggestionService.GetSuggestions("gros", list, 2).ToList();

        // -- Assert -- 
        Assert.Equal(2, result.Count);
        Assert.Equal("gros", result[0]);
        Assert.Equal("gras", result[1]);
    }

    [Fact]
    public void GetSuggestions_ShouldReturnEmpty_WhenNoMatchFound()
    {
        // -- Arrange -- 
        var list = new List<string> { "fr", "go" };

        // -- Act -- 
        var result = _suggestionService.GetSuggestions("gros", list, 3);

        // -- Assert -- 
        Assert.Empty(result);
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


    [Theory]
    // -- Cas standard de l'énoncé -- 
    [InlineData("gros", 3, "gros", "gras", "aggressif")]
    // -- Cas où on demande 0 suggestions -- 
    [InlineData("gros", 0)]
    public void GetSuggestions_VerifyMultipleScenarios(
        string query,
        int n,
        params string[] expected)
    {
        // -- Arrange -- 
        var list = new List<string> { "gros", "gras", "graisse", "aggressif", "go" };

        // -- Act -- 
        var result = _suggestionService.GetSuggestions(query, list, n).ToList();

        // -- Assert -- 
        Assert.Equal(expected.Length, result.Count); // -- Vérifie la taille du retour -- 
        for (int i = 0; i < expected.Length; i++)
        {
            // -- Vérifie chaque mot et son ordre --
            Assert.Equal(expected[i], result[i]);
        }
    }
}
