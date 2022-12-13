namespace AssetManagement.Contracts.Common
{
    public class ViewListPageResult<T> where T: class
    {
        public List<T> Data { get; set; }
        public int Total { get; set; }

    }
}
