using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using CoreApi1.Context;
using CoreApi1.IService;

namespace CoreApi1.Service
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class, new()
    {
        private readonly DemoContext _DbContext;
        public BaseService(DemoContext _DbContext)
        {
            this._DbContext = _DbContext;
            _DbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> Add(TEntity entity)
        {
            this._DbContext.Add(entity);
            return await this._DbContext.SaveChangesAsync();
        }
        /// <summary>
        /// 查询全部数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<TEntity>> QueryAsync()
        {
            //同步
            //return  this._DbContext.Set<TEntity>().ToList();
            //异步方法
            var data = await this._DbContext.Set<TEntity>().ToListAsync();
            return data;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> Del(Expression<Func<TEntity, bool>> delWhere)
        {
            List<TEntity> listDels = await _DbContext.Set<TEntity>().Where(delWhere).ToListAsync();
            listDels.ForEach(model =>
            {
                _DbContext.Entry(model).State = EntityState.Deleted;
            });
            return _DbContext.SaveChanges();
        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> EditAsync(TEntity entity)
        {
            this._DbContext.Entry(entity).State = EntityState.Modified;
            return await this._DbContext.SaveChangesAsync();
        }
    }
}


