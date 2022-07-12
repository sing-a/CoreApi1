using System.Linq.Expressions;

namespace CoreApi1.IService
{
    public interface IBaseService<TEntity> where TEntity : class, new()
    {
        //查询表全部数据     
        public Task<List<TEntity>> QueryAsync();
        //新增
        public Task<int> Add(TEntity entity);
        //删除
        public Task<int> Del(Expression<Func<TEntity, bool>> delWhere);
        //修改
        public Task<int> EditAsync(TEntity entity);

    }
}
