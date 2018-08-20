using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wiki.Core.Domain;
using Wiki.Core.Repositories;

namespace Wiki.Infrastructure.Services
{
    public class SuggestionService : ISuggestionService
    {
        private readonly ISuggestionRepository suggestionRepository;

        public SuggestionService(ISuggestionRepository suggestionRepository)
        {
            this.suggestionRepository = suggestionRepository;
        }

        public async Task AddAsync(int? authorId, int? textId, string content)
        {
            var suggestion = new Suggestion(authorId, textId, content);
            await suggestionRepository.AddAsync(suggestion);
        }
    }
}
