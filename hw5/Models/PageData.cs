using cloudscribe.Pagination.Models;
using hw6.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace hw6.Models
{
    public static class PageData
    {
        public static PagedResult<T> GetPage<T>(List<T> viewModels, int totalItems, int pageNumber, int pageSize) where T : class, IDatabaseEntity
        {
            return new PagedResult<T>
            {
                Data = viewModels,
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
