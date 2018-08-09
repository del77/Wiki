namespace Wiki.Infrastructure.DTO
{
    public class ArticleDetailsDto
    {
        public int Id { get; set; }
        public ArticleCategoryDto Category { get; set; }
        public TextDetailsDto Master { get; set; }
    }
}