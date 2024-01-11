using GenZStyleAPP.BAL.DTOs.FashionItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleAPP.BAL.Repository.Interfaces
{
    public interface IFashionItemRepository
    {
        public Task<List<GetFashionItemResponse>> SearchByFashionName(string fashionName);
    }
}
