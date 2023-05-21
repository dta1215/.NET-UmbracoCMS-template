using Microsoft.Identity.Client;
using System.Data;

namespace UmbracoBoutique.Services.DTO
{
    public class DataSet<T> 
    {
        public T Items { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; } = 8;
        public int TotalRows { get; set; }
        public int TotalPages 
        {
            get
            {
                return ((int)Math.Ceiling((double)TotalRows / (double)PageSize));
            }
            set
            {
                this.TotalPages = value;
            }
        }
        public string SearchValue { get; set; }
        public dynamic Other { get; set; }
    }
}
