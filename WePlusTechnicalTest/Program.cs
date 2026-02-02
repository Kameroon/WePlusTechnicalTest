using WePlusTechnicalTest.Services.Implementations;
using WePlusTechnicalTest.Services.Interfaces;

string termeRecherche = "gros";
var dictionnaire = new List<string> { "gros", "gras", "graisse", "aggressif", "go", "ros", "gro" };
int nombreDeSuggestions = 2;

// -- Création de l'instance du suggester -- 
ISuggestionTermsService suggester = new SuggestionTermsService();

Console.WriteLine($"--- Test de suggestion pour : '{termeRecherche}' ---");

// -- Obtention des suggestions --
var resultats = suggester.GetSuggestions(
    termeRecherche,
    dictionnaire,
    nombreDeSuggestions)
    .ToList();

// -- Affichage des résultats --
if (resultats.Count == 0)
{
    Console.WriteLine("Aucune suggestion trouvée.");
}
else
{
    for (int i = 0; i < resultats.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {resultats[i]}");
    }
}