using System;
using IMS.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace IMS.Data.Repositories;

public class ContactTypeRepository: IContactTypeRepository
{
    private readonly IMSDbContext _context;

    public ContactTypeRepository(IMSDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<ContactType>> GetAllAsync()=>
        await _context.ContactTypes.ToListAsync();

}