using CarManagementSystem.Services.Dtos;


namespace CarManagementSystem.Services.Interfaces
{
    public interface IReportService
    {

        Task<MonthlyRevenueReportDto> GetMonthlyRevenueAsync(int month, int year);
    }
}
