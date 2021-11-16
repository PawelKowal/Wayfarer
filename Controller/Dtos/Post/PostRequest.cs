namespace Controller.Dtos.Post
{
    public class PostRequest
    {
        public int PostId { get; set; }
        public string Content { get; set; }
        public string LocationDescription { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Image { get; set; }
    }
}
