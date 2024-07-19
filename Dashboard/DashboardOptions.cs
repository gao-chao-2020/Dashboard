namespace Dashboard;
public class DashboardOptions
{
    public DashboardOptions()
    {
        PathBase = string.Empty;
        PathMatch = "/dashboard";
        StatsPollingInterval = 1000;
    }
    public string PathBase { get; set; }
    public string PathMatch { get; set; }
    public int StatsPollingInterval { get; set; }
}