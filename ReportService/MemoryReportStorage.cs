using ReportService.Models;

namespace ReportService
{
    public class MemoryReportStorage : IMemoryReportStorage
    {
        private IList<Report> reports = new List<Report>();
        public void Add(Report report)
        {
            reports.Add(report);
        }

        public IEnumerable<Report> Get()
        {
            return reports;
        }
    }
}
