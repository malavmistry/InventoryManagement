﻿using Inventory.Data;
using Inventory.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service
{
    public class ItemService
    {
        private readonly DatabaseContext _context;
        public ItemService(DatabaseContext context)
        {
            _context = context;
        }
        public async Task<bool> AddItems(Items item)
        {
            var items = await GetAllItems();
            if (items?.Any(x => x.UPC == item.UPC) ?? false)
                throw new ApplicationException($"Item with UPC-{item.UPC} already exists.");
            var result = await _context.AddItemAsync(item);
            await _context.GetAllAsync<Items>();
            return result;
        }
        public async Task<bool> UpdateItem(Items item)
        {
            var result = await _context.UpdateItemAsync(item);

            if (!result)
                throw new ApplicationException($"Item with UPC-{item.UPC} not updated");
            return result;
        }
        public async Task<List<Items>> GetAllItems()
        {
            var result = await _context.GetAllAsync<Items>();
            return result;
        }
        public async Task<bool> DeleteItem(Items item)
        {
            var result = await _context.DeleteItemAsync<Items>(item);
            return result;
        }
    }
}
