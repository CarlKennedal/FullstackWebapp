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

    public async Task<IEnumerable<Customer>> SearchCustomersAsync(
     int? id = null,
     string? firstName = null,
     string? lastName = null,
     string? email = null,
     string? phone = null)
    {
        var query = customerRepo.AsQueryable();

        if (id.HasValue)
            query = query.Where(c => c.Id == id.Value);

        if (!string.IsNullOrEmpty(firstName))
            query = query.Where(c => c.FirstName.Contains(firstName));

        if (!string.IsNullOrEmpty(lastName))
            query = query.Where(c => c.LastName.Contains(lastName));

        if (!string.IsNullOrEmpty(email))
            query = query.Where(c => c.Email.Contains(email));

        if (!string.IsNullOrEmpty(phone))
            query = query.Where(c => c.MobilePhone.Contains(phone));

        return await query.ToListAsync();
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
        existing.Addresses = customer.Addresses;

        await customerRepo.SaveChangesAsync();
    }

    private async Task<bool> EmailExistsAsync(string email)
        => await customerRepo.AsQueryable()
            .AnyAsync(c => c.Email == email);

    private static bool IsValidPhoneNumber(string phone)
        => !string.IsNullOrWhiteSpace(phone) && phone.Length >= 8;
}