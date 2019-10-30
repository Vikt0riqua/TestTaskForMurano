using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DA.Models
{
    public class SearchResult
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(450)]
        public string Header { get; set; }
        [StringLength(450)]
        public string Link { get; set; }
        public string ResultText { get; set; }

        public override bool Equals(object o)
        {
            if (ReferenceEquals(null, o)) return false;
            if (ReferenceEquals(this, o)) return true;
            if (o.GetType() != typeof(SearchResult)) return false;
            return Equals((SearchResult) o);
        }

        private bool Equals(SearchResult other)
        {
            return Id == other.Id && Header == other.Header && Link == other.Link && ResultText == other.ResultText;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id;
                hashCode = (hashCode * 397) ^ (Header != null ? Header.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Link != null ? Link.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ResultText != null ? ResultText.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
    
}