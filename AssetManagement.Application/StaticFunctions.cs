using AssetManagement.Domain.Models;

namespace AssetManagement.Application
{
    public static class StaticFunctions<T> where T: class
    {
        public static List<T> Paging(IQueryable<T> records, int start, int end)
        {
            if(records.Count() == 0)
            {
                return new List<T>();
            }
            if(start < 0 || start > end || start >= records.Count())
            {
                start = 1;
            }
            if(end > records.Count())
            {
                end = records.Count();
            }
            return records.Skip(start).Take(end - start).ToList();
        }

        //public static IQueryable<T> Sort(IQueryable<T> dataList, string sort, string order)
        //{
        //    sort = ConvertCamelToTitle(sort);
        //    var prop = typeof(Asset).GetProperty(sort);
        //    if (order == "ASC")
        //    {
        //        return dataList
        //            .OrderBy(data => prop.GetValue(data, null));
        //    }
        //    else
        //    {
        //        return dataList
        //            .OrderByDescending(data => prop.GetValue(data, null));
        //    }
        //}

        //public static string ConvertCamelToTitle(string camelString)
        //{
        //    string titleString = camelString.Substring(0, 1).ToUpper() + camelString.Substring(1);
        //    return titleString;
        //}
    }
}
