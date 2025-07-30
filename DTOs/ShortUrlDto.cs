namespace EncurtadorUrl.DTOs
{
    public class UrlOriginalDto
    {
        public string UrlOriginal { get; set; }
        public DateTime? DataExpiracao { get; set; }
    }
    public class LinkPersonalizadoDto
    {
        public string UrlOriginal { get; set; }
        public string LinkPersonalizado { get; set; }
        public DateTime? DataExpiracao { get; set; }
    }
}