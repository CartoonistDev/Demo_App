namespace Product.API.Jobs;

public interface IRecurringJobs
{
    Task ProcessEmailJobs();
    Task GenerateReports();

}
public class RecurringJobs : IRecurringJobs
{
    private readonly AppDbContext dbContext;


    public RecurringJobs(AppDbContext dbContext)
    {

        this.dbContext = dbContext;
    }

    [AutomaticRetry(Attempts = 3)]
    public async Task ProcessEmailJobs()
    {
        var emailJobs = await dbContext.EmailJobs.Where(e => !e.Sent).ToListAsync();

        foreach (var job in emailJobs)
        {
            // Simulate email sending logic
            Console.WriteLine($"Sending email to {job.EmailAddress}");
            job.Sent = true;

            dbContext.EmailJobs.Update(job);
            await dbContext.SaveChangesAsync();
        }
    }

    [AutomaticRetry(Attempts = 3)]
    public async Task GenerateReports()
    {
        var reportJobs = await dbContext.ReportJobs.Where(r => !r.Generated).ToListAsync();

        foreach (var job in reportJobs)
        {
            // Simulate report generation logic
            Console.WriteLine($"Generating report: {job.ReportName}");
            job.Generated = true;

            dbContext.ReportJobs.Update(job);
            await dbContext.SaveChangesAsync();
        }
    }
}
