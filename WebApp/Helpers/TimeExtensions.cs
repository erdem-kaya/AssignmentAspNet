namespace WebApp.Helpers;

public static class TimeExtensions
{
    public static string ToRemainingTime(this DateTime endDate)
    {
        var now = DateTime.UtcNow.Date;
        var remainingTime = (endDate.Date - now);

        if (remainingTime.TotalDays < 0)
        {
            return "Project is overdue";
        }

        if (remainingTime.TotalDays == 0)
        {
            return "Project is due today";
        }

        if (remainingTime.TotalDays < 7)
        {
            return $"{remainingTime.Days} day{(remainingTime.Days > 1 ? "s" : "")} left";
        }

        var weeks = (int)Math.Ceiling(remainingTime.TotalDays / 7);
        return $"{weeks} week{(weeks > 1 ? "s" : "")} left";
    }
}
