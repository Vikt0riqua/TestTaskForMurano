using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SearchEngine.Models
{
    public class SearchResult
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(450)]
        public string Header { get; set; }
        [StringLength(450)]
        public string Link { get; set; }
        [StringLength(450)]
        public string ResultText { get; set; }
    }
    
}