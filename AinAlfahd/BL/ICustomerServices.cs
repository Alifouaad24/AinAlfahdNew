using AinAlfahd.Data;
using AinAlfahd.Models;
using Microsoft.EntityFrameworkCore;

namespace AinAlfahd.BL
{
    public interface ICustomerServices
    {
        Task<List<CustomerService>> GetAll();
        Task<CustomerService> GetByPier(int cusId, int serId);
        Task<bool> AddData(CustomerService model);
        Task<bool> UpdateData(CustomerService model);
        Task<bool> DeleteData(int cusId, int serId);
    }

    public class CustomerServices : ICustomerServices
    {
        MasterDBContext db;
        public CustomerServices(MasterDBContext db)
        {
            this.db = db;
        }

        public async Task<List<CustomerService>> GetAll()
        {
            var allData = await db.CustomerServices.ToListAsync();
            return allData;
        }

        public async Task<CustomerService> GetByPier(int cusId, int serId)
        {
            var allData = await db.CustomerServices.FirstOrDefaultAsync(c => c.CustomerId == cusId || c.ServiceId == serId);
            return allData;
        }


        public async Task<bool> AddData(CustomerService model)
        {
            try
            {
                await db.CustomerServices.AddAsync(model);
                await db.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return false;
        }

        public async Task<bool> UpdateData(CustomerService model)
        {
            try
            {
                db.Entry(model).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;
            }
        }

        public async Task<bool> DeleteData(int cusId, int serId)
        {
            try
            {
                var model = await GetByPier(cusId, serId);
                if (model == null)
                    return false;

                db.CustomerServices.Remove(model);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
