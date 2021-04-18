using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Routine.Api.Helpers
{
    public class PageList<T> : List<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public PageList(List<T> items, int count, int pagenum, int pagesize)
        {
            TotalCount = count;
            PageSize = pagesize;
            CurrentPage = pagenum;
            TotalPages = (int)Math.Ceiling(TotalCount / (double)pagesize);//TotalCount % pagesize != 0 ? (TotalCount / pagesize) + 1 : (TotalCount / pagesize);
            AddRange(items);
        }

        public async static Task<PageList<T>> CreateAsync(IQueryable<T> source,int pagenum,int pagesize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip(pagesize * (pagenum - 1)).Take(pagesize).ToListAsync();
            return new PageList<T>(items, count, pagenum, pagesize);
        }
    }
}
