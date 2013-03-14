using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllanXingCommonLibrary
{
    // This class implements IncrementalLoadingBase. 
    // To create your own Infinite List, you can create a class like this one that doesn't have 'generator' or 'maxcount', 
    //  and instead downloads items from a live data source in LoadMoreItemsOverrideAsync.
    public class GeneratorIncrementalLoadingClass<T> : IncrementalLoadingBase
        where T : class
    {
        uint _pageCount = 1;
        uint _maxPageCount = 0;

        public uint MaxPageCount
        {
            get { return _maxPageCount; }
            set { _maxPageCount = value; }
        }
        public GeneratorIncrementalLoadingClass(uint maxCount, Func<uint, Task<List<T>>> generator)
        {
            _generator = generator;
            _maxPageCount = _maxCount = maxCount;
        }

        protected async override Task<IList<object>> LoadMoreItemsOverrideAsync(System.Threading.CancellationToken c, uint count)
        {
            uint toGenerate = System.Math.Min(count, _maxCount - _count);
            Task<List<T>> task = _generator(_pageCount);
            List<T> value = await task;
            _pageCount++;
            return value.ToArray();
        }

        protected override bool HasMoreItemsOverride()
        {
            return HasMorePage() && HasMoreCount();
        }

        //judge total itemcount
        protected virtual bool HasMoreCount()
        {
            // return _count < _maxCount;
            return HasCount;
        }

        public bool HasCount = true;
        //judege pagecount
        protected virtual bool HasMorePage()
        {
            return _pageCount < _maxPageCount;
        }

        #region State

        Func<uint, Task<List<T>>> _generator;
        uint _count = 0;
        uint _maxCount;

        #endregion
    }
}
