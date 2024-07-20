using RingoMediaAssignment.DAL;

namespace RingoMediaAssignment.Services
{
    public class ReminderService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly EmailService _emailService;

        public ReminderService(IServiceProvider serviceProvider, EmailService emailService)
        {
            _serviceProvider = serviceProvider;
            _emailService = emailService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    var reminders = context.Reminders
                        .Where(r => r.ReminderDateTime <= DateTime.Now)
                        .ToList();

                    foreach (var reminder in reminders)
                    {
                        await _emailService.SendEmailAsync("recipient@example.com", reminder.Title, "This is your reminder!");
                        context.Reminders.Remove(reminder);
                    }

                    await context.SaveChangesAsync();
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // Check every minute
            }
        }
    }
}
