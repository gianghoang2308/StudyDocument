using StudyDocument.Models;
using System.Collections.Generic;

namespace StudyDocument.Models
{
    public class SearchModel
    {
        public List<Image>? ImageList { get; set; }
        public List<Video>? VideoList { get; set; }
        public List<Document>? DocumentList { get; set; }
        public string? SearchTerm { get; set; }
    }
}
