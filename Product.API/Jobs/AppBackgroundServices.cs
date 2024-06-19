namespace Product.API.Jobs;

public class AppBackgroundServices
{
    public static void RecurringJobsSchedule()
    {
        RecurringJob.AddOrUpdate<IRecurringJobs>("send-emails", service => service.ProcessEmailJobs(), Cron.Hourly);
        RecurringJob.AddOrUpdate<IRecurringJobs>("generate-reports", service => service.GenerateReports(), Cron.Daily);

    }
}
