using AinAlfahd.Data;
using AinAlfahd.Models;
using Microsoft.EntityFrameworkCore;

namespace AinAlfahd.BL
{
    public interface ICustomer
    {
        Task<List<Customer>> GetAll();
        Task<Customer> GetByWord(string word);
        Task<Customer> GetByID(int id);
        Task<bool> AddData(Customer model);
        Task<bool> UpdateData(Customer model);
        Task<bool> DeleteData(int id);
    }

    public class CutomerRepo : ICustomer
    { 
        MasterDBContext db;
        public CutomerRepo(MasterDBContext db)
        {
            this.db = db;
        }

        public async Task<List<Customer>> GetAll()
        {
            var allData = await db.Customers.ToListAsync();
            return allData;
        }

        public async Task<Customer> GetByWord(string word)
        {
            var allData = await db.Customers.FirstOrDefaultAsync(c => c.CustMob.Contains(word) || c.CustName.Contains(word) || c.Id.ToString() == word);
            return allData;
        }

        public async Task<Customer> GetByID(int id)
        {
            var allData = await db.Customers.FindAsync(id);
            return allData;
        }

        public async Task<bool> AddData(Customer model)
        {
            try
            {
                await db.Customers.AddAsync(model);
                await db.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return false;
        }

        public async Task<bool> UpdateData(Customer model)
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

        public async Task<bool> DeleteData(int id)
        {
            try
            {
                var model = await GetByID(id);
                if (model == null)
                    return false;

                db.Customers.Remove(model);
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
