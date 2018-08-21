using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wiki.Core.Domain;
using Wiki.Core.Repositories;
using Wiki.Infrastructure.DTO;

namespace Wiki.Infrastructure.Services
{
    public class SuggestionService : ISuggestionService
    {
        private readonly ISuggestionRepository suggestionRepository;
        private readonly IMapper mapper;

        public SuggestionService(ISuggestionRepository suggestionRepository, IMapper mapper)
        {
            this.suggestionRepository = suggestionRepository;
            this.mapper = mapper;
        }

        public async Task AddAsync(int? authorId, int? textId, string content)
        {
            var suggestion = new Suggestion(content, 0);
            if(authorId != null)
                suggestion.SetAuthor(new User((int)authorId));
            if (textId != null)
                suggestion.SetText(new Text((int)textId));

            await suggestionRepository.AddAsync(suggestion);
        }

        public async Task<IEnumerable<SuggestionDto>> BrowseAsync()
        {
            var suggestions = await suggestionRepository.GetAllAsync();
            return mapper.Map<IEnumerable<SuggestionDto>>(suggestions);
        }

        public async Task<SuggestionDto> GetAsync(int id)
        {
            var suggestion = await suggestionRepository.GetAsync(id);
            return mapper.Map<SuggestionDto>(suggestion);
        }

        public async Task MakeServed(int id)
        {
            var suggestion = await suggestionRepository.GetAsync(id);
            suggestion.MakeServed();
            await suggestionRepository.UpdateAsync(suggestion);
        }
    }
}
