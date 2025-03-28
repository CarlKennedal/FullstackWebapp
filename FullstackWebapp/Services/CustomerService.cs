using FullstackWebapp.Models;
using FullstackWebapp.Repositories;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

public class CustomerService(IRepository<Customer> customerRepo)
{
    public async Task<IEnumerable<Customer>> GetAllCustomersAsync() => await customerRepo.GetAllAsync();
    public async Task<Customer?> GetCustomerByIdAsync(int id) => await customerRepo.GetByIdAsync(id);
    public async Task DeleteCustomerAsync(int id) => await customerRepo.DeleteAsync(id);

    public async Task<IEnumerable<Customer>> SearchCustomersAsync(string query)
    {
        var customers = await customerRepo.GetAllAsync();
        return customers.Where(c =>
            c.FirstName.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            c.Surname.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            c.Email.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            c.MobilePhone.Contains(query, StringComparison.OrdinalIgnoreCase)
        );
    }
    public async Task AddCustomerAsync(Customer customer)
    {
        // Validate email uniqueness
        if (await EmailExistsAsync(customer.Email))
            throw new ValidationException("Email already exists");

        // Validate mobile phone format
        if (!IsValidPhoneNumber(customer.MobilePhone))
            throw new ValidationException("Invalid phone number format");

        await customerRepo.AddAsync(customer);
    }

    public async Task UpdateCustomerAsync(Customer customer)
    {
        // Check if customer exists
        var existing = await customerRepo.GetByIdAsync(customer.Id);
        if (existing == null)
            throw new KeyNotFoundException("Customer not found");

        // Validate email uniqueness (if changed)
        if (existing.Email != customer.Email && await EmailExistsAsync(customer.Email))
            throw new ValidationException("Email already exists");

        await customerRepo.UpdateAsync(customer);
    }

    private async Task<bool> EmailExistsAsync(string email)
        => await customerRepo.AsQueryable()
            .AnyAsync(c => c.Email == email);

    private static bool IsValidPhoneNumber(string phone)
        => !string.IsNullOrWhiteSpace(phone) && phone.Length >= 8;
}