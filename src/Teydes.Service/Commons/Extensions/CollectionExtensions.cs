using Newtonsoft.Json;
using Teydes.Domain.Commons;
using Teydes.Domain.Configurations;
using Teydes.Service.Commons.Exceptions;
using Teydes.Service.Commons.Helpers;

namespace Teydes.Service.Commons.Extensions;

public static class CollectionExtensions_
{
    public static IQueryable<TEntity> ToPagedList<TEntity>(this IQueryable<TEntity> entities, PaginationParams @params)
           where TEntity : Auditable
    {
      
        var metaData = new PaginationMetaData(entities.Count(), @params);

        var json = JsonConvert.SerializeObject(metaData);
        if (HttpContextHelper.ResponseHeaders != null)
        {
            if (HttpContextHelper.ResponseHeaders.ContainsKey("X-Pagination"))
                HttpContextHelper.ResponseHeaders.Remove("X-Pagination");

            HttpContextHelper.ResponseHeaders.Add("X-Pagination", json);
        }

        return @params.PageIndex > 0 && @params.PageSize > 0 ?
            entities
            .OrderBy(e => e.Id)
            .Skip((@params.PageIndex - 1) * @params.PageSize).Take(@params.PageSize)
            : throw new CustomException(400, "Please, enter valid numbers");
    }

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable)
    {
        var list = enumerable.ToList();
        var rng = new Random();
        int n = list.Count;
        while (n > 1)
        {
            int k = rng.Next(n--);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }

        foreach (var item in list)
        {
            yield return item;
        }
    }
}
