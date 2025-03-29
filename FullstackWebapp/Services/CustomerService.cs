using FullstackWebapp.Models;
using FullstackWebapp.Repositories;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

public class CustomerService(IRepository<Customer> customerRepo)
{
    public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        => await customerRepo.GetAllAsync();

    public async Task<Customer?> GetCustomerByIdAsync(int id)
        => await customerRepo.GetByIdAsync(id);

    public async Task<bool> DeleteCustomerAsync(int id)
    {
        var customer = await customerRepo.GetByIdAsync(id);
        if (customer == null) return false;

        await customerRepo.DeleteAsync(id);
        await customerRepo.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Customer>> SearchCustomersAsync(string query)
    {
        return await customerRepo.AsQueryable()
            .Where(c =>
                c.FirstName.Contains(query) ||
                c.LastName.Contains(query) ||
                c.Email.Contains(query) ||
                c.MobilePhone.Contains(query))
            .ToListAsync();
    }

    public async Task<Customer> AddCustomerAsync(Customer customer)
    {
        if (await EmailExistsAsync(customer.Email))
            throw new ValidationException("Email exists");
        if (!IsValidPhoneNumber(customer.MobilePhone))
            throw new ValidationException("Invalid phone number");

        await customerRepo.AddAsync(customer);
        await customerRepo.SaveChangesAsync();
        return customer;
    }

    public async Task UpdateCustomerAsync(Customer customer)
    {
        var existing = await customerRepo.GetByIdAsync(customer.Id);
        if (existing == null)
            throw new KeyNotFoundException("Customer not found");

        if (existing.Email != customer.Email && await EmailExistsAsync(customer.Email))
            throw new ValidationException("Email exists");

        if (!IsValidPhoneNumber(customer.MobilePhone))
            throw new ValidationException("Invalid phone number");

        existing.FirstName = customer.FirstName;
        existing.LastName = customer.LastName;
        existing.Email = customer.Email;
        existing.MobilePhone = customer.MobilePhone;

        await customerRepo.SaveChangesAsync();
    }

    private async Task<bool> EmailExistsAsync(string email)
        => await customerRepo.AsQueryable()
            .AnyAsync(c => c.Email == email);

    private static bool IsValidPhoneNumber(string phone)
        => !string.IsNullOrWhiteSpace(phone) && phone.Length >= 8;
}