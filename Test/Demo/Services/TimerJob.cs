namespace Demo.Services
{
    using Hangfire;
    using Qx.Sprite.BackgroundJob;
    using Qx.Sprite.Core;

    public class TimerJob : ISheduledTask ,IScoped
    {
        public string Title => "TimerJob";

        public string CronExpression { get; set; } = Cron.Hourly();

        public Task Start()
        {
            return Task.CompletedTask;
        }
    }
}
