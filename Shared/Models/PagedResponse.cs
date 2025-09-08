using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shared.Models.DTOs.ProductTypesDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class PagedResponse<T>
    {
        private PagedResponse(List<T> items, int page, int totalCount)
        {
            Items = items;
            Page = page;
            TotalCount = totalCount;
        }
        public List<T> Items { get; set; }
        public int Page {  get; }
        //public int PageSize { get; }
        public int TotalCount { get; }  
        
        public static async Task<PagedResponse<T>> CreateAsync(IQueryable<T> query, int page)
        {
            int pageSize = 20;
            var totalCount = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new(items, page, totalCount);
        }

        public PagedResponse<TMap> Map<TMap>(IMapper mapper)
        {
            var mappedItems = mapper.Map<List<TMap>>(Items);
            return new PagedResponse<TMap>(mappedItems, Page, TotalCount);
        }
    }
}
