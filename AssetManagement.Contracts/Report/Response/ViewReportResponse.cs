namespace AssetManagement.Contracts.Report.Response
{
    public class ViewReportResponse
    {
        public string ID { get; set; }
        public string Category { get; set; }
        public int Total { get; set; }
        public int Assigned { get; set; }
        public int Available { get; set; }
        public int NotAvailable { get; set; }
        public int WaitingForRecycling { get; set; }
        public int Recycled { get; set; }
    }
}
