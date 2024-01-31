using GP.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GP.Core.Specificatios
{
    public class BaseSpecification<T> : ISpecification<T> where T :BaseEntity
    {
        public Expression<Func<T, bool>> criteria { get; set; }//where
        public List<Expression<Func<T, object>>> includes { get; set; } = new List<Expression<Func<T, object>>>(); //include

        public BaseSpecification()
        {
          
        }

        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            this.criteria = criteria;

                        
        }

    }
}
