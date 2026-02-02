using System;
using System.Collections.Generic;
using System.Text;

namespace WePlusTechnicalTest.Services.Interfaces;

public interface ISuggestionTermsService
{
    IEnumerable<string> GetSuggestions(string term, IEnumerable<string> terms, int number);
}
