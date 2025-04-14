namespace WebApi.Common
{
    public class BookViewModel
    {
        public string Title { get; set; }

        //buradaki genre mapp.profile aracılığıyla alınıyor
        public string Genre { get; set; }
        public int PageCount { get; set; }
        public string PublishDate { get; set; }

    }
}